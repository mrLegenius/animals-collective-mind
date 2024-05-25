using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LevAI.UtilityAI
{
    public sealed class DualReasoningActionSelector : IActionSelector
    {
        private readonly RelativeActionSelector _relativeActionSelector = new();

        private readonly List<SelectorActions> _possibleActions = new();
        private readonly List<SelectorActions> _maxRankActions = new();
        private readonly List<SelectorActions> _bestActions = new();
        
        public Option SelectAction(IReadOnlyList<SelectorActions> actions)
        {
            if (actions.Count == 0) return default;
            
            //1. Eliminate all options with a weight of 0.

            _possibleActions.Clear();
            foreach (SelectorActions action in actions)
            {
                if (!Mathf.Approximately(action.Score, 0))
                    _possibleActions.Add(action);
            }

            if (_possibleActions.Count == 0) return default;
            
            //2. Determine the highest rank category, and eliminate options with lower rank.

            var maxRank = int.MinValue;
            
            foreach (var action in _possibleActions)
            {
                if (action.Rank > maxRank)
                    maxRank = action.Rank;
            }
            
            _maxRankActions.Clear();

            foreach (var action in _possibleActions)
            {
                if (action.Rank == maxRank)
                    _maxRankActions.Add(action);
            }
            
            //3. Eliminate options whose weight is significantly less than that of the best remaining option.
            
            var maxScore = float.MinValue;
            
            foreach (var action in _maxRankActions)
            {
                if (action.Score > maxScore)
                    maxScore = action.Score;
            }
            
            const float scoreThreshold = 0.2f;
            
            _bestActions.Clear();

            foreach (var action in _maxRankActions)
            {
                if (maxScore - scoreThreshold <= action.Score + float.Epsilon) 
                    _bestActions.Add(action);
            }
            
            //4. Use weighted random on the options that remain.
            return _relativeActionSelector.SelectAction(_bestActions);
        }
    }
    public sealed class RelativeActionSelector : IActionSelector
    {
        public Option SelectAction(IReadOnlyList<SelectorActions> actions)
        {
            float scoreSum = actions.Select(x => x.Score).Sum();
            float score = 0;

            float randomScore = Random.Range(0, scoreSum);

            var sortedActions = actions.Where(x => !Mathf.Approximately(x.Score, 0)).OrderBy(x => x.Score);
            foreach (SelectorActions action in sortedActions)
            {
                score += action.Score;
                
                if (score > randomScore)
                    return new Option(action.Action, action.Context);
            }

            return default;
        }
    }
    public sealed class AbsoluteActionSelector : IActionSelector
    {
        public Option SelectAction(IReadOnlyList<SelectorActions> actions)
        {
            var selectedAction = actions
                .Where(x => !Mathf.Approximately(x.Score, 0))
                .OrderBy(x => x.Score)
                .FirstOrDefault();
            
            return new Option(selectedAction.Action, selectedAction.Context);
        }
    }
}