using System;

namespace Fanex.COProfit.Business.Models
{
    public class AIOModel
    {
        public DateTime Date { get; set; }

        public string ReportDate
        {
            get
            {
                return $"{Date:dd-MMM-yyyy}";
            }
        }

        public string League { get; set; }

        public string Bettype { get; set; }

        public string Status { get; set; }

        public decimal RealTurnover { get; set; }

        public decimal RealIncome { get; set; }

        public decimal Margin
        {
            get
            {
                if (RealTurnover == 0)
                {
                    return 0;
                }

                return RealIncome / RealTurnover;
            }
        }
    }
}