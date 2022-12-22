using Fanex.COProfit.Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fanex.COProfit.Business.Services
{
    public interface RefineDataService
    {
        Tuple<IEnumerable<SummaryDTO>, IEnumerable<DetailDTO>> Execute(IEnumerable<SummaryModel> summary, IEnumerable<DetailModel> detail);
    }

    public class RefineDataServiceImp : RefineDataService
    {
        public Tuple<IEnumerable<SummaryDTO>, IEnumerable<DetailDTO>> Execute(IEnumerable<SummaryModel> summary, IEnumerable<DetailModel> detail)
        {
            var summaryData = ProcessSummaryData(summary);
            var detailData = ProcessDetailData(detail);

            return new Tuple<IEnumerable<SummaryDTO>, IEnumerable<DetailDTO>>(summaryData, detailData);
        }

        private static IEnumerable<SummaryDTO> ProcessSummaryData(IEnumerable<SummaryModel> summary)
        {
            var result = new List<SummaryDTO>();

            foreach (var summaryModel in summary)
            {
                var row = new SummaryDTO
                {
                    CurrencyCode = summaryModel.MultipliedBy > 1 ? $"{summaryModel.MultipliedBy}_{summaryModel.CurrencyCode}" : summaryModel.CurrencyCode,
                    BetCount = summaryModel.BetCount,
                    COTurnover = summaryModel.COTurnover,
                    AssumedPayout = summaryModel.AssumedPayout,
                    BBAmount = summaryModel.BBAmount
                };

                result.Add(row);
            }

            var orderResult = result.OrderBy(s => s.CurrencyCode).ToList();
            var total = new SummaryDTO
            {
                CurrencyCode = "Grand Total",
                BetCount = orderResult.Sum(s => s.BetCount),
                COTurnover = orderResult.Sum(s => s.COTurnover),
                AssumedPayout = orderResult.Sum(s => s.AssumedPayout),
                BBAmount = orderResult.Sum(s => s.BBAmount)
            };

            orderResult.Add(total);
            return orderResult;
        }

        private static IEnumerable<DetailDTO> ProcessDetailData(IEnumerable<DetailModel> detail)
        {
            var result = new List<DetailDTO>();
            foreach (var detailModel in detail)
            {
                var row = new DetailDTO
                {
                    CashOutDate = detailModel.CashOutDate,
                    CurrencyCode = detailModel.MultipliedBy > 1 ? $"{detailModel.MultipliedBy}_{detailModel.CurrencyCode}" : detailModel.CurrencyCode,
                    BetCount = detailModel.BetCount,
                    COTurnover = detailModel.COTurnover,
                    AssumedPayout = detailModel.AssumedPayout,
                    BBAmount = detailModel.BBAmount
                };

                result.Add(row);
            }

            return result.OrderBy(s => s.CurrencyCode).ThenBy(s => s.CashOutDate);
        }
    }
}