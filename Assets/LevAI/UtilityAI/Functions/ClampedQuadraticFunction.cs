using UnityEngine;

namespace LevAI.UtilityAI
{
    /// <summary>
    /// 1 - (x / xMax) ^ power
    /// </summary>
    public class ClampedQuadraticInvertedFunction : IFunction
    {
        private readonly float _max;
        private readonly float _power;

        public ClampedQuadraticInvertedFunction(float max, float power)
        {
            _max = max;
            _power = power;
        }

        public float Execute(float x)
        {
            if (Mathf.Approximately(_max, 0))
                return 0.0f;
            
            return Mathf.Clamp01(1.0f - Mathf.Pow(x / _max, _power));
        }
    }
    
    /// <summary>
    /// (x / xMax) ^ power
    /// </summary>
    public class ClampedQuadraticFunction : IFunction
    {
        private readonly float _max;
        private readonly float _power;

        public ClampedQuadraticFunction(float max, float power)
        {
            _max = max;
            _power = power;
        }

        public float Execute(float x)
        {
            if (Mathf.Approximately(_max, 0))
                return 1.0f;
            
            return Mathf.Clamp01(Mathf.Pow(x / _max, _power));
        }
    }
}