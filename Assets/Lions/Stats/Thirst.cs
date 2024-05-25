using System;

namespace Lions
{
    [Serializable]
    public class Thirst
    {
        public float Value;
        public float Max;
        
        public bool IsFull => Value >= Max;
        public bool IsEmpty => Value <= 0;
        public float NormalizedValue => Value / Max;
        
        public void Change(float delta)
        {
            Value = Math.Clamp(Value + delta, 0, Max);
        }
    }
}