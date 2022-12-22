using System;

namespace Fanex.COProfit.Business.Models
{
    public class DetailDTO
    {
        public DateTime CashOutDate { get; set; }

        public string CurrencyCode { get; set; }

        public int BetCount { get; set; }

        public decimal COTurnover { get; set; }

        public decimal AssumedPayout { get; set; }

        public decimal BBAmount { get; set; }

        public decimal COProfit
        {
            get
            {
                return AssumedPayout - BBAmount;
            }
        }
    }
}