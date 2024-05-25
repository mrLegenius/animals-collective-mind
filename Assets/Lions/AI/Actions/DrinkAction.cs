using LevAI.UtilityAI;
using Lions.AI.Contexts;
using Lions.Animals;
using UnityEngine;

namespace Lions.AI
{
    public class DrinkAction : ActionBase<WaterSourceContext>
    {
        public override int GetBaseRank(IAgent agent) => 3;

        public override void Execute(IAgent agent, WaterSourceContext context)
        {
            var animal = agent.GetData<Animal>(AnimalBlackboardKeys.Animal);
            
            var water = context.Source;
            var stats = agent.GetData<AnimalStats>(AnimalBlackboardKeys.Stats);
            var state = agent.GetData<AnimalState>(AnimalBlackboardKeys.State);
            var drinkRate = stats.DrinkRate;
            var energyLossRate = stats.EnergyLossRate;
            var thirst = state.Thirst;
            var energy = state.Energy;
            
            water.Drink(drinkRate * Time.deltaTime, animal.gameObject);
            thirst.Change(-drinkRate * Time.deltaTime);
            energy.Change(-energyLossRate * Time.deltaTime);

            var hunger = state.Hunger;
            var hungerGainRate = stats.HungerGainRate;
            hunger.Change(hungerGainRate * Time.deltaTime);
        }

        public override bool IsValid(IAgent agent, WaterSourceContext context) => context.SqrDistance < 3.0f;

        public override bool IsFinished(IAgent agent, WaterSourceContext context) =>
            agent.GetData<AnimalState>(AnimalBlackboardKeys.State).Thirst.IsEmpty;
    }
}