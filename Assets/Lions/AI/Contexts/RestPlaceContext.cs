using System.Collections.Generic;
using System.Linq;
using LevAI.UtilityAI;
using Lions.Animals;
using Lions.Animals.Lion;
using Lions.Rest;
using UnityEngine;

namespace Lions.AI.Contexts
{
    public class RestPlaceContext : IDistanceContext, IGoToContext
    {
        public string Name => Source.name;
        public Transform Target => Source.Transform;
        public RestPlace Source;
        public float SqrDistance { get; set; }
        public bool Equals(IContext other) => 
            other is RestPlaceContext context && context.Source == Source;
        
        public bool IsValid() => Source;
    }
    
    public class RestPlaceContextProducer : IContextProducer
    {
        private readonly List<IContext> _contextItems = new();

        public IEnumerable<IContext> Context => _contextItems;

        public void Produce(IAgent agent)
        {
            var animal = agent.GetData<Lion>(AnimalBlackboardKeys.Animal);
            var sources = agent.GetData<List<RestPlace>>(LionBlackboardKeys.AllRestPlaces);
            var contextItems = sources
                .Where(x => x != null)
                .Where(x => x.CanBeUsedBy(animal))
                .Select(source => new RestPlaceContext
                {
                    Source = source,
                    SqrDistance = agent.GetSqrDistance(source.transform),
                });

            _contextItems.Clear();
            _contextItems.AddRange(contextItems);
        }
    }
}