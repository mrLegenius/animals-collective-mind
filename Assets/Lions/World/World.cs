using System;
using System.Collections.Generic;
using System.Linq;
using Lions.Animals.Lion;
using Lions.Animals.Prey;
using Lions.Extensions;
using Lions.Food.Meat;
using Lions.Rest;
using UnityEngine;

namespace Lions
{
    public class World : MonoBehaviour
    {
        [SerializeField] private List<MeatSource> _meatSources;
        [SerializeField] private List<PrideRegion> _regions;
        [SerializeField] private List<Pride> _prides;
        [SerializeField] private List<RestPlace> _tallGrass;
        
        private readonly List<WaterSource> emptyWaterSources = new();

        public List<MeatSource> AllMeatSources => _meatSources;

        public IEnumerable<Lion> AllLions => _prides.SelectMany(x => x.Lions);

        public List<Prey> AllPreys { get; private set; }
        public IEnumerable<PrideRegion> AllRegions => _regions;
        public IEnumerable<RestPlace> TallGrassList => _tallGrass;

        private void Awake()
        {
            Time.timeScale = 4;
            AllPreys = FindObjectsOfType<Prey>().ToList();
        }

        private void OnValidate()
        {
            if (_tallGrass.Count == transform.childCount) return;
            
            _tallGrass = new List<RestPlace>();
            foreach (Transform child in transform)
            {
                if (child.TryGetComponent(out RestPlace grass))
                    _tallGrass.Add(grass);
            }
        }

        public List<WaterSource> GetClosestAvailableWaterSources(Vector3 position, GameObject user)
        {
            var closestWaterSource = _regions.SelectMany(x => x.WaterReservoirs)
                .Where(x => x.AnySourceAvailableFor(user))
                .FindMin(x => (x.Position - position).sqrMagnitude);

            return closestWaterSource == null ? emptyWaterSources : closestWaterSource.AllSources;
        }

        public void RegisterMeat(MeatSource meat) => _meatSources.Add(meat);

    }
}