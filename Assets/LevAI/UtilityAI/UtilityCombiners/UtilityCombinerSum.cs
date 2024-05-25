using System.Collections.Generic;
using System.Linq;

namespace LevAI.UtilityAI
{
    public class UtilityCombinerSum : IUtilityCombiner
    {
        public float Combine(IEnumerable<float> utilities) => utilities.Sum();
    }
}