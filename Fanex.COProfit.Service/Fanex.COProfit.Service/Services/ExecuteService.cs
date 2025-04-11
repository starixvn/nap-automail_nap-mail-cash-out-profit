namespace Fanex.COProfit.Service.Services;
using Fanex.COProfit.Business.Services;
using MailDelivery.Client.Jobs;
using MailDelivery.Core.Models;
using Microsoft.Extensions.DependencyInjection;
using Olympus.Diagnostic;
using Quartz;
using System;

public class ExecuteService : JobBase
{
    protected override JobMessage Run(Schedule schedule, IJobExecutionContext jobContext)
    {
        var services = ServiceCollectionExtensions.Services;
        var serviceProvider = services.BuildServiceProvider();

        var mainService = serviceProvider.GetService<MainService>();
        var result = mainService.Execute();

        var gc = GarbageCollector.CollectExtremely();
        Console.WriteLine(gc);

        return new JobMessage
        {
            JobStatus = result.JobStatus,
            Schedule = schedule,
            Mail = result.Mail
        };
    }
}