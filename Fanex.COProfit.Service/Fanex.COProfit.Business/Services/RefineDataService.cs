using Fanex.COProfit.Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fanex.COProfit.Business.Services;

public interface RefineDataService
{
    Tuple<IEnumerable<SummaryDTO>, IEnumerable<DetailDTO>> Execute(IEnumerable<SummaryModel> summary,
        IEnumerable<DetailModel> detail);

    Tuple<IEnumerable<BetBuilderSummaryDTO>, IEnumerable<BetBuilderDetailDTO>> ExecuteBetBuilder(
        IEnumerable<BetBuilderSummaryModel> summary,
        IEnumerable<BetBuilderDetailModel> detail);
}

public class RefineDataServiceImp : RefineDataService
{
    public Tuple<IEnumerable<SummaryDTO>, IEnumerable<DetailDTO>> Execute(IEnumerable<SummaryModel> summary,
        IEnumerable<DetailModel> detail)
    {
        var summaryData = ProcessSummaryData(summary);
        var detailData = ProcessDetailData(detail);

        return new Tuple<IEnumerable<SummaryDTO>, IEnumerable<DetailDTO>>(summaryData, detailData);
    }

    public Tuple<IEnumerable<BetBuilderSummaryDTO>, IEnumerable<BetBuilderDetailDTO>> ExecuteBetBuilder(
        IEnumerable<BetBuilderSummaryModel> summary, IEnumerable<BetBuilderDetailModel> detail)
    {
        var summaryData = ProcessSummaryBetBuilderData(summary);
        var detailData = ProcessDetailBetBuilderData(detail);

        return new Tuple<IEnumerable<BetBuilderSummaryDTO>, IEnumerable<BetBuilderDetailDTO>>(summaryData, detailData);
    }

    private static IEnumerable<SummaryDTO> ProcessSummaryData(IEnumerable<SummaryModel> summary)
    {
        var result = summary.Select(summaryModel => new SummaryDTO
        {
            CurrencyCode = summaryModel.MultipliedBy > 1
                    ? $"{summaryModel.MultipliedBy}_{summaryModel.CurrencyCode}"
                    : summaryModel.CurrencyCode,
            BetCount = summaryModel.BetCount,
            COTurnover = summaryModel.COTurnover,
            AssumedPayout = summaryModel.AssumedPayout,
            BBAmount = summaryModel.BBAmount
        })
            .ToList();

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
        var result = detail.Select(detailModel => new DetailDTO
        {
            CashOutDate = detailModel.CashOutDate,
            CurrencyCode = detailModel.MultipliedBy > 1
                    ? $"{detailModel.MultipliedBy}_{detailModel.CurrencyCode}"
                    : detailModel.CurrencyCode,
            BetCount = detailModel.BetCount,
            COTurnover = detailModel.COTurnover,
            AssumedPayout = detailModel.AssumedPayout,
            BBAmount = detailModel.BBAmount
        })
            .ToList();

        return result.OrderBy(s => s.CurrencyCode).ThenBy(s => s.CashOutDate);
    }

    private static IEnumerable<BetBuilderSummaryDTO> ProcessSummaryBetBuilderData(
        IEnumerable<BetBuilderSummaryModel> summary)
    {
        var result = summary.Select(summaryModel => new BetBuilderSummaryDTO
        {
            CurrencyCode = summaryModel.MultipliedBy > 1
                    ? $"{summaryModel.MultipliedBy}_{summaryModel.CurrencyCode}"
                    : summaryModel.CurrencyCode,
            BetCount = summaryModel.BetCount,
            MemberTurnover = summaryModel.MemberTurnover,
            MemberWinloss = summaryModel.MemberWinloss,
            JtProfit = summaryModel.JtProfit
        })
            .ToList();

        var orderResult = result.OrderBy(s => s.CurrencyCode).ToList();
        var total = new BetBuilderSummaryDTO
        {
            CurrencyCode = "Grand Total",
            BetCount = orderResult.Sum(s => s.BetCount),
            MemberTurnover = orderResult.Sum(s => s.MemberTurnover),
            MemberWinloss = orderResult.Sum(s => s.MemberWinloss),
            JtProfit = orderResult.Sum(s => s.JtProfit)
        };

        orderResult.Add(total);
        return orderResult;
    }

    private static IEnumerable<BetBuilderDetailDTO> ProcessDetailBetBuilderData(
        IEnumerable<BetBuilderDetailModel> detail)
    {
        var result = detail.Select(detailModel => new BetBuilderDetailDTO
        {
            RptDate = detailModel.RptDate,
            CurrencyCode = detailModel.MultipliedBy > 1
                    ? $"{detailModel.MultipliedBy}_{detailModel.CurrencyCode}"
                    : detailModel.CurrencyCode,
            BetCount = detailModel.BetCount,
            MemberTurnover = detailModel.MemberTurnover,
            MemberWinloss = detailModel.MemberWinloss,
            JtProfit = detailModel.JtProfit
        })
            .ToList();

        return result.OrderBy(s => s.CurrencyCode).ThenBy(s => s.RptDate);
    }
}