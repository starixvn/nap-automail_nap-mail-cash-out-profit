using System;

namespace Fanex.COProfit.Business.Services
{
    public class DateServiceMonthly : DateService
    {
        public DateTime StartDate
        {
            get
            {
                var date = DateTime.Now.AddDays(-1);
                if (date.Day == 31 && date.Month == 12)
                {
                    return Convert.ToDateTime($"{date:yyyy-MM-01}");
                }

                return Convert.ToDateTime($"{date.AddMonths(-1):yyyy-MM-01}");
            }
        }

        public DateTime EndDate
        {
            get
            {
                var date = DateTime.Now.AddDays(-1);
                if (date.Day == 31 && date.Month == 12)
                {
                    return Convert.ToDateTime($"{date:yyyy-MM-dd}");
                }

                date = Convert.ToDateTime($"{date:yyyy-MM-dd}");
                var numOfDays = date.Day;
                return date.AddDays(-numOfDays);
            }
        }
    }
}