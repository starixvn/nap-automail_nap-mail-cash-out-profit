using Fanex.COProfit.Business.Models;
using Fanex.Data.Repository;
using System;
using System.Collections.Generic;

namespace Fanex.COProfit.Business.Services;

using Criteria;

public interface DataService
{
    Tuple<
        IEnumerable<SummaryModel>,
        IEnumerable<DetailModel>
    > QueryData(int sportType);
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
        var criteria = new COProfitCriteria
        {
            StartDate = date.StartDate,
            EndDate = date.EndDate,
            SportType = sportType
        };

        var data = repository.FetchMultiple<SummaryModel, DetailModel>(criteria);

        return new Tuple<IEnumerable<SummaryModel>, IEnumerable<DetailModel>>(data.FirstResultSet, data.SecondResultSet);
    }
}