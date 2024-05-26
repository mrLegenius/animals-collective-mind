using UnityEngine;

namespace LevAI.UtilityAI
{
    /// <summary>
    /// ( log a (x / (1 - x)) + 2e) / 4e
    /// </summary>
    public class ClampedLogitFunction : IFunction
    {
        private readonly float _a;

        public ClampedLogitFunction(float a) => _a = a;

        public float Execute(float x)
        {
            return Mathf.Clamp01((Mathf.Log(x / (1 - x), _a) + 2 * Mathf.Epsilon) / 4 * Mathf.Epsilon);
        }
    }
}