using LevAI.UtilityAI;
using Lions.AI.Contexts;
using Lions.Animals;
using UnityEngine;

namespace Lions.AI
{
    public class EatAction : ActionBase<MeatSourceContext>
    {
        public override int GetBaseRank(IAgent agent) => 1;

        public override void Execute(IAgent agent, MeatSourceContext context)
        {
            var stats = agent.GetData<AnimalStats>(AnimalBlackboardKeys.Stats);
            var state = agent.GetData<AnimalState>(AnimalBlackboardKeys.State);
            
            var thirst = state.Thirst;
            var thirstGainRate = stats.ThirstGainRate;
            thirst.Change(thirstGainRate * Time.deltaTime);
            
            var energy = state.Energy;
            var energyLossRate = stats.EnergyLossRate;
            energy.Change(-energyLossRate * Time.deltaTime);
            
            var hunger = state.Hunger;
            var eatRate = stats.EatRate;
            hunger.Change(-eatRate * Time.deltaTime);

            var meat = context.Source;
            meat.Eat(eatRate * Time.deltaTime);
        }

        public override bool IsValid(IAgent agent, MeatSourceContext context) => 
            context.SqrDistance < 3.0f;
        
        public override bool IsFinished(IAgent agent, MeatSourceContext context) =>
            agent.GetData<AnimalState>(AnimalBlackboardKeys.State).Hunger.IsEmpty;
    }
}