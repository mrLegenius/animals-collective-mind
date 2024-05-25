using UnityEngine;

namespace LevAI.UtilityAI
{
    public interface IAgent
    {
        T GetData<T>(string key);
        void SetData<T>(string key, T data, int priority);
        T GetValue<T>(string objectId, string key);
        T GetValue<T>(Object obj, string key);
        void SetData<T>(string objectId, string key, T data, int priority);
        IAction CurrentAction { get; }
        IContext CurrentActionContext { get; }
        string ActionGroup { get; }
        void RemoveData(string objectId, string key);
        void RemoveData(string key);
    }
}