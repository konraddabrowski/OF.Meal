using System.Collections.Generic;
using Arcaim.CQRS.Commands;

namespace OF.Meal.Application.Commands
{
    public class AddMeal : ICommand
    {
        public string Title { get; set; }
        public string Description { get; set; }
        // public IEnumerable<Component> Components { get; set; }
        public string Recipe { get; set; }
        // public IEnumerable<Picture> Pictures { get; set; }
    }
}