using System.Collections.Generic;
using System.Linq;
using LevAI.UtilityAI;
using Lions.AI;
using Lions.AI.Contexts;
using UnityEngine;

namespace Lions.Animals.Prey
{
    public class LionContext : IDistanceContext
    {
        public string Name => Source.name;
        public Transform Target => Source.gameObject.transform;
        public Lion.Lion Source;
        public float SqrDistance { get; set; }
        public bool Equals(IContext other) => 
            other is LionContext context && context.Source == Source;

        public bool IsValid() => Source;
    }
    
    public class LionContextProducer : IContextProducer
    {
        private readonly List<IContext> _contextItems = new();

        public IEnumerable<IContext> Context => _contextItems;

        public void Produce(IAgent agent)
        {
            var sources = agent.GetData<IEnumerable<Lion.Lion>>(PreyBlackboardKeys.Lions);
            var contextItems = sources
                .Where(x => x != null)
                .Select(source => new LionContext
                {
                    Source = source,
                    SqrDistance = agent.GetSqrDistance(source.transform),
                });

            _contextItems.Clear();
            _contextItems.AddRange(contextItems);
        }
    }
}