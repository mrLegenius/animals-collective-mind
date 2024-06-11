namespace LevAI.UtilityAI
{
public interface IAction
{
    string Group { get; }
    int GetBaseRank(IAgent agent);
    void Execute(IAgent agent, IContext context);
    bool IsValid(IAgent agent, IContext context);
    bool IsFinished(IAgent agent, IContext context);
    void OnFinished(IAgent agent);
    void OnInterrupted(IAgent agent);
}
}
