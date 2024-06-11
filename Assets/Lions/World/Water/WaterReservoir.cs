using System;
using System.Collections.Generic;
using System.Linq;
using Lions.Extensions;
using UnityEngine;

namespace Lions
{
    public class WaterReservoir : MonoBehaviour
    {
        private readonly List<WaterSource> _waterSources = new();
        [SerializeField] private float _heightChange;

        private Vector3 _initialPosition;
        private void Awake()
        {
            _initialPosition = transform.position;
            _waterSources.Clear();
            GetComponentsInChildren(_waterSources);
        }

        public void Update()
        {
            transform.position =
                _initialPosition + new Vector3(0, Mathf.Lerp(-_heightChange, 0, TotalCapacity / TotalMaxCapacity), 0);
        }

        public float TotalCapacity => _waterSources.Sum(x => x.Capacity);
        public float TotalMaxCapacity => _waterSources.Sum(x => x.MaxCapacity);
        public bool AnySourceAvailableFor(GameObject user) => _waterSources.Any(x => x.IsAvailableToDrinkFor(user));
        public Vector3 Position => transform.position;
        public List<WaterSource> AllSources => _waterSources;

        public WaterSource GetClosestAvailableSource(Vector3 position, GameObject user)
        {
            var closestWaterSource = _waterSources
                .Where(x => x.IsAvailableToDrinkFor(user))
                .FindMin(x => (x.Position - position).sqrMagnitude);

            return closestWaterSource;
        }
    }
}