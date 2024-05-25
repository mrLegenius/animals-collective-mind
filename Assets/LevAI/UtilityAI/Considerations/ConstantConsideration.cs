namespace LevAI.UtilityAI
{
    public sealed class ConstantConsideration : IConsideration
    {
        private readonly float _constant;
        public ConstantConsideration(float constant) => _constant = constant;
        public int CalculateRank(IAgent agent, IAction action, IContext context) => 0;
        public float Execute(IAgent agent, IContext context) => _constant;
    }
}