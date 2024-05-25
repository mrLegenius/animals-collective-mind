using LevAI.UtilityAI;
using Lions.AI.Contexts;
using Lions.Animals;
using Lions.Animals.Lion;

namespace Lions.AI
{
    public class JoinMigrationGroupAction : ActionBase<RegionContext>
    {
        public override int GetBaseRank(IAgent agent) => agent.GetData<Lion>(AnimalBlackboardKeys.Animal).Pride.IsGroupExists(Pride.MigrationGroup) ? 100 : 0;

        public override string Group => Pride.MigrationGroup;

        public override void Execute(IAgent agent, RegionContext context)
        {
            agent.SetData(LionBlackboardKeys.MigrationRegion, context.Source, 100);
            agent.GetData<Lion>(AnimalBlackboardKeys.Animal).JoinMigration();
        }
    }
}