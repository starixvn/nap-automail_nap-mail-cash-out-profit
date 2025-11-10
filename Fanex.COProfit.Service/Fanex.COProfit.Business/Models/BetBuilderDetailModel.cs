using System;

namespace Fanex.COProfit.Business.Models;

public class BetBuilderDetailModel
{
    public DateTime RptDate { get; set; }

    public string CurrencyCode { get; set; }

    public int MultipliedBy { get; set; }

    public int BetCount { get; set; }

    public decimal MemberTurnover { get; set; }

    public decimal MemberWinloss { get; set; }

    public decimal JtProfit { get; set; }
}