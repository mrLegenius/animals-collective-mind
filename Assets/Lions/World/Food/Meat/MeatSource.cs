using UnityEngine;

namespace Lions.Food.Meat
{
    public class MeatSource : MonoBehaviour
    {
        public float MeatCapacity;
        public Transform Transform => transform;

        public void Eat(float eatRate)
        {
            MeatCapacity -= eatRate;
            
            if (MeatCapacity <= 0)
                Destroy(gameObject);
        }
    }
}