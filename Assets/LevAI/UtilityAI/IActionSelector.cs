using System.Collections.Generic;

namespace LevAI.UtilityAI
{
    public interface IActionSelector
    {
        Option SelectAction(IReadOnlyList<SelectorActions> actions);
    }
}