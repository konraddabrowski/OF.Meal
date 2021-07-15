using System;
using Microsoft.AspNetCore.Http;
using OF.Meal.Core.Exceptions;

namespace OF.Meal.Infrastructure.Exceptions
{
    public class ArgumentNullException : InfrastructureException
    {
        public override string Code => "argument_null";
        public override int StatusCode => StatusCodes.Status500InternalServerError;

        private ArgumentNullException(string message) : base(message)
        {
        }

        private static ArgumentNullException CreateArgumentNullException(string message)
            => new ArgumentNullException(message);

        public static ArgumentNullException Create(string argumentName)
            => CreateArgumentNullException($"The argument {argumentName} is null");
        
        public static ArgumentNullException Create(Exception excepion)
            => CreateArgumentNullException(excepion.Message);
    }
}