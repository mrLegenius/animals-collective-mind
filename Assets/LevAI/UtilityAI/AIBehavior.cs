using System.Collections.Generic;

namespace LevAI.UtilityAI
{
    public struct AIBehavior
    {
        public IAction Action;
        public List<IConsideration> Considerations;
        public IUtilityCombiner Combiner;
        public IContextProducer ContextProducer;
    }
}