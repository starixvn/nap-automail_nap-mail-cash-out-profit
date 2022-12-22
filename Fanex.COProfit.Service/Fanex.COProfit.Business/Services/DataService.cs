using Fanex.COProfit.Business.Criterias;
using Fanex.COProfit.Business.Models;
using Fanex.Data.Repository;
using System;
using System.Collections.Generic;

namespace Fanex.COProfit.Business.Services
{
    public interface DataService
    {
        Tuple<
            IEnumerable<SummaryModel>,
            IEnumerable<DetailModel>
            > QueryData();
    }

    public class DataServiceImp : DataService
    {
        private readonly IDynamicRepository repository;
        private readonly DateServiceProfiver dateService;

        public DataServiceImp(
            IDynamicRepository repository,
            DateServiceProfiver dateService)
        {
            this.repository = repository;
            this.dateService = dateService;
        }

        public Tuple<
            IEnumerable<SummaryModel>,
            IEnumerable<DetailModel>
            > QueryData()
        {
            var date = dateService.DateService();
            var criteria = new COProfitCriteria
            {
                StartDate = date.StartDate,
                EndDate = date.EndDate,
            };

            var data = repository.FetchMultiple<SummaryModel, DetailModel>(criteria);
            return data;
        }
    }
}