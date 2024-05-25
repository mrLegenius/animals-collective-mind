using LevAI.UtilityAI;

namespace Lions.AI
{
    public class MeatAbsenceConsideration : IConsideration
    {
        public int CalculateRank(IAgent agent, IAction action, IContext context) => 0;

        public float Execute(IAgent agent, IContext context) =>
            agent.GetData<bool>(LionBlackboardKeys.HasAnyMeatSource) ? 0.0f : 1.0f;
    }
}