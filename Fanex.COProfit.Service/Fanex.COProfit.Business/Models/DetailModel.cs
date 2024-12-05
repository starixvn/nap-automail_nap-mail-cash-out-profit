using System;

namespace Fanex.COProfit.Business.Models;

public class DetailModel
{
    public DateTime CashOutDate { get; set; }

    public string CurrencyCode { get; set; }

    public int MultipliedBy { get; set; }

    public int BetCount { get; set; }

    public decimal COTurnover { get; set; }

    public decimal AssumedPayout { get; set; }

    public decimal BBAmount { get; set; }
}