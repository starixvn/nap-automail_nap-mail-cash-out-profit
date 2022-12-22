using System;

namespace Fanex.COProfit.Business.Services
{
    public interface DateService
    {
        DateTime StartDate { get; }

        DateTime EndDate { get; }
    }
}