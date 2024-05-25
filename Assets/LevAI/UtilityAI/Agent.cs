using System;
using System.Collections.Generic;
using System.Linq;
using LevAI.WorkingMemory;
using UnityEngine;
using UnityEngine.Profiling;
using Object = UnityEngine.Object;

namespace LevAI.UtilityAI
{
    public class Agent : IAgent
    {
        private readonly string _id;
        private readonly List<AIBehavior> _behaviors;
        private readonly IActionSelector _actionSelector;
        public IAction CurrentAction => CurrentOption.Action;
        public IContext CurrentActionContext => CurrentOption.Context;
        public Option CurrentOption { get; private set; }

        private float _thinkTimer;
        private const float ThinkInterval = 0.1f;
        
        private float _memoryUpdateTimer;
        private const float MemoryUpdateInterval = 5.0f;
        public bool ShowLogs { get; set; }

        public string ActionGroup { get; private set; } = string.Empty;

        public event Action<string> ActionGroupChanged;

        private readonly List<SelectorActions> _actions;
        private readonly IEnumerable<IContextProducer> _contextProducers;
        private readonly List<float> _utilities = new();

        public IAgentMemory Memory { get; } = new AgentMemory();

        private IUpdatableMemory UpdatableMemory => (IUpdatableMemory)Memory;

        public Agent(Object id, List<AIBehavior> behaviors, IActionSelector actionSelector) : this(id.GetInstanceID().ToString(), behaviors, actionSelector)
        {
        }
        
        public Agent(string id, List<AIBehavior> behaviors, IActionSelector actionSelector)
        {
            _id = id;
            _behaviors = behaviors;
            _actionSelector = actionSelector;
            _actions = new List<SelectorActions>(_behaviors.Count);
            _contextProducers = behaviors.Select(x => x.ContextProducer).Where(x => x != null);
        }

        public void Update(float deltaTime)
        {
            Profiler.BeginSample("Agent Update");
            
            UpdateMemory(deltaTime);
            UpdateThinkTimer(deltaTime);

            if (CurrentAction == null)
            {
                Think();
                Profiler.EndSample();
                return;
            }

            var context = CurrentOption.Context;
            
            if ((context != null && !context.IsValid()) 
                || !CurrentAction.IsValid(this, context))
            {
                FinishAction();
                Profiler.EndSample();
                return;
            }
            
            CurrentAction.Execute(this, context);

            if (CurrentAction.IsFinished(this, context))
            {
                FinishAction();
            }
            
            Profiler.EndSample();
        }

        private void UpdateThinkTimer(float deltaTime)
        {
            _thinkTimer += deltaTime;
            if (_thinkTimer < ThinkInterval) return;
            
            _thinkTimer -= ThinkInterval;
            Think();
        }

        private void UpdateMemory(float deltaTime)
        {
            _memoryUpdateTimer += deltaTime;
            if (_memoryUpdateTimer < MemoryUpdateInterval) return;
            _memoryUpdateTimer -= MemoryUpdateInterval;
            
            Profiler.BeginSample("Update memory");
            UpdatableMemory.Update();
            Profiler.EndSample();
        }

        private void FinishAction()
        {
            CurrentAction.OnFinished(this);
            CurrentOption = default;
            _thinkTimer = 0;
            Think();
        }
        
        private void Think()
        {
            CurrentOption = GetBestDecision();

            string newGroup = CurrentOption.Action?.Group ?? string.Empty;

            if (ActionGroup != newGroup)
            {
                ActionGroup = newGroup;
                ActionGroupChanged?.Invoke(newGroup);
            }
        }

        public Option GetBestDecision()
        {
            Profiler.BeginSample("Get Best Decision");
            
            _actions.Clear();

            Profiler.BeginSample("Context Generation");
            
            foreach (IContextProducer contextProducer in _contextProducers) 
                contextProducer.Produce(this);

            Profiler.EndSample();
            
            string log = string.Empty;
            if (ShowLogs)
                log = "Action process: \n";
            
            Profiler.BeginSample("Action Score Calculation");
            
            foreach (var behavior in _behaviors)
            {
                var action = behavior.Action;
                string actionName = action.GetType().Name;
                
                if (behavior.ContextProducer != null)
                {
                    foreach (var context in behavior.ContextProducer.Context)
                    {
                        TryCreateOptionToConsider(action, context, actionName, behavior);
                    }
                }
                else
                {
                    TryCreateOptionToConsider(action, null, actionName, behavior);
                }
            }
            Profiler.EndSample();

            Profiler.BeginSample("Action Selection");
            var bestOption = _actionSelector.SelectAction(_actions);
            Profiler.EndSample();

            if (ShowLogs)
            {
                log += "\n";
                if (bestOption.Action != null)
                    log += $"Selecting '{bestOption.Action.GetType().Name}' Action{GetContextLog(bestOption.Context)}";
                else
                    log += "Selecting no action";
                
                Debug.Log(log);
            }
                

            Profiler.EndSample();
            
            return bestOption;

            void TryCreateOptionToConsider(IAction action, IContext context, string actionName, AIBehavior behavior)
            {
                if (context != null && !context.IsValid())
                {
                    if (ShowLogs)
                        log += $"Action '{actionName}' context is not valid \n";
                    
                    return;
                }
                
                if (!action.IsValid(this, context))
                {
                    if (ShowLogs)
                        log += $"Action '{actionName}'{GetContextLog(context)}is not valid \n";
                    
                    return;
                }

                Profiler.BeginSample("TryCreateOptionToConsider");
                
                float calculatedScore = CalculateScore(behavior, context);
                    
                var rank = action.GetBaseRank(this) + CalculateRank(behavior, context);

                if (action.Equals(CurrentAction) && (context == null || context.Equals(CurrentActionContext)))
                    rank++;

                if (!string.IsNullOrEmpty(ActionGroup) && ActionGroup == action.Group)
                    rank += 10;
            
                if (ShowLogs)
                    log += $"Action '{actionName}'{GetContextLog(context)}score is {calculatedScore} and rank is {rank}\n";
                
                _actions.Add(new SelectorActions
                {
                    Action = action,
                    Score = calculatedScore,
                    Rank = rank,
                    Context = context,
                });
                
                Profiler.EndSample();
            }
        }

        public T GetData<T>(string key) => Memory.GetData<T>(_id, key);
        public void SetData<T>(string key, T data, int priority) => Memory.InsertData(_id, key, data, priority);
        public T GetValue<T>(Object obj, string key) => Memory.GetData<T>(obj, key);
        public T GetValue<T>(string objectId, string key) => Memory.GetData<T>(objectId, key);
        public void SetData<T>(string objectId, string key, T data, int priority) => Memory.InsertData(objectId, key, data, priority);
        public void RemoveData(string objectId, string key) => Memory.RemoveData(objectId, key);
        public void RemoveData(string key) => Memory.RemoveData(_id, key);

        private float CalculateScore(AIBehavior behavior, IContext context)
        {
            Profiler.BeginSample("CalculateScore");
            
            _utilities.Clear();

            string log = string.Empty;
            
            if (ShowLogs)
                log = "Consideration process: \n";
            foreach (var consideration in behavior.Considerations)
            {
                var score = consideration.Execute(this, context);

                if (ShowLogs)
                    log += $"{consideration.GetType().Name} has score {score} \n";
                _utilities.Add(score);
            }

            float totalScore = behavior.Combiner.Combine(_utilities);

            if (ShowLogs)
                log += $"Total Score is {totalScore}";

            if (ShowLogs)
                Debug.Log(log);

            Profiler.EndSample();
               
            return totalScore;
        }
        
        private int CalculateRank(AIBehavior behavior, IContext context)
        {
            var maxRank = 0;
            foreach (IConsideration consideration in behavior.Considerations)
            {
                int rank = consideration.CalculateRank(this, behavior.Action, context);
                if (rank > maxRank)
                    maxRank = rank;
            }
            
            return maxRank;
        }

        private static string GetContextLog(IContext context) => context == null ? " " : $" ({context.Name}) ";
    }
}
