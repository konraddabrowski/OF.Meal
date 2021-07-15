using System.Threading.Tasks;
using OF.Meal.Core.Repositories;
using Arcaim.MyDapper;
using OF.Meal.Infrastructure.Exceptions;

namespace OF.Meal.Infrastructure.Repositories
{
    public class MealRepository : IMealRepository
    {
        private readonly IDapperWrapper _dapper;
        public MealRepository(IDapperWrapper dapper)
        {
            _dapper = dapper ?? throw Exceptions.ArgumentNullException.Create(nameof(dapper));
        }

        public async Task Create(string name, string description, string recipe)
        {
            string createUserSQL = @"
                INSERT INTO meal (name, description, recipe)
                VALUES (@name, @dscription, @recipe)"; //TODO: Add components and pictures
            await _dapper.ExecuteAsync(createUserSQL, new { name, description, recipe });
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