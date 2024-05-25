using System.Linq;
using LevAI.UtilityAI;
using Lions.Animals;
using Lions.Animals.Lion;

namespace Lions.AI
{
    public class WaitForAmbushAction : ActionBase
    {
        public override void Execute(IAgent agent)
        {
            
        }

        public override bool IsFinished(IAgent agent)
        {
            var group = agent.GetData<Lion>(AnimalBlackboardKeys.Animal).CurrentGroup;
            var huntingGroup = (HuntGroup)group;
            return huntingGroup.IsAmbushReady;
        }

        public override bool IsValid(IAgent agent)
        {
            var animal = agent.GetData<Lion>(AnimalBlackboardKeys.Animal);
            if (animal.GetHuntingRole() != LionHuntingRole.Chase) return false;
            
            var group = animal.CurrentGroup;

            if (group == null) return false;
            if (group.Name != Pride.HuntingGroup) return false;
            
            var huntingGroup = (HuntGroup)group;

            return !huntingGroup.IsAmbushReady && group.Lions.Any(x => x.GetHuntingRole() == LionHuntingRole.Ambush);
        }
    }
}