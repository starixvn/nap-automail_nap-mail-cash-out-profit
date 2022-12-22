using Fanex.MailDelivery.Client.Jobs;
using Fanex.MailDelivery.Core.Models;
using Fanex.COProfit.Business.Services;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace Fanex.COProfit.Service
{
    public class ExecuteService : JobBase
    {
        protected override JobMessage Run(Schedule schedule, IJobExecutionContext jobContext)
        {
            var services = ServiceCollectionExtensions.Services;
            var serviceProvider = services.BuildServiceProvider();

            var mainService = serviceProvider.GetService<MainService>();
            var result = mainService.Execute();
            return new JobMessage
            {
                JobStatus = result.JobStatus,
                Schedule = schedule,
                Mail = result.Mail
            };
        }
    }
}