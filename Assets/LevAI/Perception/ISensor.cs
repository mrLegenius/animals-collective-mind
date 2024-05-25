using System.Collections.Generic;
using UnityEngine;

namespace LevAI.Perception
{
    public interface ISensor
    {
        bool Tick();
        IEnumerable<GameObject> ObservableGameObjects { get; }
        int Priority { get; }
    }
}