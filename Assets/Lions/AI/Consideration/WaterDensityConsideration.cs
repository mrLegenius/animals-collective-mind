using LevAI.UtilityAI;
using Lions.AI.Contexts;

namespace Lions.AI
{
    public class WaterDensityConsideration : ConsiderationBase<RegionContext>
    {
        public WaterDensityConsideration(IFunction function) : base(function)  {  }

        protected override float GetInput(IAgent agent, RegionContext context) => context.Source.CurrentWaterDensity < 0.2 ? 0.0f : context.Source.CurrentWaterDensity;
    }
}