using System.Collections.Generic;
using UnityEngine;

namespace LevAI.Perception
{
    public interface IObservable { } 
    
    public class SphereCast : MonoBehaviour
    {
        private readonly RaycastHit[] _hits;
        private readonly IList<IObservable> _observables;

        public SphereCast(int maxObservations)
        {
            _hits = new RaycastHit[maxObservations];
            _observables = new IObservable[maxObservations];
        }

        public void Cast(Vector3 origin, float radius, LayerMask layerMask)
        {
            int hits = Physics.SphereCastNonAlloc(origin, radius, Vector3.zero, _hits, radius, layerMask.value);

            for (var hitIndex = 0; hitIndex < hits; hitIndex++)
            {
                RaycastHit hit = _hits[hitIndex];
                _observables[hitIndex] = hit.collider.GetComponent<IObservable>();
            }

            for (int emptyHit = hits; emptyHit < _observables.Count; emptyHit++) 
                _observables[emptyHit] = null;
        }
    }
}
