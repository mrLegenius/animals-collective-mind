using System.Collections.Generic;
using System.Linq;

namespace LevAI.WorkingMemory
{
    public sealed class ObjectStimuli : Dictionary<string, Stimulus>
    {
        public void SetValue<T>(string key, T value, int priority)
        {
            var newValue = new Stimulus { Priority = priority, Data = value };
            if (base.TryGetValue(key, out Stimulus stimulus))
            {
                if (stimulus.Priority > priority) return;
                
                this[key] = newValue;
                return;
            }

            Add(key, newValue);
        }
        
        public bool TryGetValue<T>(string key, out T value)
        {
            if (base.TryGetValue(key, out Stimulus obj))
            {
                value = (T)obj.Data;
                return true;
            }
            
            value = default;
            return false;
        }

        public void DecreaseAllPriorities()
        {
            var keys = Keys.ToArray();
            foreach (var key in keys)
            {
                base.TryGetValue(key, out Stimulus stimulus);
                stimulus.Priority--;
                this[key] = stimulus;
            }
        }
    }
}