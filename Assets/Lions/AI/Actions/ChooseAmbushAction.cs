using LevAI.UtilityAI;
using Lions.AI.Contexts;

namespace Lions.AI
{
    public class ChooseAmbushAction : ActionBase<RestPlaceContext>
    {
        public override void Execute(IAgent agent, RestPlaceContext context)
        {
            agent.SetData(LionBlackboardKeys.Ambush, context.Target, 100);
        }
    }
}