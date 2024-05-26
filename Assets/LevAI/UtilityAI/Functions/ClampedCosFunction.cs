using UnityEngine;

namespace LevAI.UtilityAI
{
    /// <summary>
    /// sin(x * pi * a) + b
    /// </summary>
    public class ClampedCosFunction : IFunction
    {
        private readonly float _a;
        private readonly float _b;

        public ClampedCosFunction(float a, float b)
        {
            _a = a;
            _b = b;
        }

        public float Execute(float x) => Mathf.Clamp01(1.0f - Mathf.Cos(x * Mathf.PI * _a) + _b);
    }
}