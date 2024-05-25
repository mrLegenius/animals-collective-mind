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

        public override void RemoveLionToGroup(Lion lion)
        {
            base.RemoveLionToGroup(lion);
            lion.Agent.RemoveData(LionBlackboardKeys.MigrationRegion);
            lion.SetRestPlace(_targetRegion.RestPlace);
        }
    }
    
    public sealed class HuntGroup : LionsGroup
    {
        public override string Name => Pride.HuntingGroup;
        public bool IsAmbushReady => Lions.All(x => x.GetHuntingRole() == LionHuntingRole.Chase) || Lions.Any(x => x.GetHuntingRole() == LionHuntingRole.Ambush && IsAmbushReadyFor(x.Agent));

        private Prey _targetPrey;
        
        public override void AddLionToGroup(Lion lion)
        {
            if (Lions.Count == 0) 
                _targetPrey ??= lion.Agent.GetData<Prey>(LionBlackboardKeys.PreyTarget);

            lion.Agent.SetData(LionBlackboardKeys.PreyTarget, _targetPrey, 100);
            
            base.AddLionToGroup(lion);
            
            var role = Lions.Count % 2 == 0 ? LionHuntingRole.Ambush : LionHuntingRole.Chase;
            lion.SetHuntingRole(role);
        }

        public override void RemoveLionToGroup(Lion lion)
        {
            lion.SetHuntingRole(LionHuntingRole.None);
            base.RemoveLionToGroup(lion);
            
            if (Lions.Count == 1 && Lions.First().GetHuntingRole() == LionHuntingRole.Ambush)
                Lions.First().SetHuntingRole(LionHuntingRole.Chase);
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

        public virtual void RemoveLionToGroup(Lion lion) => Lions.Remove(lion);

        public void SendMessage(Lion from, string objectId, string key, object message)
        {
            foreach (Lion lion in Lions)
            {
                if (lion == from) continue;
                
                lion.SetInfo(objectId, key, message, 5);
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