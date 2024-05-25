using System.Collections.Generic;
using System.Linq;
using Lions.Rest;
using UnityEngine;

namespace Lions
{
    public class PrideRegion : MonoBehaviour
    {
        [SerializeField] private PrideRestPlaces _restPlace;
        [SerializeField] private List<WaterReservoir> _waterReservoirs;

        public PrideRestPlaces RestPlace => _restPlace;
        public List<WaterReservoir> WaterReservoirs => _waterReservoirs;

        public bool HasAnyAvailableWaterFor(GameObject user) => !WaterReservoirs.TrueForAll(x => !x.AnySourceAvailableFor(user));
        public Vector3 Position => _restPlace.Position;

        public float CurrentWaterDensity =>
            _waterReservoirs.SelectMany(x => x.AllSources).Sum(x => x.Capacity)
            / _waterReservoirs.SelectMany(x => x.AllSources).Sum(x => x.MaxCapacity);
    }
}