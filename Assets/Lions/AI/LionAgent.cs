using System.Collections.Generic;
using LevAI.UtilityAI;
using Lions.AI.Contexts;
using Lions.Animals;
using Lions.Animals.Lion;
using Lions.Animals.Prey;
using UnityEngine;

namespace Lions.AI
{
    public class LionAgent : Agent
    {
        private static readonly WaterSourceContextProducer WaterSourceContextProducer = new();
        private static readonly MeatSourceContextProducer MeatSourceContextProducer = new();
        private static readonly RestPlaceContextProducer RestPlaceContextProducer = new();
        private static readonly PreyContextProducer PreyContextProducer = new();
        private static readonly RegionContextProducer RegionContextProducer = new();
        
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
                    new GroupParticipatingConsideration(Pride.HuntingGroup),
                    new AmbushReadyConsideration(),
                    new HuntingRoleConsideration(LionHuntingRole.Chase),
                },
                Combiner = new UtilityCombinerProduct(),
            },
            new()
            {
                Action = new GoToSpecificPlace(agent =>
                {
                    var prey = agent.GetData<Prey>(LionBlackboardKeys.PreyTarget);
                    return prey ? prey.transform : null;
                }, 10, Pride.HuntingGroup),
                Considerations = new List<IConsideration>
                {
                    new HungerConsideration(new ClampedLinearFunction(1.0f)),
                    new GroupParticipatingConsideration(Pride.HuntingGroup),
                    new AmbushReadyConsideration(),
                    new HuntingRoleConsideration(LionHuntingRole.Chase),
                },
                Combiner = new UtilityCombinerProduct(),
            },
            new()
            {
                Action = new RunToSpecificPlace(agent => 
                {
                    var prey = agent.GetData<Prey>(LionBlackboardKeys.PreyTarget);
                    return prey ? prey.transform : null;
                }, 10, Pride.HuntingGroup),
                Considerations = new List<IConsideration>
                {
                    new HungerConsideration(new ClampedLinearFunction(1.0f)),
                    new GroupParticipatingConsideration(Pride.HuntingGroup),
                    new AmbushReadyConsideration(),
                    new HuntingRoleConsideration(LionHuntingRole.Chase),
                },
                Combiner = new UtilityCombinerProduct(),
            },
            new()
            {
                Action = new AttackAction(),
                Considerations = new List<IConsideration>
                {
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
                    new DistanceToTargetConsideration(agent =>
                    {
                        var prey = agent.GetData<Prey>(LionBlackboardKeys.PreyTarget);
                        return prey ? prey.transform : null;
                    }, AIUtils.GetDistanceFunction())
                },
                Combiner = new UtilityCombinerProduct(),
                ContextProducer = RestPlaceContextProducer,
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
        
        private LionAgent(Object obj, List<AIBehavior> behaviors) : base(obj, behaviors, new DualReasoningActionSelector()) { }
    }
}