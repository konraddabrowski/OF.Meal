namespace OF.Meal.Core.Settings
{
    public class ServicesSetting
    {
        public static string Services = "Services";
        public AuthSetting Auth { get; set; }
    }

    public class AuthSetting
    {
        public static string Auth = $"{ServicesSetting.Services}::Auth";
        public string Address { get; set; }
    }
    
}