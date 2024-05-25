using UnityEngine;

namespace LevAI.UtilityAI
{
    /// <summary>
    /// e ^ (a * x)
    /// </summary>
    public class ClampedExponentialFunction : IFunction
    {
        private readonly float _a;

        public ClampedExponentialFunction(float a) => _a = a;

        public float Execute(float x) => Mathf.Clamp01(Mathf.Exp(_a * x)) - 1;
    }
    
    /// <summary>
    /// e ^ (a * -x)
    /// </summary>
    public class ClampedExponentialInvertedFunction : IFunction
    {
        private readonly float _a;
        
        public ClampedExponentialInvertedFunction(float a) => _a = a;
        public float Execute(float x) => Mathf.Clamp01(Mathf.Exp(_a * -x));
    }
}