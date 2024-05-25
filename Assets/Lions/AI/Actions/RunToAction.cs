using LevAI.UtilityAI;
using Lions.Animals;

namespace Lions.AI
{
    public class RunToAction : GoToAction
    {
        public RunToAction(int baseRank = 0, string group = "") : base(baseRank, group) { }
        public override int GetBaseRank(IAgent agent) => 2 + (agent.ActionGroup == Group ? 10 : 0);

        protected override float Speed(IAgent agent) => 
            agent.GetData<AnimalStats>(AnimalBlackboardKeys.Stats).RunSpeed;
        protected override float EnergyLoss(IAgent agent) => 
            agent.GetData<AnimalStats>(AnimalBlackboardKeys.Stats).EnergyLossRateOnRun;
    }
}