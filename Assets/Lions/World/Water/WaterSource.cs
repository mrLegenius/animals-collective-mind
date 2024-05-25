using System;
using UnityEngine;

namespace Lions
{
    public class WaterSource : MonoBehaviour
    {
        [SerializeField] public float MaxCapacity;
        public float Capacity;
        [SerializeField] private float DrainRate;
        [SerializeField] private float RestoreRate;

        [SerializeField] private Material _dryMaterial;
        [SerializeField] private Material _normalMaterial;
        [SerializeField] private MeshRenderer _mesh;

        private bool _drying = true;
        private GameObject _user = null;
        private float _usedTimer;

        public Vector3 Position => transform.position;
        public Transform Transform => transform;
        public bool IsAvailableToDrinkFor(GameObject user) => (_user == null || _user == user) && IsAvailableToDrink;

        private bool IsAvailableToDrink => _drying || Capacity > MaxCapacity / 2;
        
        public void Drink(float amount, GameObject user)
        {
            Capacity -= amount;
            _user = user;
        }

        private void Update()
        {
            var changeRate = _drying ? -DrainRate : RestoreRate;

            Capacity += changeRate * Time.deltaTime;

            if (Capacity <= 0)
            {
                Capacity = 0;
                _drying = false;
                _mesh.material = _normalMaterial;
            }

            if (Capacity >= MaxCapacity / 2) 
                _drying = true;

            if (Capacity >= MaxCapacity)
            {
                Capacity = MaxCapacity;
                _drying = true;
                _mesh.material = _dryMaterial;
            }

            _mesh.sharedMaterial = IsAvailableToDrink ? _normalMaterial : _dryMaterial;

            if (_user != null)
            {
                _usedTimer += Time.deltaTime;

                if (_usedTimer > 0.5f)
                {
                    _user = null;
                    _usedTimer = 0.0f;
                }
            }
        }
    }
}