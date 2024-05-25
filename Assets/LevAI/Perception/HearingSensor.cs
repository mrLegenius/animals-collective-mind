using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LevAI.Perception
{
    public partial class HearingSensor : MonoBehaviour, ISensor
    {
        [SerializeField] private float _radius;
        [SerializeField] private float _attenuation;
        [SerializeField] private int _priority;

        public IEnumerable<GameObject> ObservableGameObjects { get; } = Enumerable.Empty<GameObject>();
        public int Priority => _priority;
        public bool Tick()
        {
            return false;
        }
    }
}   