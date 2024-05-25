using System;

namespace LevAI.UtilityAI
{
    public interface IContext : IEquatable<IContext>
    {
        public string Name { get; }
        bool IsValid();
    }
}