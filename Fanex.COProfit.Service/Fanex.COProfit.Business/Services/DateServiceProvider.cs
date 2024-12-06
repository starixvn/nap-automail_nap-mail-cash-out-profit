using System.Collections.Generic;

namespace Fanex.COProfit.Business.Services;

public interface DateServiceProvider
{
    DateService DateService();
}

public class DateServiceProviderImp : DateServiceProvider
{
    private static Dictionary<bool, DateService> Date =>
        new()
        {
            { true, new DateServiceManual() },
            { false, new DateServiceMonthly() }
        };

    public DateService DateService()
    {
        return Date[AppConfigService.Config.IsManual];
    }
}