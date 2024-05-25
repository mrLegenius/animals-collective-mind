using UnityEngine;

namespace LevAI.UtilityAI
{
    /// <summary>
    /// 1 - x / xMax
    /// </summary>
    public class ClampedLinearInvertedFunction : IFunction
    {
        private readonly float _max;

        public ClampedLinearInvertedFunction(float max)
        {
            _max = max;
        }

        public float Execute(float x) => Mathf.Clamp01(1.0f - (Mathf.Approximately(_max, 0) ? 0 : x / _max));
    }
    
    /// <summary>
    /// x / xMax
    /// </summary>
    public class ClampedLinearFunction : IFunction
    {
        private readonly float _max;

        public ClampedLinearFunction(float max)
        {
            _max = max;
        }

        public float Execute(float x) => Mathf.Clamp01(Mathf.Approximately(_max, 0) ? 0 : x / _max);
    }
}