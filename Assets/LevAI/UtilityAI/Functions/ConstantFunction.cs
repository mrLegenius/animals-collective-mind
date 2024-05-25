namespace LevAI.UtilityAI
{
    public class ConstantFunction : IFunction
    {
        private readonly float _a;

        public ConstantFunction(float a) => _a = a;

        public float Execute(float _) => _a;
    }
}