using LevAI.UtilityAI;
using Lions.Animals;
using UnityEngine;

namespace Lions.AI
{
    public class ThirstConsideration : ConsiderationBase
    {
        private const int MaxRank = 5;
        public ThirstConsideration(IFunction function) : base(function) {  }

        protected override int GetRank(IAgent agent, IAction action)
        {
            var stat = agent.GetData<AnimalState>(AnimalBlackboardKeys.State).Thirst;
            var value = stat.Value / stat.Max;
            return Mathf.RoundToInt(value * MaxRank);
        }

        protected override float GetInput(IAgent agent)
        {
            var stat = agent.GetData<AnimalState>(AnimalBlackboardKeys.State).Thirst;
            return stat.Value / stat.Max;
        }
    }
}