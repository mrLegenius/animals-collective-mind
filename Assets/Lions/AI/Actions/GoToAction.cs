using System;
using LevAI.UtilityAI;
using Lions.Animals;
using Lions.Animals.Lion;
using UnityEngine;

namespace Lions.AI
{
    public class RunToSpecificPoint : GoToSpecificPoint
    {
        protected override float Speed(IAgent agent) => agent.GetData<AnimalStats>(AnimalBlackboardKeys.Stats).RunSpeed;
        protected override float EnergyLoss(IAgent agent) => agent.GetData<AnimalStats>(AnimalBlackboardKeys.Stats).EnergyLossRateOnRun;
        
        public RunToSpecificPoint(Func<IAgent, Vector3> placeGetter, int rank = 0, string group = "", Action<IAgent> onFinished = null) : base(placeGetter, rank, group, onFinished)
        {
            
        }
    }
    
    public class GoToSpecificPoint : ActionBase
    {
        public override int GetBaseRank(IAgent agent) => _baseRank;

        public override string Group { get; }

        private readonly int _baseRank;
        private readonly Func<IAgent, Vector3> _placeGetter;
        private readonly Action<IAgent> _onFinished;

        public GoToSpecificPoint(Func<IAgent, Vector3> placeGetter, int rank = 0, string group = "", Action<IAgent> onFinished = null)
        {
            _baseRank = rank;
            Group = group;
            _placeGetter = placeGetter;
            _onFinished = onFinished;
        }

        protected virtual float Speed(IAgent agent) => agent.GetData<AnimalStats>(AnimalBlackboardKeys.Stats).Speed;
        protected virtual float EnergyLoss(IAgent agent) => agent.GetData<AnimalStats>(AnimalBlackboardKeys.Stats).EnergyLossRateOnWalk;
        public override void Execute(IAgent agent)
        {
            var stats = agent.GetData<AnimalStats>(AnimalBlackboardKeys.Stats);
            var state = agent.GetData<AnimalState>(AnimalBlackboardKeys.State);
            
            var thirstGainRate = stats.ThirstGainRate;
            var thirst = state.Thirst;
            thirst.Change(thirstGainRate * Time.deltaTime);
            
            var energy = state.Energy;
            var energyLossRate = EnergyLoss(agent);
            energy.Change(-energyLossRate * Time.deltaTime);
            
            var hunger = state.Hunger;
            var hungerGainRate = stats.HungerGainRate;
            hunger.Change(hungerGainRate * Time.deltaTime);
            
            var target = _placeGetter(agent);
            var animal = agent.GetData<Animal>(AnimalBlackboardKeys.Animal);
            animal.MoveTo(target, Speed(agent));
        }
        public override void OnFinished(IAgent agent)
        {
            _onFinished?.Invoke(agent);
        }

        public override void OnInterrupted(IAgent agent) => 
            agent.GetData<Animal>(AnimalBlackboardKeys.Animal).StopMoving();

        public override bool IsValid(IAgent agent) => agent.GetData<AnimalState>(AnimalBlackboardKeys.State).Energy.Value > 0;

        public override bool IsFinished(IAgent agent)
        {
            var target = _placeGetter(agent);
            return agent.GetSqrDistance(target) < 2.0f;
        }
    }

    
    public interface IGoToContext : IContext
    {
        Transform Target { get; }
        float SqrDistance { get; }
    }

    public class RunToSpecificPlace : GoToSpecificPlace
    {
        protected override float Speed(IAgent agent) => agent.GetData<AnimalStats>(AnimalBlackboardKeys.Stats).RunSpeed;
        protected override float EnergyLoss(IAgent agent) => agent.GetData<AnimalStats>(AnimalBlackboardKeys.Stats).EnergyLossRateOnRun;
        
        public RunToSpecificPlace(Func<IAgent, Transform> placeGetter, int rank = 0, string group = "", Action<IAgent> onFinished = null, Func<IAgent, Vector3> placeOffset = null) : base(placeGetter, rank, group, onFinished, placeOffset)
        {
            
        }
    }
    
    public class GoToSpecificPlace : ActionBase
    {
        public override int GetBaseRank(IAgent agent) => _baseRank;

        public override string Group { get; }

        private readonly int _baseRank;
        private readonly Func<IAgent, Transform> _placeGetter;
        private readonly Func<IAgent, Vector3> _placeOffset;
        private readonly Action<IAgent> _onFinished;

        public GoToSpecificPlace(Func<IAgent, Transform> placeGetter, int rank = 0, string group = "", Action<IAgent> onFinished = null, Func<IAgent, Vector3> placeOffset = null)
        {
            _baseRank = rank;
            Group = group;
            _placeGetter = placeGetter;
            _placeOffset = placeOffset;
            _onFinished = onFinished;
        }

        protected virtual float Speed(IAgent agent) => agent.GetData<AnimalStats>(AnimalBlackboardKeys.Stats).Speed;
        protected virtual float EnergyLoss(IAgent agent) => agent.GetData<AnimalStats>(AnimalBlackboardKeys.Stats).EnergyLossRateOnWalk;
        public override void Execute(IAgent agent)
        {
            var stats = agent.GetData<AnimalStats>(AnimalBlackboardKeys.Stats);
            var state = agent.GetData<AnimalState>(AnimalBlackboardKeys.State);
            
            var thirstGainRate = stats.ThirstGainRate;
            var thirst = state.Thirst;
            thirst.Change(thirstGainRate * Time.deltaTime);
            
            var energy = state.Energy;
            var energyLossRate = EnergyLoss(agent);
            energy.Change(-energyLossRate * Time.deltaTime);
            
            var hunger = state.Hunger;
            var hungerGainRate = stats.HungerGainRate;
            hunger.Change(hungerGainRate * Time.deltaTime);
            
            var target = _placeGetter(agent);
            var animal = agent.GetData<Animal>(AnimalBlackboardKeys.Animal);
            Vector3 offset = _placeOffset?.Invoke(agent) ?? Vector3.zero;
            animal.MoveTo(target.position + offset, Speed(agent));
        }
        public override void OnFinished(IAgent agent)
        {
            _onFinished?.Invoke(agent);
        }

        public override void OnInterrupted(IAgent agent) => 
            agent.GetData<Animal>(AnimalBlackboardKeys.Animal).StopMoving();

        public override bool IsValid(IAgent agent)
        {
            var target = _placeGetter(agent);
            
            return agent.GetData<AnimalState>(AnimalBlackboardKeys.State).Energy.Value > 0
                && target != null;
        }

        public override bool IsFinished(IAgent agent)
        {
            var target = _placeGetter(agent);
            return agent.GetSqrDistance(target) < 2.0f;
        }
    }

    
    public class GoToAction : ActionBase<IGoToContext>
    {
        public override string Group { get; }
        private int BaseRank { get; }

        private readonly Action<IAgent> _onFinished;
        
        public override int GetBaseRank(IAgent agent) => BaseRank + (!string.IsNullOrEmpty(Group) && agent.ActionGroup == Group ? 10 : 0);

        public GoToAction(int baseRank = 0, string group = "", Action<IAgent> onFinished = null)
        {
            _onFinished = onFinished;
            BaseRank = baseRank;
            Group = group;
        }

        protected virtual float Speed(IAgent agent) => agent.GetData<AnimalStats>(AnimalBlackboardKeys.Stats).Speed;
        protected virtual float EnergyLoss(IAgent agent) => agent.GetData<AnimalStats>(AnimalBlackboardKeys.Stats).EnergyLossRateOnWalk;
        
        public override void Execute(IAgent agent, IGoToContext context)
        {
            var stats = agent.GetData<AnimalStats>(AnimalBlackboardKeys.Stats);
            var state = agent.GetData<AnimalState>(AnimalBlackboardKeys.State);
            
            var thirstGainRate = stats.ThirstGainRate;
            var thirst = state.Thirst;
            thirst.Change(thirstGainRate * Time.deltaTime);
            
            var energy = state.Energy;
            var energyLossRate = EnergyLoss(agent);
            energy.Change(-energyLossRate * Time.deltaTime);
            
            var hunger = state.Hunger;
            var hungerGainRate = stats.HungerGainRate;
            hunger.Change(hungerGainRate * Time.deltaTime);
            
            var animal = agent.GetData<Animal>(AnimalBlackboardKeys.Animal);
            animal.MoveTo(context.Target.position, Speed(agent));
        }

        public override void OnFinished(IAgent agent) => _onFinished?.Invoke(agent);

        public override void OnInterrupted(IAgent agent) => 
            agent.GetData<Animal>(AnimalBlackboardKeys.Animal).StopMoving();

        public override bool IsValid(IAgent agent, IGoToContext context) =>
            agent.GetData<AnimalState>(AnimalBlackboardKeys.State).Energy.Value > 0;
        
        public override bool IsFinished(IAgent agent, IGoToContext context) => 
            context.SqrDistance < 2.0f;
    }
}