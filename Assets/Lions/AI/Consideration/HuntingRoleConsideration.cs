using LevAI.UtilityAI;
using Lions.Animals;
using Lions.Animals.Lion;

namespace Lions.AI
{
    public class HuntingRoleConsideration : IConsideration
    {
        private readonly LionHuntingRole _huntingRole;

        public HuntingRoleConsideration(LionHuntingRole huntingRole) => _huntingRole = huntingRole;
        public int CalculateRank(IAgent agent, IAction action, IContext context) => 0;

        public float Execute(IAgent agent, IContext context) => 
            agent.GetData<Lion>(AnimalBlackboardKeys.Animal).GetHuntingRole() == _huntingRole ? 1.0f : 0.0f;
    }
}