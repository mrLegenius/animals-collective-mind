using LevAI.UtilityAI;
using Lions.AI.Contexts;
using Lions.Animals;
using Lions.Animals.Lion;

namespace Lions.AI
{
    public class ChooseAmbushAction : ActionBase<RestPlaceContext>
    {
        public override int GetBaseRank(IAgent agent) => 10;
        public override string Group => Pride.HuntingGroup;
        
        public override void Execute(IAgent agent, RestPlaceContext context)
        {
            agent.SetData(LionBlackboardKeys.Ambush, context.Target, 100);
            var lion =  agent.GetData<Lion>(AnimalBlackboardKeys.Animal);
            var group = lion.CurrentGroup;
            group.SendMessage(lion, LionBlackboardKeys.Ambush, context.Target, 100);
        }
    }
}