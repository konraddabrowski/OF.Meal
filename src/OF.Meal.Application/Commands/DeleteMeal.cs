using Arcaim.CQRS.Commands;

namespace OF.Meal.Application.Commands
{
    public class DeleteMeal : ICommand
    {
        public int MealId { get; set; }
    }
}