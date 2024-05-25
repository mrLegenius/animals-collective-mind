namespace LevAI.UtilityAI
{
    public abstract class ConsiderationBase<TContext> : IConsideration where TContext : IContext 
    {
        private readonly IFunction _function;
        protected ConsiderationBase(IFunction function) => _function = function;

        protected virtual int GetRank(IAgent agent, IAction action, TContext context) => 0;
        protected virtual bool CanExecute(IAgent agent, TContext context) => true;
        protected abstract float GetInput(IAgent agent, TContext context);

        private float CalculateScore(float input) => _function.Execute(input);
        
        int IConsideration.CalculateRank(IAgent agent, IAction action, IContext context) => 
            GetRank(agent, action, (TContext)context);
        float IConsideration.Execute(IAgent agent, IContext context)
        {
            if (!CanExecute(agent, (TContext)context)) return 0;

            float input = GetInput(agent, (TContext)context);
            return CalculateScore(input);
        }
    }
    
    public abstract class ConsiderationBase : IConsideration
    {
        private readonly IFunction _function;

        protected ConsiderationBase(IFunction function) => _function = function;

        protected virtual int GetRank(IAgent agent, IAction action) => 0;
        protected virtual bool CanExecute(IAgent agent) => true;
        protected abstract float GetInput(IAgent agent);
        
        private float CalculateScore(float input) => _function.Execute(input);

        int IConsideration.CalculateRank(IAgent agent, IAction action, IContext context) => GetRank(agent, action);
        float IConsideration.Execute(IAgent agent, IContext context)
        {
            if (!CanExecute(agent)) return 0;

            float input = GetInput(agent);
            return CalculateScore(input);
        }
    }
}