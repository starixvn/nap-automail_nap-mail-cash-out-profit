using Fanex.COProfit.Business.Services;
using Fanex.Data;
using Fanex.Data.Repository;
using Fanex.Logging;
using Fanex.Logging.Sentry.RavenClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OfficeOpenXml;
using SharpRaven;
using System.Collections.Generic;
using System.Linq;

namespace Fanex.COProfit.Service
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection Services { get; set; }

        public static void SentryRegister(this IServiceCollection services)
        {
            var sentry = new SentryLogging(new SentryEngineOptions
            {
                Dsn = new Dsn(AppConfigService.Config.SentryClientKey)
            });

            LogManager
               .SetDefaultLogCategory(AppConfigService.Config.CategorySentryLog)
               .Use(new List<ILogEngine>
               {
                    sentry
               });
        }

        public static void FanexDataRegister(this IServiceCollection services, IConfiguration Configuration)
        {
            var connections = Configuration.GetSection("ConnectionStrings")
                .GetChildren()
                .AsEnumerable()
                .ToDictionary
                (
                     x => x.Key,
                     y => y.Value
                );

            DbSettingProviderManager
             .StartNewSession()
             .UseConnectionStrings(connections)
             .UseDefaultDbSettingProvider(Configuration.GetSection("StoredProcedurePath").Value)
             .Run();
        }

        public static void InternalServiceRegister(this IServiceCollection services)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            services.AddSingleton<Log, LogImp>();
            services.AddSingleton<IDynamicRepository, DynamicRepository>();
            services.AddSingleton<DataService, DataServiceImp>();
            services.AddSingleton<RefineDataService, RefineDataServiceImp>();
            services.AddSingleton<MainService, MainServiceImp>();
            services.AddSingleton<ExcelService, ExcelServiceImp>();
            services.AddSingleton<DateServiceProfiver, DateServiceProfiverImp>();
            services.AddSingleton<MailService, MailServiceImp>();

            Services = services;
        }
    }
}