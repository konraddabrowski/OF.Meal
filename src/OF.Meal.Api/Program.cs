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
using OF.Meal.Application.Commands;
using OF.Meal.Application.Queries;
using OF.Meal.Core.Entities;
using OF.Meal.Infrastructure.Repositories;
using OF.Meal.Core.Repositories;
using System.Data;
using OF.Meal.Infrastructure.Services;
using Arcaim.CQRS.WebApi.Interfaces;

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
                    .AddMyDapper(serviceProvider => {
                        var connection = serviceProvider
                            .GetRequiredService<IAppSettings>()
                            .ConnectionStrings
                            .DefaultConnection;
                        
                        return new MySqlConnection(connection);
                    })
                    .AddScoped<IMealRepository, MealRepository>()
                    .AddDistributedMemoryCache()
                    .AddSession(options =>
                    {
                        var session = hostContext.Configuration
                            .GetSection(SessionSetting.Session)
                            .Get<SessionSetting>();
                     
                        options.IdleTimeout = TimeSpan.FromSeconds(session.IdleTimeout);
                    })
                    .AddScoped<IAuthorization, Authorization>()
                    .AddWebApi()
                    .AddAssertor())
                .ConfigureWebHostDefaults(webBuilder =>
                    webBuilder.Configure(app => app
                        .UseMiddleware<ErrorHandlerMiddleware>()
                        .UseRouting()
                        .UseSession()
                        .UseEndpoints(endpoints =>
                        {
                            endpoints.Controller("meal", (api, act) => act
                                .Command(() => api.Post<AddMeal>())
                                // .Command(nameof(AddMeal), () => api.Post<AddMeal>())
                                .Command(nameof(UpdateMeal), () => api.Put<UpdateMeal>())
                                .Command(nameof(DeleteMeal), () => api.Delete<DeleteMeal>())
                                .Query(nameof(GetMeal), () => api.Get<GetMeal, OF.Meal.Core.Entities.Meal>())
                                .Query(nameof(GetMeals), () => api.Get<GetMeals, Meals>()));
                        })
                    )
                );
    }
}
