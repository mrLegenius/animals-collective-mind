using System.Collections.Generic;

namespace LevAI.UtilityAI
{
    public interface IContextProducer
    {
        IEnumerable<IContext> Context { get; }
        void Produce(IAgent agent);
    }
}