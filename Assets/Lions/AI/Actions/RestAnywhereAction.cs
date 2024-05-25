
using LevAI.UtilityAI;
using Lions.Animals;
using UnityEngine;

namespace Lions.AI
{
    public class RestAnywhereAction : ActionBase
    {
        public override int GetBaseRank(IAgent agent) => -5;

        public override void Execute(IAgent agent)
        {
            var stats = agent.GetData<AnimalStats>(AnimalBlackboardKeys.Stats);
            var state = agent.GetData<AnimalState>(AnimalBlackboardKeys.State);
            
            var restRate = stats.RestRate;
            var thirstGainRate = stats.ThirstGainRate;
            var thirst = state.Thirst;
            var energy = state.Energy;
            
            energy.Change(0.5f * restRate * Time.deltaTime);
            thirst.Change(1.5f * thirstGainRate * Time.deltaTime);
            
            var hunger = state.Hunger;
            var hungerGainRate = stats.HungerGainRate;
            hunger.Change(1.5f * hungerGainRate * Time.deltaTime);
        }

        public override bool IsFinished(IAgent agent) => 
            agent.GetData<AnimalState>(AnimalBlackboardKeys.State).Energy.IsFull;
    }
}