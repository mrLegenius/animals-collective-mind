using UnityEngine;

namespace LevAI.UtilityAI
{
    /// <summary>
    /// 1 / (1 + e^ (-k * (4e * (x - x0) - 2e))) 
    /// </summary>
    public class ClampedLogisticFunction : IFunction
    {
        private readonly float _a;
        private readonly float _b;

        public ClampedLogisticFunction(float a, float b)
        {
            _a = a;
            _b = b;
        }

        public float Execute(float x)
        {
            return Mathf.Clamp01(1 / (1 + Mathf.Exp(-_a * (4 * Mathf.Epsilon * (x - _b) - 2 * Mathf.Epsilon))));
        }
    }
}