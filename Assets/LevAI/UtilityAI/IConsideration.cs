namespace LevAI.UtilityAI
{
public interface IConsideration
{
    int CalculateRank(IAgent agent, IAction action, IContext context);
    float Execute(IAgent agent, IContext context);
}
}
