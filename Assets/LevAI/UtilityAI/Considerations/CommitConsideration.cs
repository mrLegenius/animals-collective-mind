namespace LevAI.UtilityAI
{
    public class CommitConsideration<TContext> : ConsiderationBase<TContext> where TContext : IContext
    {
        private const int DefaultRank = 0;
        private readonly int _commitingRank;
        
        public CommitConsideration(int commitingRank) : base(new ConstantFunction(1)) => 
            _commitingRank = commitingRank;

        protected override int GetRank(IAgent agent, IAction action, TContext context) => 
            action.Equals(agent.CurrentAction) && context.Equals(agent.CurrentActionContext) ? _commitingRank : DefaultRank;

        protected override float GetInput(IAgent agent, TContext context) => 1;
    }
    
    public class CommitConsideration : ConsiderationBase
    {
        private const int DefaultRank = 0;
        private readonly int _commitingRank;
        
        public CommitConsideration(int commitingRank) : base(new ConstantFunction(1)) => 
            _commitingRank = commitingRank;

        protected override int GetRank(IAgent agent, IAction action) => 
            action.Equals(agent.CurrentAction) ? _commitingRank : DefaultRank;

        protected override float GetInput(IAgent agent) => 1;
    }
}