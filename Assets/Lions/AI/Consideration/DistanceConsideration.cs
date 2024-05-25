using LevAI.UtilityAI;

namespace Lions.AI
{
    public interface IDistanceContext : IContext
    {
        float SqrDistance { get; }
    }
    
    public class DistanceConsideration : ConsiderationBase<IDistanceContext>
    {
        public DistanceConsideration(IFunction function) : base(function) { }
        
        protected override float GetInput(IAgent agent, IDistanceContext context) => context.SqrDistance;
    }
}