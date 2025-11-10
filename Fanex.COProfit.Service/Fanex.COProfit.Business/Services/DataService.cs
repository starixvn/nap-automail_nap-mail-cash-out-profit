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

        var data = repository.FetchMultiple<SummaryModel, DetailModel>(criteria);

        return new Tuple<IEnumerable<SummaryModel>, IEnumerable<DetailModel>>(data.FirstResultSet,
            data.SecondResultSet);
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

        var data = repository.FetchMultiple<BetBuilderSummaryModel, BetBuilderDetailModel>(criteria);
        return new Tuple<IEnumerable<BetBuilderSummaryModel>, IEnumerable<BetBuilderDetailModel>>(data.FirstResultSet,
            data.SecondResultSet);
    }
}