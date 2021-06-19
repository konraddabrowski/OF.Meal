namespace OF.Meal.Core.Exceptions
{
    public abstract class InfrastructureException : System.Exception
    {
        public abstract string Code { get; }
        public abstract int StatusCode { get; }
        public override string Source { get => "OF.Meal.Infrastructure"; }

        public InfrastructureException(string message) : base(message)
        {
        }
    }
}