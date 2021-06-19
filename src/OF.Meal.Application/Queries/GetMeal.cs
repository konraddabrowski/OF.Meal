using Arcaim.CQRS.Queries;

namespace OF.Meal.Application.Queries
{
    public class GetMeal : IQuery<Core.Entities.Meal>
    {
        public int MealId { get; set; }
    }
}