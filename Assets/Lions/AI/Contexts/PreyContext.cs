using System.Collections.Generic;
using System.Linq;
using LevAI.UtilityAI;
using Lions.Animals.Prey;
using UnityEngine;

namespace Lions.AI.Contexts
{
    public class PreyContext : IDistanceContext, IGoToContext
    {
        public string Name => Source.name;
        public Transform Target => Source.gameObject.transform;
        public Prey Source;
        public float SqrDistance { get; set; }
        public bool Equals(IContext other) => 
            other is PreyContext context && context.Source == Source;

        public bool IsValid() => Source;
    }
    
    public class PreyContextProducer : IContextProducer
    {
        private readonly List<IContext> _contextItems = new();

        public IEnumerable<IContext> Context => _contextItems;

        public void Produce(IAgent agent)
        {
            var sources = agent.GetData<IEnumerable<Prey>>(LionBlackboardKeys.AllPreys);
            var contextItems = sources
                .Where(x => x != null)
                .Select(source => new PreyContext
                {
                    Source = source,
                    SqrDistance = agent.GetSqrDistance(source.transform),
                });

            _contextItems.Clear();
            _contextItems.AddRange(contextItems);
        }
    }
}