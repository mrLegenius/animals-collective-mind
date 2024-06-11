using System;
using LevAI.UtilityAI;
using Lions.AI.Contexts;
using UnityEngine;

namespace Lions.AI
{
    public class DistanceFromAgentToTargetConsideration : ConsiderationBase
    {
        private readonly Func<IAgent, Transform> _targetGetter;
        private readonly float _maxDistance;

        public DistanceFromAgentToTargetConsideration(Func<IAgent, Transform> targetGetter, IFunction function, float? maxDistance = null) : base(function)
        {
            _targetGetter = targetGetter;
            var value = maxDistance.GetValueOrDefault();
            _maxDistance = maxDistance != null ? value * value : float.MaxValue;
        }

        protected override float GetInput(IAgent agent)
        {
            var target = _targetGetter(agent);
            if (!target) return 0.0f;
            
            var sqrDistance = agent.GetSqrDistance(target);
            
            return sqrDistance > _maxDistance ? 0.0f : sqrDistance;
        }
    }
    
    public class DistanceFromTargetToContextConsideration : ConsiderationBase<RestPlaceContext>
    {
        private readonly Func<IAgent, Transform> _targetGetter;
        private readonly float _maxDistance;

        public DistanceFromTargetToContextConsideration(Func<IAgent, Transform> targetGetter, IFunction function, float? maxDistance = null) : base(function)
        {
            _targetGetter = targetGetter;
            var value = maxDistance.GetValueOrDefault();
            _maxDistance = maxDistance != null ? value * value : float.MaxValue;
        }

        protected override float GetInput(IAgent agent, RestPlaceContext context)
        {
            var target = _targetGetter(agent);
            if (!target) return 0.0f;
            
            var sqrDistance = target.GetSqrDistance(context.Target);
            
            return sqrDistance > _maxDistance ? 0.0f : sqrDistance;
        }
    }
}