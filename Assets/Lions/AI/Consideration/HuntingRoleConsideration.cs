using System.Linq;
using LevAI.UtilityAI;
using Lions.Animals;
using Lions.Animals.Lion;

namespace Lions.AI
{
    public class HuntingRoleConsideration : LogicConsideration
    {
        private readonly LionHuntingRole _huntingRole;

        public HuntingRoleConsideration(LionHuntingRole huntingRole) => _huntingRole = huntingRole;

        protected override bool Predicate(IAgent agent, IContext context)
        {
            var group = agent.GetData<Lion>(AnimalBlackboardKeys.Animal).CurrentGroup as HuntGroup;

            if (group == null) return false;
            
            return group.IsAmbushTriggered || agent.GetData<Lion>(AnimalBlackboardKeys.Animal).GetHuntingRole() == _huntingRole;
        }
    }
    
    public class HasHuntingRoleInGroupConsideration : LogicConsideration
    {
        private readonly LionHuntingRole _huntingRole;

        public HasHuntingRoleInGroupConsideration(LionHuntingRole huntingRole) => _huntingRole = huntingRole;

        protected override bool Predicate(IAgent agent, IContext context)
        {
            var group = agent.GetData<Lion>(AnimalBlackboardKeys.Animal).CurrentGroup as HuntGroup;

            if (group == null) return false;
            
            return group.Lions.Any(x => x.GetHuntingRole() == _huntingRole);
        }
    }
}