using Fanex.COProfit.Business.Models;
using Fanex.COProfit.Business.Services;
using Fanex.COProfitByTO.Service;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.IO;

namespace Fanex.COProfit.Service
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
            LoadConfiguration();

            return Host.CreateDefaultBuilder(args)
                   .ConfigureWebHostDefaults(webBuilder =>
                   {
                       webBuilder.UseUrls($"http://0.0.0.0:5000");
                       webBuilder.UseStartup<Startup>();
                   });
        }

        private static void LoadConfiguration()
        {
            var text = File.ReadAllText("appsettings.json");
            AppConfigService.Config = JsonConvert.DeserializeObject<ConfigSettingModel>(text);
        }
    }
}