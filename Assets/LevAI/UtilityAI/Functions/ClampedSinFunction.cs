using UnityEngine;

namespace LevAI.UtilityAI
{
    /// <summary>
    /// sin(x * pi * a) + b
    /// </summary>
    public class ClampedSinFunction : IFunction
    {
        private readonly float _a;
        private readonly float _b;

        public ClampedSinFunction(float a, float b)
        {
            _a = a;
            _b = b;
        }

        public float Execute(float x) => Mathf.Clamp01(Mathf.Sin(x * Mathf.PI * _a) + _b);
    }
}