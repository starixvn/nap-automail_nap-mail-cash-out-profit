namespace Fanex.COProfit.Business.Models;

public class BetBuilderSummaryDTO
{
    public string CurrencyCode { get; set; }

    public int BetCount { get; set; }

    public decimal MemberTurnover { get; set; }

    public decimal MemberWinloss { get; set; }

    public decimal JtProfit { get; set; }
}