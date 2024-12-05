using System;

namespace Fanex.COProfit.Business.Services;

public class DateServiceMonthly : DateService
{
    public DateTime StartDate
    {
        get
        {
            var date = DateTime.Now.AddDays(-1);
            return Convert.ToDateTime(date is { Day: 31, Month: 12 } 
                ? $"{date:yyyy-MM-01}" 
                : $"{date.AddMonths(-1):yyyy-MM-01}");
        }
    }

    public DateTime EndDate
    {
        get
        {
            var date = DateTime.Now.AddDays(-1);
            if (date is { Day: 31, Month: 12 })
            {
                return Convert.ToDateTime($"{date:yyyy-MM-dd}");
            }

            date = Convert.ToDateTime($"{date:yyyy-MM-dd}");
            var numOfDays = date.Day;
            return date.AddDays(-numOfDays);
        }
    }
}