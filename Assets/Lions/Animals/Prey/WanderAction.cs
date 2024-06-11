using LevAI.UtilityAI;
using UnityEngine;

namespace Lions.Animals.Prey
{
    public class WanderAction : ActionBase
    {
        private const int WanderRadius = 10;
        private bool _isWandering;
        private Vector3 _moveTarget;

        public override void Execute(IAgent agent)
        {
            var animal = agent.GetData<Animal>(AnimalBlackboardKeys.Animal);
            if (!_isWandering)
            {
                Vector2 randomCircle = Random.insideUnitCircle;
                _moveTarget = animal.transform.position + new Vector3(randomCircle.x, 0, randomCircle.y) * WanderRadius;
            }
            _isWandering = true;
            
            var stats = agent.GetData<AnimalStats>(AnimalBlackboardKeys.Stats);
            var state = agent.GetData<AnimalState>(AnimalBlackboardKeys.State);
            
            animal.MoveTo(_moveTarget, stats.Speed);
            
            var energy = state.Energy;
            var energyLossRateOnWalk = stats.EnergyLossRateOnWalk;
            energy.Change(-energyLossRateOnWalk * Time.deltaTime);
            
            var thirst = state.Thirst;
            var thirstGainRate = stats.ThirstGainRate;
            thirst.Change(thirstGainRate * Time.deltaTime);
            
            var hunger = state.Hunger;
            var hungerGainRate = stats.HungerGainRate;
            hunger.Change(hungerGainRate * Time.deltaTime);
        }

        public override bool IsValid(IAgent agent) => 
            agent.GetData<AnimalState>(AnimalBlackboardKeys.State).Energy.Value > 0;

        public override void OnFinished(IAgent agent)
        {
            _isWandering = false;
        }

        public override bool IsFinished(IAgent agent)
        {
            var animal = agent.GetData<Animal>(AnimalBlackboardKeys.Animal);
            return (animal.transform.position - _moveTarget).sqrMagnitude < 0.1f;
        }
    }
}