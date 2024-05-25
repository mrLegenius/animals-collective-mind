namespace LevAI.UtilityAI
{
    public readonly struct Option
    {
        public IContext Context { get; }
        public IAction Action { get; }

        public Option(IAction action, IContext context)
        {
            Action = action;
            Context = context;
        }
    }
}