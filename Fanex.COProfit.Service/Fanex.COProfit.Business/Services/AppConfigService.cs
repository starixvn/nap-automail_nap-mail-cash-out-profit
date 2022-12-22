using Fanex.COProfit.Business.Models;

namespace Fanex.COProfit.Business.Services
{
    public static class AppConfigService
    {
        public static ConfigSettingModel Config { get; set; }

        public static bool IsProcessing { get; set; }
    }
}