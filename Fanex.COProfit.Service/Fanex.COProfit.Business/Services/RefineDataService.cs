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
            CurrencyCode = FormatCurrencyCode(summaryModel.MultipliedBy, summaryModel.CurrencyCode),
            BetCount = summaryModel.BetCount,
            COTurnover = summaryModel.COTurnover,
            AssumedPayout = summaryModel.AssumedPayout,
            BBAmount = summaryModel.BBAmount
        })
            .ToList();

        return AddGrandTotalAndOrder(result, 
            orderResult => new SummaryDTO
            {
                CurrencyCode = "Grand Total",
                BetCount = orderResult.Sum(s => s.BetCount),
                COTurnover = orderResult.Sum(s => s.COTurnover),
                AssumedPayout = orderResult.Sum(s => s.AssumedPayout),
                BBAmount = orderResult.Sum(s => s.BBAmount)
            });
    }

    private static IEnumerable<DetailDTO> ProcessDetailData(IEnumerable<DetailModel> detail)
    {
        var result = detail.Select(detailModel => new DetailDTO
        {
            CashOutDate = detailModel.CashOutDate,
            CurrencyCode = FormatCurrencyCode(detailModel.MultipliedBy, detailModel.CurrencyCode),
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
            CurrencyCode = FormatCurrencyCode(summaryModel.MultipliedBy, summaryModel.CurrencyCode),
            BetCount = summaryModel.BetCount,
            MemberTurnover = summaryModel.MemberTurnover,
            MemberWinloss = summaryModel.MemberWinloss,
            JtProfit = summaryModel.JtProfit
        })
            .ToList();

        return AddGrandTotalAndOrder(result,
            orderResult => new BetBuilderSummaryDTO
            {
                CurrencyCode = "Grand Total",
                BetCount = orderResult.Sum(s => s.BetCount),
                MemberTurnover = orderResult.Sum(s => s.MemberTurnover),
                MemberWinloss = orderResult.Sum(s => s.MemberWinloss),
                JtProfit = orderResult.Sum(s => s.JtProfit)
            });
    }

    private static IEnumerable<BetBuilderDetailDTO> ProcessDetailBetBuilderData(
        IEnumerable<BetBuilderDetailModel> detail)
    {
        var result = detail.Select(detailModel => new BetBuilderDetailDTO
        {
            RptDate = detailModel.RptDate,
            CurrencyCode = FormatCurrencyCode(detailModel.MultipliedBy, detailModel.CurrencyCode),
            BetCount = detailModel.BetCount,
            MemberTurnover = detailModel.MemberTurnover,
            MemberWinloss = detailModel.MemberWinloss,
            JtProfit = detailModel.JtProfit
        })
            .ToList();

        return result.OrderBy(s => s.CurrencyCode).ThenBy(s => s.RptDate);
    }

    private static string FormatCurrencyCode(int multipliedBy, string currencyCode)
    {
        return multipliedBy > 1 ? $"{multipliedBy}_{currencyCode}" : currencyCode;
    }

    private static IEnumerable<T> AddGrandTotalAndOrder<T>(List<T> result, Func<List<T>, T> createTotal)
    {
        var orderResult = result.OrderBy(s => ((dynamic)s).CurrencyCode).ToList();
        var total = createTotal(orderResult);
        orderResult.Add(total);
        return orderResult;
    }
}