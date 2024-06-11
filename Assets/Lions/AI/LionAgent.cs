using System;
using System.Collections.Generic;
using System.Linq;
using LevAI.UtilityAI;
using Lions.AI.Contexts;
using Lions.Animals;
using Lions.Animals.Lion;
using Lions.Animals.Prey;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Lions.AI
{
    public class LionAgent : Agent
    {
        private static readonly WaterSourceContextProducer WaterSourceContextProducer = new();
        private static readonly MeatSourceContextProducer MeatSourceContextProducer = new();
        private static readonly RestPlaceContextProducer RestPlaceContextProducer = new();
        private static readonly PreyContextProducer PreyContextProducer = new();
        private static readonly RegionContextProducer RegionContextProducer = new();
        private static readonly TallGrassContextProducer TallGrassContextProducer = new();
        
        public LionAgent(Object obj) : this(obj, new List<AIBehavior>
        {
            //Resting
            new()
            {
                Action = new RestAnywhereAction(),
                Considerations = new List<IConsideration>
                {
                    new EnergyConsideration(new ClampedQuadraticInvertedFunction(1.0f, 0.4f)),
                },
                Combiner = new UtilityCombinerProduct()
            },
            new()
            {
                Action = new RestAction(),
                Considerations = new List<IConsideration>
                {
                    new EnergyConsideration(new ClampedQuadraticInvertedFunction(1.0f, 0.5f)),
                },
                Combiner = new UtilityCombinerProduct(),
                ContextProducer = RestPlaceContextProducer,
            },
            AIUtils.CreateGoToBehavior(RestPlaceContextProducer, new List<IConsideration>
            {
                new EnergyConsideration(new ClampedQuadraticInvertedFunction(1.0f, 0.5f)),
                AIUtils.GetDistanceConsideration(),
            }),

            //Drinking
            new()
            {
                Action = new DrinkAction(),
                Considerations = new List<IConsideration>
                {
                    new ThirstConsideration(new ClampedLinearFunction(1.0f)),
                },
                Combiner = new UtilityCombinerProduct(),
                ContextProducer = WaterSourceContextProducer,
            },
            AIUtils.CreateGoToBehavior(WaterSourceContextProducer, new List<IConsideration>
            {
                new ThirstConsideration(new ClampedLinearFunction(1.0f)),
                AIUtils.GetDistanceConsideration(),
            }),

            //Eating
            AIUtils.CreateGoToBehavior(MeatSourceContextProducer, new List<IConsideration>
            {
                new HungerConsideration(new ClampedLinearFunction(1.0f)),
                AIUtils.GetDistanceConsideration(),
            }),
            new()
            {
                Action = new EatAction(),
                Considerations = new List<IConsideration>
                {
                    new HungerConsideration(new ClampedLinearFunction(1.0f)),
                },
                Combiner = new UtilityCombinerProduct(),
                ContextProducer = MeatSourceContextProducer,
            },

            //Hunting
            new()
            {
                Action = new JoinHuntGroupAction(),
                Considerations = new List<IConsideration>
                {
                    new MeatAbsenceConsideration(),
                    new GroupParticipatingConsideration(Pride.HuntingGroup, GroupParticipatingConsideration.Type.Absence),
                },
                Combiner = new UtilityCombinerProduct(),
                ContextProducer = PreyContextProducer,
            },
            //--Chase
            new()
            {
                Action = new WaitForAmbushAction(),
                Considerations = new List<IConsideration>
                {
                    new MeatAbsenceConsideration(),
                    new GroupParticipatingConsideration(Pride.HuntingGroup),
                    new AmbushReadyConsideration(false),
                    new HuntingRoleConsideration(LionHuntingRole.Chase),
                    new HasHuntingRoleInGroupConsideration(LionHuntingRole.Ambush),
                },
                Combiner = new UtilityCombinerProduct(),
            },
            new()
            {
                Action = new GoToSpecificPoint(AmbushChasePreyPosition, 10, Pride.HuntingGroup),
                Considerations = new List<IConsideration>
                {
                    new MeatAbsenceConsideration(),
                    new HungerConsideration(new ClampedLinearFunction(1.0f)),
                    new GroupParticipatingConsideration(Pride.HuntingGroup),
                    new HuntingRoleConsideration(LionHuntingRole.Chase),
                    new DistanceFromAgentToTargetConsideration(agent =>
                    {
                        var prey = agent.GetData<Prey>(LionBlackboardKeys.PreyTarget);
                        return prey ? prey.transform : null;
                    }, AIUtils.GetDistanceFunction(), 5.0f),
                },
                Combiner = new UtilityCombinerProduct(),
            },
            new()
            {
                Action = new RunToSpecificPoint(AmbushChasePreyPosition, 10, Pride.HuntingGroup),
                Considerations = new List<IConsideration>
                {
                    new MeatAbsenceConsideration(),
                    new HungerConsideration(new ClampedLinearFunction(1.0f)),
                    new GroupParticipatingConsideration(Pride.HuntingGroup),
                    new AmbushReadyConsideration(),
                    new HuntingRoleConsideration(LionHuntingRole.Chase),
                    new DistanceFromAgentToTargetConsideration(agent =>
                    {
                        var prey = agent.GetData<Prey>(LionBlackboardKeys.PreyTarget);
                        return prey ? prey.transform : null;
                    }, AIUtils.GetDistanceFunction(), 2.0f),
                },
                Combiner = new UtilityCombinerProduct(),
            },
            new()
            {
                Action = new AttackAction(),
                Considerations = new List<IConsideration>
                {
                    new MeatAbsenceConsideration(),
                    new ConstantConsideration(100),
                    new GroupParticipatingConsideration(Pride.HuntingGroup),
                },
                Combiner = new UtilityCombinerProduct(),
                ContextProducer = PreyContextProducer,
            },
            //--Ambush
            new()
            {
                Action = new ChooseAmbushAction(),
                Considerations = new List<IConsideration>
                {
                    new GroupParticipatingConsideration(Pride.HuntingGroup),
                    new HuntingRoleConsideration(LionHuntingRole.Ambush),
                    new DistanceFromTargetToContextConsideration(agent =>
                    {
                        var prey = agent.GetData<Prey>(LionBlackboardKeys.PreyTarget);
                        return prey ? prey.transform : null;
                    }, AIUtils.GetDistanceFunction()),
                },
                Combiner = new UtilityCombinerProduct(),
                ContextProducer = TallGrassContextProducer,
            },
            new()
            {
                Action = new GoToSpecificPlace(agent =>
                {
                    var ambush = agent.GetData<Transform>(LionBlackboardKeys.Ambush);
                    return ambush;
                }, 10, Pride.HuntingGroup),
                Considerations = new List<IConsideration>
                {
                    new AmbushReadyConsideration(false),
                    new MeatAbsenceConsideration(),
                    new HungerConsideration(new ClampedLinearFunction(1.0f)),
                    new GroupParticipatingConsideration(Pride.HuntingGroup),
                    new HuntingRoleConsideration(LionHuntingRole.Ambush),
                },
                Combiner = new UtilityCombinerProduct(),
            },
            new()
            {
                Action = new WaitForAmbushTriggeredAction(),
                Considerations = new List<IConsideration>
                {
                    new AmbushReadyConsideration(),
                    new MeatAbsenceConsideration(),
                    new HungerConsideration(new ClampedLinearFunction(1.0f)),
                    new GroupParticipatingConsideration(Pride.HuntingGroup),
                    new HuntingRoleConsideration(LionHuntingRole.Ambush),
                },
                Combiner = new UtilityCombinerProduct(),
            },
            new()
            {
                Action = new TriggerAmbushAction(),
                Considerations = new List<IConsideration>
                {
                    new GroupParticipatingConsideration(Pride.HuntingGroup),
                    new HuntingRoleConsideration(LionHuntingRole.Ambush),
                    new AmbushReadyConsideration(),
                },
                Combiner = new UtilityCombinerProduct(),
            },

            //Migrating
            new()
            {
                Action = new JoinMigrationGroupAction(),
                Considerations = new List<IConsideration>
                {
                    new GroupParticipatingConsideration(Pride.MigrationGroup, GroupParticipatingConsideration.Type.Absence),
                    AIUtils.GetDistanceConsideration(100),
                    new WaterDensityConsideration(new ClampedLinearFunction(1.0f)),
                },
                Combiner = new UtilityCombinerProduct(),
                ContextProducer = RegionContextProducer,
            },
            new()
            {
                Action = new GoToSpecificPlace(agent => agent.GetData<PrideRegion>(LionBlackboardKeys.MigrationRegion)?.transform, 100, Pride.MigrationGroup, agent =>
                    {
                        var lion = agent.GetData<Lion>(AnimalBlackboardKeys.Animal);
                        var region = agent.GetData<PrideRegion>(LionBlackboardKeys.MigrationRegion);
                        lion.SetRegion(region);
                
                        lion.LeaveGroup(Pride.MigrationGroup);
                        lion.Agent.RemoveData(LionBlackboardKeys.MigrationRegion);
                    }),
                Considerations = new List<IConsideration>
                {
                    new GroupParticipatingConsideration(Pride.MigrationGroup, GroupParticipatingConsideration.Type.Existence),
                },
                Combiner = new UtilityCombinerProduct(),
            },
        })
        {
        }

        private static Vector3 AmbushChasePreyPosition(IAgent agent)
        {
            const int SafeDistance = 50;
            const int ChaseDistance = 10;
            
            var prey = agent.GetData<Prey>(LionBlackboardKeys.PreyTarget).transform;

            var ambush = agent.GetData<Transform>(LionBlackboardKeys.Ambush);

            if (!ambush)
                return prey.position;

            var lion = agent.GetData<Lion>(AnimalBlackboardKeys.Animal);
            var lionTransform = lion.transform;
            if (lion.CurrentGroup is HuntGroup { IsAmbushTriggered: true })
                return prey.position;
            
            if ((prey.position - lionTransform.position).magnitude < ChaseDistance + 1) 
                return prey.position;

            var distance = lion.CurrentGroup is HuntGroup { IsAmbushReady: true } ? ChaseDistance : SafeDistance;
            
            var fromPreyToAmbush = prey.position - ambush.position;
            var directionFromTarget = fromPreyToAmbush.normalized * distance;
            var target = prey.position + directionFromTarget;

            var fromLionToTarget = target - lionTransform.position;

            if (Vector3.Dot(fromPreyToAmbush, fromLionToTarget) < -0.9) 
                return prey.position + directionFromTarget;

            var safeOffsetDirection = Vector3.Cross(fromLionToTarget, Vector3.up).normalized * SafeDistance;
            var opposite = -safeOffsetDirection;

            var safeDistance = prey.position + safeOffsetDirection;
            var oppositeDistance = prey.position + opposite;
            
            if ((safeDistance - lionTransform.position).sqrMagnitude < (oppositeDistance - lionTransform.position).sqrMagnitude)
                target = safeDistance;
            else
                target = oppositeDistance;

            return target;
        }

        private LionAgent(Object obj, List<AIBehavior> behaviors) : base(obj, behaviors, new DualReasoningActionSelector()) { }
    }
}