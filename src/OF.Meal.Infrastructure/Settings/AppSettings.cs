using Microsoft.Extensions.Configuration;
using OF.Meal.Core.Settings;
using OF.Meal.Infrastructure.Exceptions;
using static OF.Meal.Core.Settings.ServicesSetting;

namespace OF.Meal.Infrastructure.Settings
{
    public class AppSettings : IAppSettings
    {
        public ConnectionStringsSetting ConnectionStrings { get; init; }
        public SessionSetting Session { get; init; }
        public ServicesSetting Services { get; init; }

        public AppSettings(IConfiguration configuration)
        {
            if (configuration is null)
            {
                throw ArgumentNullException.Create(nameof(configuration));
            }

            ConnectionStrings = configuration
                .GetSection(ConnectionStringsSetting.ConnectionStrings)
                .Get<ConnectionStringsSetting>();
            Session = configuration
                .GetSection(SessionSetting.Session)
                .Get<SessionSetting>();
            Services = new()
            {
                Auth = configuration.GetSection(AuthSetting.Auth).Get<AuthSetting>()
            };
            
        }
    }
}