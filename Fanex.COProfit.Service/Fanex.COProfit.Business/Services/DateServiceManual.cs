using System;

namespace Fanex.COProfit.Business.Services;

public class DateServiceManual : DateService
{
    public DateTime StartDate => AppConfigService.Config.StartDate;

    public DateTime EndDate => AppConfigService.Config.EndDate;
}