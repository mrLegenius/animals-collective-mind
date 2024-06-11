using System;
using System.Collections.Generic;
using LevAI.UtilityAI;
using Lions.Animals;
using UnityEngine;

namespace Lions.AI
{
    public static class AIUtils
    {
        private const float MaxSqrDistance = 1000f;

        public static float GetSqrDistance(this IAgent agent, Transform transform) =>
            agent.GetSqrDistance(transform.position);
        
        public static float GetSqrDistance(this IAgent agent, Vector3 position) => 
            GetSqrDistance(agent.GetData<Animal>(AnimalBlackboardKeys.Animal).transform, position);

        public static float GetSqrDistance(this Transform obj, Transform transform) => GetSqrDistance(obj, transform.position);

        public static float GetSqrDistance(this Transform obj, Vector3 position)
        {
            Vector3 agentPosition = obj.position;
            agentPosition.y = 0;

            Vector3 targetPosition = position;
            targetPosition.y = 0;
            
            return Vector3.SqrMagnitude(agentPosition - targetPosition);
        }

        public static AIBehavior CreateGoToBehavior(IContextProducer producer, List<IConsideration> considerations, int baseRank = 0, string group = "", Action<IAgent> onFinished = null) =>
            new()
            {
                Action = new GoToAction(baseRank: baseRank, group: group, onFinished: onFinished),
                Considerations = considerations,
                Combiner = new UtilityCombinerProduct(),
                ContextProducer = producer,
            };

        private static IConsideration _distanceConsideration;
        public static IConsideration GetDistanceConsideration(float? maxDistance = null) => 
            new DistanceConsideration(GetDistanceFunction(maxDistance));

        public static IFunction GetDistanceFunction(float? maxDistance = null)
        {
            maxDistance ??= MaxSqrDistance;
            return new ClampedExponentialInvertedFunction(1f / maxDistance.Value);
        }
    }
}