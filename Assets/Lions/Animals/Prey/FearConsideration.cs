using LevAI.UtilityAI;

namespace Lions.Animals.Prey
{
    public class FearConsideration : ConsiderationBase
    {
        public FearConsideration(IFunction function) : base(function)
        {
        }

        protected override float GetInput(IAgent agent) => agent.GetData<float>(AnimalBlackboardKeys.Fear);
    }
}