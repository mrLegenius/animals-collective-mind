
namespace LevAI.UtilityAI
{
    public abstract class LogicConsideration : IConsideration
    {
        protected abstract bool Predicate(IAgent agent, IContext context);

        public virtual int CalculateRank(IAgent agent, IAction action, IContext context) => 0;

        public float Execute(IAgent agent, IContext context) => Predicate(agent, context) ? 1.0f : 0.0f;
    }
}