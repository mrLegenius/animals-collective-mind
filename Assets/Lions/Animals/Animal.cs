using LevAI.Perception;
using LevAI.UtilityAI;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Profiling;

namespace Lions.Animals
{
    public abstract class Animal : MonoBehaviour, IDangerSource
    {
        [SerializeField] public AnimalState State;
        [SerializeField] public AnimalStats Stats;
        
        [SerializeField] protected NavMeshAgent _navMeshAgent;
        [SerializeField] protected World _world;
        [SerializeField] protected bool _showLogs;
        [SerializeField] private float _deathTime = 10.0f;
        [SerializeField] protected Perception _perception;
        [SerializeField] protected DangerSensor _dangerSensor;
        
        private string _state;
        public Agent Agent;
        
        private Transform _moveTarget;

        private float _deathTimer;

        public float Danger => Stats.Danger;
        public World World => _world;

        private void Awake()
        {
            Agent = CreateBrains();
            
            CollectObservation();
            
            _perception.SetAgent(Agent.Memory);
        }
        
        public void Update()
        {
            Agent.ShowLogs = _showLogs;
            
            StopMoving();
            
            Profiler.BeginSample("AI Loop");
            
            Profiler.BeginSample("CollectObservation");
            
            CollectObservation();
            
            Profiler.EndSample();
            Agent.Update(Time.deltaTime);
            
            Profiler.EndSample();
            
            Profiler.BeginSample("Animal State Update");
            
            if (Agent.CurrentAction == null)
            {
                State.Energy.Change(-Stats.EnergyLossRate * Time.deltaTime);
                State.Thirst.Change(Stats.ThirstGainRate * Time.deltaTime);
                State.Hunger.Change(Stats.HungerGainRate * Time.deltaTime);

                _state = "None";
            }
            else
            {
                _state = $"{Agent.CurrentAction.GetType().Name} {Agent.CurrentOption.Context?.Name ?? ""}";
            }

            if (State.Energy.Value <= 0.0f || State.Hunger.NormalizedValue >= 1.0f || State.Thirst.NormalizedValue >= 1.0f)
            {
                _deathTimer += Time.deltaTime;
            }
            else
            {
                _deathTimer -= Time.deltaTime;
            }

            if (_deathTimer < 0.0f)
            {
                _deathTimer = 0.0f;
            }
            else if (_deathTimer >= _deathTime)
            {
                OnDied();
                Destroy(gameObject);
                Debug.Log($"Animal '{name}' died");
            }
            
            Profiler.EndSample();
        }
        
        protected abstract Agent CreateBrains();
        
        protected virtual void OnDied() { }
        protected virtual void CollectObservation()
        {
            Agent.SetData(AnimalBlackboardKeys.State, State, int.MaxValue);
            Agent.SetData(AnimalBlackboardKeys.Animal, this, int.MaxValue);
            Agent.SetData(AnimalBlackboardKeys.Stats, Stats, int.MaxValue);
            
            Agent.SetData(AnimalBlackboardKeys.AllWaterSources,
                _world.GetClosestAvailableWaterSources(transform.position, gameObject), 10);

            if (_dangerSensor)
                Agent.SetData(AnimalBlackboardKeys.Fear, _dangerSensor.Fear, int.MaxValue);
        }
        
        public void MoveTo(Vector3 position, float speed)
        {
            _navMeshAgent.isStopped = false;
            _navMeshAgent.speed = speed;
            _navMeshAgent.SetDestination(position);
        }

        public void StopMoving()
        {
            _navMeshAgent.isStopped = true;
        }
    }
}