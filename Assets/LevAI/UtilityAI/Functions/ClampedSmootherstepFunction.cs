using UnityEngine;

namespace LevAI.UtilityAI
{
    /// <summary>
    /// x * x * x * (x * (6x - 15) + 10)
    /// </summary>
    public class ClampedSmootherstepFunction : IFunction
    {
        public float Execute(float x)
        {
            return Mathf.Clamp01(x * x * x * (x * (6 * x - 15) + 10));
        }
    }
}