using LevAI.UtilityAI;
using Lions.Animals;
using UnityEngine;

namespace Lions.AI
{
    public class EnergyConsideration : ConsiderationBase
    {
        private const int MaxRank = 5;
        
        public EnergyConsideration(IFunction function) : base(function) {  }

        protected override int GetRank(IAgent agent, IAction action)
        {
            var stat = agent.GetData<AnimalState>(AnimalBlackboardKeys.State).Energy;
            var value = stat.Value / stat.Max;
            return Mathf.RoundToInt( (1 - value) * MaxRank);
        }
        
        protected override float GetInput(IAgent agent)
        {
            var stat = agent.GetData<AnimalState>(AnimalBlackboardKeys.State).Energy;
            return stat.Value / stat.Max;
        }
    }
}