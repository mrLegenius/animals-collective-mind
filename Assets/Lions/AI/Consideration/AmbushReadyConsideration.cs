﻿using LevAI.UtilityAI;
using Lions.Animals;
using Lions.Animals.Lion;

namespace Lions.AI
{
    public class AmbushReadyConsideration : IConsideration
    {
        private readonly bool _waitForReady;

        public AmbushReadyConsideration(bool waitForReady = true)
        {
            _waitForReady = waitForReady;
        }
        public int CalculateRank(IAgent agent, IAction action, IContext context) => 0;

        public float Execute(IAgent agent, IContext context)
        {
            var group = agent.GetData<Lion>(AnimalBlackboardKeys.Animal).CurrentGroup;

            if (group == null) return 0.0f;
            if (group.Name != Pride.HuntingGroup) return 0.0f;
            
            var huntingGroup = (HuntGroup)group;
            
            
            return huntingGroup.IsAmbushReady == _waitForReady ? 1.0f : 0.0f;
        }
    }
}