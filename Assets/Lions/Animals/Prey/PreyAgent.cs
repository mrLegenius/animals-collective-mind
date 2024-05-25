using System.Collections.Generic;
using LevAI.UtilityAI;
using Lions.AI;
using Lions.AI.Contexts;
using UnityEngine;

namespace Lions.Animals.Prey
{
    public class PreyAgent : Agent
    {
        private static readonly WaterSourceContextProducer WaterSourceContextProducer = new();
        private static readonly LionContextProducer LionContextProducer = new();
        private static readonly FearConsideration FearConsideration = new(new ClampedQuadraticInvertedFunction(100.0f, 2.0f));
        
        public PreyAgent(Object obj) : base(obj,
            new List<AIBehavior>
            {
                new()
                {
                    Action = new RestAnywhereAction(),
                    Considerations = new List<IConsideration>
                    {
                        new EnergyConsideration(new ClampedQuadraticInvertedFunction(0.4f, 0.4f)),
                        FearConsideration,
                    },
                    Combiner = new UtilityCombinerProduct() 
                },
                new()
                {
                    Action = new DrinkAction(),
                    Considerations = new List<IConsideration>
                    {
                        new ThirstConsideration(new ClampedQuadraticFunction(1, 4)),
                        AIUtils.GetDistanceConsideration(),
                        FearConsideration,
                    },
                    Combiner = new UtilityCombinerProduct(),
                    ContextProducer = WaterSourceContextProducer,
                },
                AIUtils.CreateGoToBehavior(WaterSourceContextProducer, new List<IConsideration>
                {
                    new ThirstConsideration(new ClampedQuadraticFunction(1, 4)),
                    AIUtils.GetDistanceConsideration(),
                    FearConsideration,
                }),
                new()
                {
                    Action = new WanderAction(),
                    Considerations = new List<IConsideration>
                    {
                        FearConsideration,
                    },
                    Combiner = new UtilityCombinerProduct()
                },
                new()
                {
                    Action = new RunAwayAction(),
                    Considerations = new List<IConsideration>
                    {
                        new FearConsideration(new ClampedQuadraticFunction(100.0f, 0.5f)),
                        AIUtils.GetDistanceConsideration(100),
                    },
                    Combiner = new UtilityCombinerProduct(),
                    ContextProducer = LionContextProducer,
                },
            },
            new DualReasoningActionSelector())
        {
        }
    }
}