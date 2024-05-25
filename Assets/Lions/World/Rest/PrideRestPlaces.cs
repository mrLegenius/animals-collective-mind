using System.Collections.Generic;
using UnityEngine;

namespace Lions.Rest
{
    public class PrideRestPlaces : MonoBehaviour
    {
        private readonly List<RestPlace> _restPlaces = new();
        private void Awake()
        {
            _restPlaces.Clear();
            GetComponentsInChildren(_restPlaces);
        }

        public List<RestPlace> RestPlaces => _restPlaces;
        
        public Vector3 Position => transform.position;
    }
}