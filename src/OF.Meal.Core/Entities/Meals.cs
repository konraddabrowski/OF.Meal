using System.Collections;
using System.Collections.Generic;

namespace OF.Meal.Core.Entities
{
    public class Meals : IEnumerable<Meal>
    {
        private readonly List<Meal> _meals = new();

        public Meal this[int index]
        {
            get => _meals[index];
            set => _meals.Insert(index, value);
        }

        public IEnumerator<Meal> GetEnumerator()
            => _meals.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => this.GetEnumerator();
    }
}