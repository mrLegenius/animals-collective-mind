using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LevAI.Perception
{
    public partial class DangerSensor : MonoBehaviour, ISensor
    {
        [SerializeField] private float _radius;
        [SerializeField] private LayerMask _layers;
        [SerializeField] private float _maxFear;
        [SerializeField] private float _fearDropRate;
        
        public float Fear { get; private set; }
                
        private readonly Collider[] _colliders = new Collider[50];
        
        private int _collidersCount;
        private float _scanInterval;
        private float _scanTimer;
        
        public bool Tick()
        {
            _collidersCount = Physics.OverlapSphereNonAlloc(transform.position, _radius, _colliders, _layers);

            bool sensedAnything = false;
            
            for (int i = 0; i < _collidersCount; i++)
            {
                if (!_colliders[i].TryGetComponent<IDangerSource>(out var dangerSource)) continue;
                
                Fear += dangerSource.Danger * Time.deltaTime;
                Fear = Mathf.Min(Fear, _maxFear);
                sensedAnything = true;
            }

            if (!sensedAnything)
            {
                Fear -= _fearDropRate * Time.deltaTime;
                Fear = Mathf.Max(Fear, 0);
            }
            
            return false;
        }

        public IEnumerable<GameObject> ObservableGameObjects => Enumerable.Empty<GameObject>();
        public int Priority => 0;
    }
}