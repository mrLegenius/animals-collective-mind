using System.Collections.Generic;
using System.Linq;
using LevAI.UtilityAI;
using Lions.Animals;
using UnityEngine;

namespace Lions.AI.Contexts
{
    public class WaterSourceContext : IDistanceContext, IGoToContext
    {
        public string Name => Source.name;
        public Transform Target => Source.Transform;
        public WaterSource Source;
        public float SqrDistance { get; set; }
        public bool Equals(IContext other) => 
            other is WaterSourceContext context && context.Source == Source;
        
        public bool IsValid() => Source;
    }

    public class WaterSourceContextProducer : IContextProducer
    {
        private readonly List<IContext> _contextItems = new();

        public IEnumerable<IContext> Context => _contextItems;

        public void Produce(IAgent agent)
        {
            var animal = agent.GetData<Animal>(AnimalBlackboardKeys.Animal);
            var sources = agent.GetData<List<WaterSource>>(AnimalBlackboardKeys.AllWaterSources);
            var contextItems = sources
                .Where(x => x.IsAvailableToDrinkFor(animal.gameObject))
                .Where(x => x != null)
                .Select(source => new WaterSourceContext
                {
                    Source = source,
                    SqrDistance = agent.GetSqrDistance(source.transform),
                });

            _contextItems.Clear();
            _contextItems.AddRange(contextItems);
        }
    }
}