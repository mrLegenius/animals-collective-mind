using LevAI.UtilityAI;
using UnityEngine;

namespace Lions.Animals.Prey
{
    public class RunAwayAction : ActionBase<LionContext>
    {
        public override int GetBaseRank(IAgent agent) => 6;

        public override void Execute(IAgent agent, LionContext context)
        {
            var me = agent.GetData<Prey>(AnimalBlackboardKeys.Animal);
            var closestLion = context.Source;
            
            var stats = agent.GetData<AnimalStats>(AnimalBlackboardKeys.Stats);
            var state = agent.GetData<AnimalState>(AnimalBlackboardKeys.State);
            
            var thirstGainRate = stats.ThirstGainRate;
            var thirst = state.Thirst;
            thirst.Change(thirstGainRate * Time.deltaTime);
            
            var energy = state.Energy;
            var energyLossRate = stats.EnergyLossRateOnRun;
            energy.Change(-energyLossRate * Time.deltaTime);
            
            var hunger = state.Hunger;
            var hungerGainRate = stats.HungerGainRate;
            hunger.Change(hungerGainRate * Time.deltaTime);
            
            var animal = agent.GetData<Animal>(AnimalBlackboardKeys.Animal);

            var direction = me.transform.position - closestLion.transform.position.normalized;
            animal.MoveTo(me.transform.position + direction, stats.RunSpeed);
        }
    }
}