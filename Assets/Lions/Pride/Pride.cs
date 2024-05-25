using System;
using System.Collections.Generic;
using System.Linq;
using Lions.AI;
using Lions.Animals.Lion;
using UnityEngine;

namespace Lions
{
    public class Pride : MonoBehaviour
    {
        public const string HuntingGroup = "hunting";
        public const string MigrationGroup = "migration";
        
        [SerializeField] private PrideRegion _currentRegion;
        [SerializeField] private World _world;

        private List<Lion> _lions = new();
        private readonly Dictionary<string, LionsGroup> _groups = new();
        
        public IReadOnlyList<Lion> Lions => _lions;
        
        private void Awake()
        {
            _lions = GetComponentsInChildren<Lion>().ToList();
            
            AssignLionsNewRestPlace();
        }

        public LionsGroup JoinOrCreateGroup(string groupName, Lion lion)
        {
            _groups.TryAdd(groupName, CreateGroup(groupName));
            var group = _groups.GetValueOrDefault(groupName);
            
            if (group == null) return null;
            group.AddLionToGroup(lion);
            return group;
        }

        public void LeaveGroup(string groupName, Lion lion)
        {
            if (!_groups.TryGetValue(groupName, out var group)) return;
            
            group.RemoveLionToGroup(lion);

            if (group.Lions.Count <= 0)
                _groups.Remove(groupName);
        }

        private static LionsGroup CreateGroup(string groupName) =>
            groupName switch
            {
                HuntingGroup => new HuntGroup(),
                MigrationGroup => new MigrationGroup(),
                _ => null,
            };

        private void AssignLionsNewRestPlace()
        {
            _lions.ForEach(x => x.SetRegion(_currentRegion));
            _lions.ForEach(x => x.SetRestPlace(_currentRegion.RestPlace));
        }

        public bool IsGroupExists(string groupName) => _groups.ContainsKey(groupName);
    }
}
