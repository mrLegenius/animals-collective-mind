using System.Collections.Generic;
using LevAI.WorkingMemory;
using UnityEngine;

namespace LevAI.Perception
{
    public class Perception : MonoBehaviour
    {
        private readonly List<ISensor> _sensors = new();
        public HashSet<GameObject> ObservableObjects { get; } = new();

        private IAgentMemory _agentMemory;

        private void Awake()
        {
            _sensors.Clear();
            GetComponentsInChildren(_sensors);
        }

        public void SetAgent(IAgentMemory memory) => _agentMemory = memory;

        private void Update()
        {
            ObservableObjects.Clear();
            foreach (ISensor sensor in _sensors)
            {
                sensor.Tick();

                foreach (GameObject go in sensor.ObservableGameObjects)
                {
                    if (!go) return;
                    
                    ObservableObjects.Add(go);
                    _agentMemory.InsertData(go.GetInstanceID().ToString(), "position", go.transform.position, sensor.Priority);
                }
            }
        }
    }
}