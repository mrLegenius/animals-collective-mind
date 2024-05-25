using System.Collections.Generic;

namespace LevAI.UtilityAI
{
    public interface IUtilityCombiner
    {
        public float Combine(IEnumerable<float> utilities);
    }
}