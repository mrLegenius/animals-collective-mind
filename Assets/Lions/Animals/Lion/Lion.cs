using System.Linq;
using LevAI.UtilityAI;
using Lions.AI;
using Lions.Rest;
using UnityEngine;

namespace Lions.Animals.Lion
{
    public class Lion : Animal
    {
        [SerializeField] private Pride _pride;
        private string Group => _currentGroup?.Name ?? "";
        private string Region => _currentRegion?.name ?? "";
        
        private PrideRestPlaces _restPlaces;

        private LionsGroup _currentGroup;
        private PrideRegion _currentRegion;
        public Object CurrentRegion => _currentRegion;
        public LionsGroup CurrentGroup => _currentGroup;
        public Pride Pride => _pride;
        public bool IsInAmbush => _huntingRole == LionHuntingRole.Ambush;

        protected override Agent CreateBrains()
        {
            var agent = new LionAgent(this);
            agent.ActionGroupChanged += OnActionGroupChanged;
            
            return agent;
        }

        private void OnActionGroupChanged(string oldGroup, string group)
        {
            if (_pride.LeaveGroup(oldGroup, this))
                _currentGroup = null;
        }
        
        private LionHuntingRole _huntingRole;

        public void SetInfo<T>(string objectId, string key, T data, int priority) => Agent.SetData(objectId, key, data, priority);

        protected override void OnDied() => _pride.LeaveGroup(CurrentGroup?.Name, this);

        protected override void CollectObservation()
        {
            base.CollectObservation();

            if (_restPlaces)
            {
                var restPlaces = _restPlaces.RestPlaces;
            
                Agent.SetData( LionBlackboardKeys.AllRestPlaces, restPlaces, 5);
                Agent.SetData(LionBlackboardKeys.HasAnyRestPlace, restPlaces.Count > 0, 5);
            }
            
            if (_currentRegion)
                Agent.SetData(AnimalBlackboardKeys.AllWaterSources,
                _currentRegion.WaterReservoirs.SelectMany(x => x.AllSources).ToList(), 10);
            
            Agent.SetData(LionBlackboardKeys.AllMeatSources, _world.AllMeatSources, 5);
            Agent.SetData(LionBlackboardKeys.HasAnyMeatSource, _world.AllMeatSources.Count(x => x) > 0, 5);
            Agent.SetData(LionBlackboardKeys.AllPreys, _perception.ObservableObjects
                .Select(x =>
                {
                    if (!x) return null;
                    
                    x.TryGetComponent(out Prey.Prey component);
                    return component;
                })
                .Where(x => x), 10);
            
            Agent.SetData(LionBlackboardKeys.AllRegions, _world.AllRegions, 5);
        }

        public void SetRegion(PrideRegion region) => _currentRegion = region;
        public void SetRestPlace(PrideRestPlaces restPlaces) => _restPlaces = restPlaces;

        public void SetHuntingRole(LionHuntingRole huntingRole) => _huntingRole = huntingRole;
        public LionHuntingRole GetHuntingRole() => _huntingRole;

        public void JoinHunt() => _currentGroup = _pride.JoinOrCreateGroup(Pride.HuntingGroup, this);

        public void JoinMigration() => _currentGroup = _pride.JoinOrCreateGroup(Pride.MigrationGroup, this);
        
        public void LeaveGroup(string groupName)
        {
            _currentGroup = null;
            _pride.LeaveGroup(groupName, this);
        }
    }
}