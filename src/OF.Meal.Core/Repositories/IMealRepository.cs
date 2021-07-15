using System.Threading.Tasks;

namespace OF.Meal.Core.Repositories
{
    public interface IMealRepository
    {
        Task Create(string name, string description, string recipe);
        Task<Core.Entities.Meal> Get(int id);
        Task Delete(int id);
        Task Update(Core.Entities.Meal meal);
    }
}