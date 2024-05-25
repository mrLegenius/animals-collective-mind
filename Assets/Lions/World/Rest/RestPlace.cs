using Lions.Animals.Lion;
using UnityEngine;

namespace Lions.Rest
{
    public class RestPlace : MonoBehaviour
    {
        public Transform Transform => transform;
        public bool CanBeUsedBy(Lion lion) => _lion == null || _lion == lion;
        
        private const float UseTime = 1f;
        private float _useTimer;

        private Lion _lion;

        public void Use(Lion lion)
        {
            _lion = lion;
            _useTimer = UseTime;
        }
        
        private void Update()
        {
            if (_useTimer > 0)
                _useTimer -= Time.deltaTime;
            else
                _lion = null;
        }
    }
}