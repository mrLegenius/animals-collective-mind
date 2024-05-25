using LevAI.UtilityAI;
using Lions.AI.Contexts;
using Lions.Animals;
using Lions.Animals.Lion;
using UnityEngine;

namespace Lions.AI
{
    public class RestAction : ActionBase<RestPlaceContext>
    {
        public override int GetBaseRank(IAgent agent) => 1;

        public override void Execute(IAgent agent, RestPlaceContext context)
        {
            var animal = agent.GetData<Lion>(AnimalBlackboardKeys.Animal);
            var stats = agent.GetData<AnimalStats>(AnimalBlackboardKeys.Stats);
            var state = agent.GetData<AnimalState>(AnimalBlackboardKeys.State);
            var restPlace = context.Source;
            
            var restRate = stats.RestRate;
            var thirstGainRate = stats.ThirstGainRate;
            var thirst = state.Thirst;
            var energy = state.Energy;
            
            restPlace.Use(animal);
            energy.Change(restRate * Time.deltaTime);
            thirst.Change(thirstGainRate * Time.deltaTime);
            
            var hunger = state.Hunger;
            var hungerGainRate = stats.HungerGainRate;
            hunger.Change(hungerGainRate * Time.deltaTime);
        }

        public override bool IsValid(IAgent agent, RestPlaceContext context) => context.SqrDistance < 3.0f;

        public override bool IsFinished(IAgent agent, RestPlaceContext context) => 
            agent.GetData<AnimalState>(AnimalBlackboardKeys.State).Energy.IsFull;
    }
}