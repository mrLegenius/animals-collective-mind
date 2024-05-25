using System;

namespace Lions
{
    [Serializable]
    public class Energy
    {
        public float Value;
        public float Max;

        public float NormalizedValue => Value / Max;
        public bool IsFull => Value >= Max;
        public void Change(float delta)
        {
            Value = Math.Clamp(Value + delta, 0, Max);
        }
    }
}