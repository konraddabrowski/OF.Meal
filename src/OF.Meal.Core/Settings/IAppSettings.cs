namespace OF.Meal.Core.Settings
{
    public interface IAppSettings
    {
        ConnectionStringsSetting ConnectionStrings { get; init; }
        SessionSetting Session { get; init; }
        ServicesSetting Services { get; init; }
    }
}