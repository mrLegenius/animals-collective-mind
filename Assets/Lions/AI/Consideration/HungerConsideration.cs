using LevAI.UtilityAI;
using Lions.Animals;
using UnityEngine;

namespace Lions.AI
{
    public class HungerConsideration : ConsiderationBase
    {
        private const int MaxRank = 3;
        
        public HungerConsideration(IFunction function) : base(function) {  }

        protected override int GetRank(IAgent agent, IAction action)
        {
            var stat = agent.GetData<AnimalState>(AnimalBlackboardKeys.State).Hunger;
            var value = stat.Value / stat.Max;
            return Mathf.RoundToInt(value * MaxRank);
        }
        
        protected override float GetInput(IAgent agent)
        {
            var stat = agent.GetData<AnimalState>(AnimalBlackboardKeys.State).Hunger;
            return stat.Value / stat.Max;
        }
    }
}