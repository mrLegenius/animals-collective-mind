using UnityEngine;

namespace LevAI.WorkingMemory
{
    public interface IAgentMemory
    {
        void InsertData<T>(Object obj, string key, T data, int priority = AgentMemory.DefaultPriority);
        void InsertData<T>(string objectId, string key, T data, int priority = AgentMemory.DefaultPriority);
        T GetData<T>(Object obj, string key);
        T GetData<T>(string objectId, string key);
        void RemoveData(string objectId, string key);
    }
}