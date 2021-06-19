namespace OF.Meal.Core.Entities
{
    public class Meal
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        // public IEnumerable<Component> Components { get; set; }
        public string Recipe { get; set; }
        // public IEnumerable<Picture> Pictures { get; set; }
    }
}