using UnityEngine;

namespace LevAI.UtilityAI
{
    /// <summary>
    /// x * x * (3 - 2x)
    /// </summary>
    public class ClampedSmoothstepFunction : IFunction
    {
        public float Execute(float x)
        {
            return Mathf.Clamp01(x * x * (3 - 2 * x));
        }
    }
}