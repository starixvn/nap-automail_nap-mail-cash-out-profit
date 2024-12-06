using Fanex.COProfit.Business.Models;
using Fanex.COProfit.Business.Services;
using Fanex.Data.Repository;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using OfficeOpenXml;

namespace Fanex.COProfit.Service;

using System.IO;
using Services;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection Services { get; set; }

    public static void AddConfiguration(this IServiceCollection services)
    {
        Services = services;
        var text = File.ReadAllText("appsettings.json");
        AppConfigService.Config = JsonConvert.DeserializeObject<ConfigSettingModel>(text);
    }

    public static void AddInternalServices(this IServiceCollection services)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        services.AddSingleton<Log, LogImp>();
        services.AddSingleton<IDynamicRepository, DynamicRepository>();
        services.AddSingleton<DataService, DataServiceImp>();
        services.AddSingleton<RefineDataService, RefineDataServiceImp>();
        services.AddSingleton<MainService, MainServiceImp>();
        services.AddSingleton<ExcelService, ExcelServiceImp>();
        services.AddSingleton<DateServiceProvider, DateServiceProviderImp>();
        services.AddSingleton<MailService, MailServiceImp>();
        services.AddSingleton<MainService, MainServiceImp>();
        services.AddSingleton<ExecuteService, ExecuteService>();
    }
}