using System.Linq;
using LevAI.UtilityAI;
using Lions.Animals;
using Lions.Animals.Lion;
using Lions.Animals.Prey;
using UnityEngine;

namespace Lions.AI
{
    public class WaitForAmbushTriggeredAction : ActionBase
    {
        public override int GetBaseRank(IAgent agent) => 10;
        public override string Group => Pride.HuntingGroup;

        public override void Execute(IAgent agent)
        {
            var prey = agent.GetData<Prey>(LionBlackboardKeys.PreyTarget);
            var lion = agent.GetData<Lion>(AnimalBlackboardKeys.Animal);

            lion.transform.rotation = Quaternion.LookRotation(prey.transform.position - lion.transform.position);
        }

        public override bool IsFinished(IAgent agent)
        {
            var group = agent.GetData<Lion>(AnimalBlackboardKeys.Animal).CurrentGroup;
            var huntingGroup = (HuntGroup)group;
            return huntingGroup?.IsAmbushTriggered ?? true;
        }
    }
    
    public class WaitForAmbushAction : ActionBase
    {
        public override int GetBaseRank(IAgent agent) => 10;
        public override string Group => Pride.HuntingGroup;

        public override void Execute(IAgent agent)
        {
            
        }

        public override bool IsFinished(IAgent agent)
        {
            var group = agent.GetData<Lion>(AnimalBlackboardKeys.Animal).CurrentGroup;
            var huntingGroup = (HuntGroup)group;
            return huntingGroup?.IsAmbushReady ?? true;
        }
    }
}