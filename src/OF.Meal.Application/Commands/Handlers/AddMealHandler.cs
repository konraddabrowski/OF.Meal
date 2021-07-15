using System.Threading.Tasks;
using Arcaim.CQRS.Commands;
using Arcaim.CQRS.WebApi.Attributes;

namespace OF.Meal.Application.Commands.Handlers
{
    // [Authorize]
    [Authorize(Roles = "Admin, Kieras")]
    // [Authorize(Roles = "Boss")]
    public class AddMealHandler : ICommandHandler<AddMeal>
    {
        public AddMealHandler()
        {
        }

        [Validate]
        public Task HandleAsync(AddMeal command)
        {
            throw new System.NotImplementedException();
        }
    }
}