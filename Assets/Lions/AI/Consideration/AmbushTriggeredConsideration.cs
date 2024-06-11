using LevAI.UtilityAI;
using Lions.Animals;
using Lions.Animals.Lion;

namespace Lions.AI
{
    public class AmbushTriggeredConsideration : LogicConsideration
    {
        protected override bool Predicate(IAgent agent, IContext context)
        {
            var lion = agent.GetData<Lion>(AnimalBlackboardKeys.Animal);

            if (lion.CurrentGroup is HuntGroup huntGroup)
                return huntGroup.IsAmbushTriggered;

            return false;
        }
    }
}