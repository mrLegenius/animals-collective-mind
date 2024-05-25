namespace LevAI.UtilityAI
{
    public class LinearFunction : IFunction
    {
        private readonly float _a;
        private readonly float _b;

        public LinearFunction(float a, float b)
        {
            _a = a;
            _b = b;
        }

        public float Execute(float x) => _a * x + _b;
    }
}
