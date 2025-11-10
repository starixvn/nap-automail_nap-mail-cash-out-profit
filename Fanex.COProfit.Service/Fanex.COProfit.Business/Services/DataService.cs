using System;
using System.Collections.Generic;
using Fanex.COProfit.Business.Models;
using Fanex.Data.Repository;

namespace Fanex.COProfit.Business.Services;

using Criteria;

public interface DataService
{
    Tuple<
        IEnumerable<SummaryModel>,
        IEnumerable<DetailModel>
    > QueryData(int sportType);

    Tuple<
        IEnumerable<BetBuilderSummaryModel>,
        IEnumerable<BetBuilderDetailModel>
    > QueryBetBuilderData();
}

public class DataServiceImp(
    IDynamicRepository repository,
    DateServiceProvider dateService)
    : DataService
{
    public Tuple<
        IEnumerable<SummaryModel>,
        IEnumerable<DetailModel>
    > QueryData(int sportType)
    {
        var date = dateService.DateService();
        var criteria = new CoProfitCriteria
        {
            StartDate = date.StartDate,
            EndDate = date.EndDate,
            SportType = sportType
        };

        return QueryDataInternal<SummaryModel, DetailModel>(criteria);
    }

    public Tuple<
        IEnumerable<BetBuilderSummaryModel>,
        IEnumerable<BetBuilderDetailModel>> QueryBetBuilderData()
    {
        var date = dateService.DateService();
        var criteria = new BetBuilderProfitCriteria
        {
            StartDate = date.StartDate,
            EndDate = date.EndDate
        };

        return QueryDataInternal<BetBuilderSummaryModel, BetBuilderDetailModel>(criteria);
    }

    private Tuple<IEnumerable<T1>, IEnumerable<T2>> QueryDataInternal<T1, T2>(CriteriaBase criteria)
    {
        var data = repository.FetchMultiple<T1, T2>(criteria);
        return new Tuple<IEnumerable<T1>, IEnumerable<T2>>(data.FirstResultSet, data.SecondResultSet);
    }
}