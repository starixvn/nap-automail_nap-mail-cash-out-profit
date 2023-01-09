using System.Collections.Generic;

namespace Fanex.COProfit.Business.Services
{
    public interface DateServiceProfiver
    {
        DateService DateService();
    }

    public class DateServiceProfiverImp : DateServiceProfiver
    {
        private static Dictionary<bool, DateService> Date
        {
            get
            {
                return new Dictionary<bool, DateService>
                {
                    { true, new DateServiceManual() },
                    { false, new DateServiceMonthly() },
                };
            }
        }

        public DateService DateService()
        {
            return Date[AppConfigService.Config.IsManual];
        }
    }
}