using System.Threading.Tasks;
using OF.Meal.Core.Repositories;

namespace OF.Meal.Infrastructure.Repositories
{
    public class MealRepository : IMealRepository
    {
        public async Task Create(Core.Entities.Meal meal)
        {
        }

        public async Task Delete(int id)
        {
        }

        public async Task<Core.Entities.Meal> Get(int id)
        {
            return null;
        }

        public async Task Update(Core.Entities.Meal meal)
        {
        }
    }
}