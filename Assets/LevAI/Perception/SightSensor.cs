using System.Collections.Generic;
using UnityEngine;

namespace LevAI.Perception
{
    public partial class SightSensor : MonoBehaviour, ISensor
    {
        [SerializeField] private float _halfAngle = 45f;
        [SerializeField] private float _distance = 10;
        [SerializeField] private int _scanFrequency = 30;
        [SerializeField] private LayerMask _layers;
        [SerializeField] private LayerMask _obstacleLayers;
        [SerializeField] private int _priority;

        private readonly Collider[] _colliders = new Collider[50];
        private readonly List<GameObject> _gameObjects = new();

        private int _collidersCount;
        private float _scanInterval;
        private float _scanTimer;
        public IEnumerable<GameObject> ObservableGameObjects => _gameObjects;
        public int Priority => _priority;

        private void Start()
        {
            _scanInterval = 1.0f / _scanFrequency;
        }
        
        bool ISensor.Tick()
        {
            _scanTimer -= Time.deltaTime;

            if (_scanTimer > 0.0f) return false;
            
            _scanTimer += _scanInterval;
            Scan();
            return true;
        }

        private void Scan()
        {
            _collidersCount = Physics.OverlapSphereNonAlloc(transform.position, _distance, _colliders, _layers);

            _gameObjects.Clear();

            for (int i = 0; i < _collidersCount; i++)
            {
                var go = _colliders[i].gameObject;
                if (IsInSight(go))
                    _gameObjects.Add(go);
            }
        }

        private bool IsInSight(GameObject go)
        {
            Vector3 origin = transform.position;
            Vector3 destination = go.transform.position;

            Vector3 direction = destination - origin;

            float angleBetween = Vector3.Angle(direction, transform.forward);

            if (angleBetween > _halfAngle)
                return false;

            origin.y += 0.5f;
            destination.y = origin.y;

            if (Physics.Linecast(origin, destination, _obstacleLayers))
                return false;

            return true;
        }
    }
}