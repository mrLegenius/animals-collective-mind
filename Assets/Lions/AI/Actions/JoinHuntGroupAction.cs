using LevAI.UtilityAI;
using Lions.AI.Contexts;
using Lions.Animals;
using Lions.Animals.Lion;

namespace Lions.AI
{
    public class JoinHuntGroupAction : ActionBase<PreyContext>
    {
        public override int GetBaseRank(IAgent agent) => 3;

        public override void Execute(IAgent agent, PreyContext context)
        {
            agent.SetData(LionBlackboardKeys.PreyTarget, context.Source, 100);
            agent.GetData<Lion>(AnimalBlackboardKeys.Animal).JoinHunt();
        }
    }
}