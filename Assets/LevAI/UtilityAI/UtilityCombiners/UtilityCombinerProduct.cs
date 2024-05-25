using System.Collections.Generic;
using System.Linq;

namespace LevAI.UtilityAI
{
    public class UtilityCombinerProduct : IUtilityCombiner
    {
        public float Combine(IEnumerable<float> utilities) =>
            utilities.Aggregate<float, float>(1, (current, utility) => current * utility);
    }
}