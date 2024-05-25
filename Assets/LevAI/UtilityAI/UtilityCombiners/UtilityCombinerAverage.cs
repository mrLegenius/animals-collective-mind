using System.Collections.Generic;

namespace LevAI.UtilityAI
{
    public class UtilityCombinerAverage : IUtilityCombiner
    {
        public float Combine(IEnumerable<float> utilities)
        {
            float sum = 0;
            float count = 0;

            foreach (var utility in utilities)
            {
                sum += utility;
                count++;
            }

            return sum / count;
        }
    }
}