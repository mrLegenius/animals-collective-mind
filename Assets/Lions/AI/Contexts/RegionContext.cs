using System.Collections.Generic;
using System.Linq;
using LevAI.UtilityAI;
using Lions.Animals;
using Lions.Animals.Lion;
using UnityEngine;

namespace Lions.AI.Contexts
{
    public class RegionContext : IDistanceContext, IGoToContext
    {
        public string Name => Source.name;
        public Transform Target => Source.gameObject.transform;
        public PrideRegion Source;
        public float SqrDistance { get; set; }
        public bool Equals(IContext other) => 
            other is RegionContext context && context.Source == Source;

        public bool IsValid() => Source;
    }
    
    public class RegionContextProducer : IContextProducer
    {
        private readonly List<IContext> _contextItems = new();

        public IEnumerable<IContext> Context => _contextItems;

        public void Produce(IAgent agent)
        {
            _contextItems.Clear();
            
            var targetRegion = agent.GetData<PrideRegion>(LionBlackboardKeys.MigrationRegion);

            if (targetRegion != null)
            {
                _contextItems.Add(new RegionContext 
                { 
                    Source = targetRegion, 
                    SqrDistance = agent.GetSqrDistance(targetRegion.transform), 
                });
                return;
            }
            
            var animal = agent.GetData<Lion>(AnimalBlackboardKeys.Animal);
            var sources = agent.GetData<IEnumerable<PrideRegion>>(LionBlackboardKeys.AllRegions);
            var contextItems = sources
                .Where(x => x != null)
                .Where(x => x != animal.CurrentRegion)
                .Select(source => new RegionContext
                {
                    Source = source,
                    SqrDistance = agent.GetSqrDistance(source.transform),
                });


            _contextItems.AddRange(contextItems);
        }
    }
}