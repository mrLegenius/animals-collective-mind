using System.Collections.Generic;
using System.Linq;
using LevAI.UtilityAI;
using Lions.Food.Meat;
using UnityEngine;

namespace Lions.AI.Contexts
{
    public class MeatSourceContext : IDistanceContext, IGoToContext
    {
        public string Name => Source.name;
        public Transform Target => Source.Transform;
        public MeatSource Source;
        public float SqrDistance { get; set; }
        public bool Equals(IContext other) => 
            other is MeatSourceContext context && context.Source == Source;
        
        public bool IsValid() => Source;
    }
    
    public class MeatSourceContextProducer : IContextProducer
    {
        private readonly List<IContext> _contextItems = new();

        public IEnumerable<IContext> Context => _contextItems;

        public void Produce(IAgent agent)
        {
            var sources = agent.GetData<List<MeatSource>>(LionBlackboardKeys.AllMeatSources);
            var contextItems = sources
                .Where(x => x != null)
                .Select(source => new MeatSourceContext
                {
                    Source = source,
                    SqrDistance = agent.GetSqrDistance(source.transform),
                });

            _contextItems.Clear();
            _contextItems.AddRange(contextItems);
        }
    }
}