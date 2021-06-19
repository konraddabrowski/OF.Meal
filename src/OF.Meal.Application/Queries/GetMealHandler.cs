using System.Threading.Tasks;
using Arcaim.CQRS.Queries;
using OF.Meal.Core.Repositories;

namespace OF.Meal.Application.Queries
{
    public class GetMealHandler : IQueryHandler<GetMeal, Core.Entities.Meal>
    {
        private readonly IMealRepository _repository;

        public GetMealHandler(IMealRepository repository)
        {
            _repository = repository;
        }

        public Task<Core.Entities.Meal> HandleAsync(GetMeal query)
        {
            throw new System.NotImplementedException();
        }
    }
}