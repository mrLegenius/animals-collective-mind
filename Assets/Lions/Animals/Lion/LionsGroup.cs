using System.Collections.Generic;
using System.Linq;
using LevAI.UtilityAI;
using Lions.Animals.Lion;
using Lions.Animals.Prey;
using UnityEngine;

namespace Lions.AI
{
    public sealed class MigrationGroup : LionsGroup
    {
        public override string Name => Pride.MigrationGroup;

        private PrideRegion _targetRegion;
        
        public override void AddLionToGroup(Lion lion)
        {
            base.AddLionToGroup(lion);
            
            _targetRegion ??= lion.Agent.GetData<PrideRegion>(LionBlackboardKeys.MigrationRegion);
            lion.Agent.SetData(LionBlackboardKeys.MigrationRegion, _targetRegion, int.MaxValue);
        }

        public override bool RemoveLionToGroup(Lion lion)
        {
            var success = base.RemoveLionToGroup(lion);
            if (!success) return false;
            
            lion.Agent.RemoveData(LionBlackboardKeys.MigrationRegion);
            lion.SetRestPlace(_targetRegion.RestPlace);

            return true;
        }
    }
    
    public sealed class HuntGroup : LionsGroup
    {
        public override string Name => Pride.HuntingGroup;
        public bool IsAmbushReady => IsAmbushTriggered 
                                     || Lions.All(x => x.GetHuntingRole() == LionHuntingRole.Chase) 
                                     || Lions.Any(x => x.GetHuntingRole() == LionHuntingRole.Ambush && IsAmbushReadyFor(x.Agent));

        public bool IsAmbushTriggered { get; private set; }

        public void TriggerAmbush()
        {
            foreach (Lion lion in Lions)
            {
                lion.SetHuntingRole(LionHuntingRole.Chase);
            }

            IsAmbushTriggered = true;
        }
        
        private Prey _targetPrey;
        
        public override void AddLionToGroup(Lion lion)
        {
            if (Lions.Count == 0) 
                _targetPrey ??= lion.Agent.GetData<Prey>(LionBlackboardKeys.PreyTarget);

            lion.Agent.SetData(LionBlackboardKeys.PreyTarget, _targetPrey, 100);

            if (Lions.Count > 1)
            {
                var ambush = lion.Agent.GetData<Transform>(LionBlackboardKeys.Ambush);
                if (ambush != null)
                    lion.Agent.SetData(LionBlackboardKeys.Ambush, ambush, 100);
            }
            
            base.AddLionToGroup(lion);
            
            var role = Lions.Count % 2 == 0 && !IsAmbushTriggered ? LionHuntingRole.Ambush : LionHuntingRole.Chase;
            lion.SetHuntingRole(role);
        }

        public override bool RemoveLionToGroup(Lion lion)
        {
            var success = base.RemoveLionToGroup(lion);
            if (!success) return false;
            
            lion.SetHuntingRole(LionHuntingRole.None);

            if (Lions.Count == 1 && Lions.First().GetHuntingRole() == LionHuntingRole.Ambush)
            {
                Lions.First().SetHuntingRole(LionHuntingRole.Chase);
                IsAmbushTriggered = false;
            }

            return true;
        }
        
        private static bool IsAmbushReadyFor(IAgent agent)
        {
            var ambushTarget = agent.GetData<Transform>(LionBlackboardKeys.Ambush);
            if (ambushTarget == null) return false;

            return agent.GetSqrDistance(ambushTarget) < 2.0f;
        }
    }
    
    public abstract class LionsGroup
    {
        public abstract string Name { get; }
        public HashSet<Lion> Lions { get; } = new();

        private bool IsEmpty => Lions.Count <= 0;

        public virtual void AddLionToGroup(Lion lion) => Lions.Add(lion);

        public virtual bool RemoveLionToGroup(Lion lion) => Lions.Remove(lion);

        public void SendMessage(Lion from, string objectId, string key, object message, int priority = 5)
        {
            foreach (Lion lion in Lions)
            {
                if (lion == from) continue;
                
                lion.SetInfo(objectId, key, message, priority);
            }
        }
        
        public void SendMessage(Lion from, string key, object message, int priority = 5)
        {
            foreach (Lion lion in Lions)
            {
                if (lion == from) continue;
                
                lion.SetInfo(lion.GetInstanceID().ToString(), key, message, priority);
            }
        }
    }
}

//When lion starts hunting it creates or joins existing group and assigning role to itself depending on order of join

public enum LionHuntingRole
{
    None,
    Chase, // First lion in group and not second
    Ambush, // Second lion in group
}

//TODO: Chasing Lion 
/*
 *  it sends position of a target
 *  it sends moving direction of a target
 */ 
 
//TODO: Ambush Lion 
/*
 *  it sends position of itself
 */ 