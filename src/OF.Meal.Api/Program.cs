using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using Arcaim.Assertor;
using Arcaim.Exception;
using Arcaim.MyDapper;
using Arcaim.CQRS.WebApi;
using OF.Meal.Infrastructure.Settings;
using OF.Meal.Core.Settings;
using OF.Meal.Infrastructure.Mappers;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace OF.Meal.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging(logging => {
                    logging.ClearProviders();
                    logging.AddConsole();
                })
                .ConfigureServices((hostContext, services) => services
                    .AddSingleton<IHttpContextAccessor, HttpContextAccessor>()
                    .AddSingleton<IAppSettings, AppSettings>()
                    .AddTransient<ErrorHandlerMiddleware>()
                    .AddSingleton<IExceptionToResponseMapper, ExceptionToResponseMapper>()
                    
                    .AddMyDapper(() => 
                    {
                        var connectionString = hostContext.Configuration
                            .GetSection(ConnectionStringsSetting.ConnectionStrings)
                            .Get<ConnectionStringsSetting>()
                            .DefaultConnection;
                        
                        return new MySqlConnection(connectionString);
                    })
                    // .AddScoped<IUserRepository, UserRepository>()
                    .AddDistributedMemoryCache()
                    .AddSession(options =>
                    {
                        var session = hostContext.Configuration.GetSection(SessionSetting.Session).Get<SessionSetting>();
                        var cookie = hostContext.Configuration.GetSection(CookieSetting.Cookie).Get<CookieSetting>();
                     
                        options.IdleTimeout = TimeSpan.FromMinutes(session.IdleTimeout);
                        options.Cookie.HttpOnly = cookie.HttpOnly;
                        options.Cookie.IsEssential = cookie.IsEssential;
                    })
                    .AddWebApi()
                    .AddAssertor())
                .ConfigureWebHostDefaults(webBuilder =>
                    webBuilder.Configure(app => app
                        .UseMiddleware<ErrorHandlerMiddleware>()
                        .UseRouting()
                        .UseSession()
                        .UseEndpoints(endpoints =>
                        {
                            // endpoints.Controller("auth", (api, act) => act
                            //     .Command(nameof(Register), () => api.Post<Register>())
                            //     .Command(nameof(Login), () => api.Post<Login>())
                            //     .Command(nameof(Refresh), () => api.Post<Refresh>())
                            //     .Command(nameof(Logout), () => api.Post<Logout>())
                            //     .Query(nameof(Authorize), () => api.Get<Authorize, AuthorizeDto>()));
                        })
                    )
                );
    }
}
