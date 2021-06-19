namespace OF.Meal.Core.Settings
{
    public class CookieSetting
    {
        public static string Cookie = "Cookie";
        public string Name { get; set; }
        public bool HttpOnly { get; set; }
        public bool IsEssential { get; set; }
    }
}