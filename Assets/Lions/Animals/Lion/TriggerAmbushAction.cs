using LevAI.UtilityAI;
using Lions.AI;

namespace Lions.Animals.Lion
{
    public class TriggerAmbushAction : ActionBase
    {
        public override int GetBaseRank(IAgent agent) => 15;
        public override string Group => Pride.HuntingGroup;

        public override void Execute(IAgent agent)
        {
            var group = (HuntGroup)agent.GetData<Lion>(AnimalBlackboardKeys.Animal).CurrentGroup;

            group.TriggerAmbush();
        }

        public override bool IsValid(IAgent agent)
        {
            var group = agent.GetData<Lion>(AnimalBlackboardKeys.Animal).CurrentGroup;

            if (group is HuntGroup { IsAmbushTriggered: true }) return false;
            
            var target = agent.GetData<Prey.Prey>(LionBlackboardKeys.PreyTarget);
            return target && agent.GetSqrDistance(target.transform) <= 100f;
        }
    }
}