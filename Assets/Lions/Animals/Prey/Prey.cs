using LevAI.UtilityAI;
using Lions.Food.Meat;
using UnityEngine;

namespace Lions.Animals.Prey
{
    public static class PreyBlackboardKeys
    {
        public const string Lions = "Lions";
    }
    
    public class Prey : Animal
    {
        [SerializeField] private float _meatCapacity;
        [SerializeField] private MeatSource _meatPrefab;

        protected override Agent CreateBrains() => new PreyAgent(this);

        public void ConvertToMeat()
        {
            MeatSource meat = Instantiate(_meatPrefab, transform.position, transform.rotation);
            meat.MeatCapacity = _meatCapacity;

            _world.RegisterMeat(meat);
            
            Destroy(gameObject);
        }

        protected override void CollectObservation()
        {
            base.CollectObservation();
            
            Agent.SetData(PreyBlackboardKeys.Lions, _world.AllLions, 5);
        }
    }
}
