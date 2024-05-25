using System;
using LevAI.UtilityAI;
using Lions.AI.Contexts;
using UnityEngine;

namespace Lions.AI
{
    public class DistanceToTargetConsideration : ConsiderationBase<RestPlaceContext>
    {
        private readonly Func<IAgent, Transform> _targetGetter;

        public DistanceToTargetConsideration(Func<IAgent, Transform> targetGetter, IFunction function) : base(function)
        {
            _targetGetter = targetGetter;
        }

        protected override float GetInput(IAgent agent, RestPlaceContext context)
        {
            var target = _targetGetter(agent);
            if (!target) return 0.0f;
            return target.GetSqrDistance(context.Target);
        }
    }
}