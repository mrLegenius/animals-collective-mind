using LevAI.UtilityAI;
using Lions.AI;
using Lions.AI.Contexts;

namespace Lions.Animals.Lion
{
    public class AttackAction : ActionBase<PreyContext>
    {
        public override string Group => Pride.HuntingGroup;
        public override int GetBaseRank(IAgent agent) => 100;

        public override void Execute(IAgent agent, PreyContext context) => 
            context.Source.ConvertToMeat();

        public override bool IsValid(IAgent agent, PreyContext context) =>
            !agent.GetData<bool>(LionBlackboardKeys.HasAnyMeatSource)
            && context.SqrDistance <= 10.0f;
    }
}