using System.Collections.Generic;
using UnityEngine;

namespace LevAI.WorkingMemory
{
    public sealed class AgentMemory : IAgentMemory, IUpdatableMemory
    {
        public const int DefaultPriority = 5;
        private readonly Dictionary<string, ObjectStimuli> _memorizedObjects = new();

        private readonly List<KeyValuePair<string, string>> _dataToDelete = new();
        private readonly List<string> _objectsToDelete = new();
        
        public void Update()
        {
            UpdateObjects();
            UpdateStimuli();
        }

        public void InsertData<T>(Object obj, string key, T data, int priority = DefaultPriority)
        {
            var objectId = obj.GetInstanceID().ToString();
            InsertData(objectId, key, data, priority);
        }
        
        public void InsertData<T>(string objectId, string key, T data, int priority = DefaultPriority)
        {
            _memorizedObjects.TryAdd(objectId, new ObjectStimuli());
            _memorizedObjects[objectId].SetValue(key, data, priority);
        }

        public void RemoveData(string objectId, string key)
        {
            if (_memorizedObjects.TryGetValue(objectId, out var stimuli))
                stimuli.Remove(key);
        }
        
        public T GetData<T>(Object obj, string key)
        {
            var objectId = obj.GetInstanceID().ToString();
            return GetData<T>(objectId, key);
        }

        public T GetData<T>(string objectId, string key)
        {
            if (_memorizedObjects.TryGetValue(objectId, out ObjectStimuli stimuli))
                return stimuli.TryGetValue(key, out T data) ? data : default;

            return default;
        }
        
        private void UpdateStimuli()
        {
            _dataToDelete.Clear();

            foreach ((string objectId, ObjectStimuli stimuli) in _memorizedObjects)
            {
                stimuli.DecreaseAllPriorities();
                foreach ((string dataKey, Stimulus stimulus) in stimuli)
                {
                    if (stimulus.Priority <= 0)
                        _dataToDelete.Add(new KeyValuePair<string, string>(objectId, dataKey));
                }
            }

            foreach ((string objectId, string dataId) in _dataToDelete)
            {
                _memorizedObjects[objectId].Remove(dataId);
            }
        }
        
        private void UpdateObjects()
        {
            _objectsToDelete.Clear();

            foreach ((string objectId, ObjectStimuli stimuli) in _memorizedObjects)
            {
                if (stimuli.Count == 0)
                    _objectsToDelete.Add(objectId);
            }

            foreach (string objectId in _objectsToDelete)
            {
                _memorizedObjects.Remove(objectId);
            }
        }
    }
}