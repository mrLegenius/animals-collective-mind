using System;

namespace Lions.Animals
{
    [Serializable]
    public struct AnimalStats
    {
        public float HungerGainRate;
        public float ThirstGainRate;
        public float EnergyLossRate;
        public float EnergyLossRateOnWalk;
        public float EnergyLossRateOnRun;
        public float DrinkRate;
        public float EatRate;
        public float RestRate;
        public float Speed;
        public float RunSpeed;
        public float Danger;
    }
}