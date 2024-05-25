
namespace LevAI.UtilityAI
{
    public abstract class ActionBase<TContext> : IAction where TContext : IContext
    {
        public virtual string Group => string.Empty;
        public virtual int GetBaseRank(IAgent agent) => 0;
        public abstract void Execute(IAgent agent, TContext context);
        public virtual bool IsValid(IAgent agent, TContext context) => true;
        public virtual bool IsFinished(IAgent agent, TContext context) => true;
        public virtual void OnFinished(Agent agent) { }
        
        void IAction.Execute(IAgent agent, IContext context) => Execute(agent, (TContext)context);
        bool IAction.IsFinished(IAgent agent, IContext context) => IsFinished(agent, (TContext)context);
        bool IAction.IsValid(IAgent agent, IContext context) => IsValid(agent, (TContext)context);
    }
    
    public abstract class ActionBase : IAction
    {
        public virtual string Group => string.Empty;
        public virtual int GetBaseRank(IAgent agent) => 0;
        public abstract void Execute(IAgent agent);
        public virtual bool IsValid(IAgent agent) => true;
        public virtual bool IsFinished(IAgent agent) => true;
        public virtual void OnFinished(Agent agent) { }
        
        void IAction.Execute(IAgent agent, IContext context) => Execute(agent);
        bool IAction.IsFinished(IAgent agent, IContext context) => IsFinished(agent);
        bool IAction.IsValid(IAgent agent, IContext context) => IsValid(agent);
    }
}