namespace OF.Meal.Core.Exceptions
{
    public abstract class ApplicationException : System.Exception
    {
        public abstract string Code { get; }
        public abstract int StatusCode { get; }
        public override string Source { get => "OF.Meal.Application"; }

        public ApplicationException(string message) : base(message)
        {
        }
    }
}