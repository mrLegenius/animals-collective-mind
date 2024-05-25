using LevAI.UtilityAI;
using Lions.Animals;
using Lions.Animals.Lion;

namespace Lions.AI
{
    public class GroupParticipatingConsideration : IConsideration
    {
        public enum Type
        {
            Existence,
            Absence,
        }
        private readonly string _group;
        private readonly bool _inversed;

        public GroupParticipatingConsideration(string group, Type type = Type.Existence)
        {
            _group = group;
            _inversed = type == Type.Absence;
        }

        public int CalculateRank(IAgent agent, IAction action, IContext context) => 0;

        public float Execute(IAgent agent, IContext context)
        {
            var lion = agent.GetData<Lion>(AnimalBlackboardKeys.Animal);

            if (lion.CurrentGroup == null) return _inversed ? 1.0f : 0.0f;
            
            if (_inversed)
                return lion.CurrentGroup.Name == _group ? 0.0f : 1.0f;
            
            return lion.CurrentGroup.Name == _group ? 1.0f : 0.0f;
        }
    }
}