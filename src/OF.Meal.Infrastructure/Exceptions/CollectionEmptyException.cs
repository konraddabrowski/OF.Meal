using Microsoft.AspNetCore.Http;
using OF.Meal.Core.Exceptions;

namespace OF.Meal.Infrastructure.Exceptions
{
    public class CollectionEmptyException : InfrastructureException
    {
        public override string Code => "collection_empty";
        public override int StatusCode => StatusCodes.Status204NoContent;

        public CollectionEmptyException(string message) : base(message)
        {
        }

        private static CollectionEmptyException CreateCollectionEmptyException(string message)
            => new CollectionEmptyException(message);
        
        public static CollectionEmptyException Create(string collectionName)
            => CreateCollectionEmptyException($"The collection {collectionName} is empty");
    }
}