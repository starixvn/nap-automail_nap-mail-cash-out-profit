namespace Fanex.COProfit.Service.Services;

using System.Diagnostics;
using System.Runtime;
using System.Text;
using System;
using Fanex.COProfit.Business.Services;
using MailDelivery.Client.Jobs;
using MailDelivery.Core.Models;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

public class ExecuteService : JobBase
{
    protected override JobMessage Run(Schedule schedule, IJobExecutionContext jobContext)
    {
        var services = ServiceCollectionExtensions.Services;
        var serviceProvider = services.BuildServiceProvider();

        var mainService = serviceProvider.GetService<MainService>();
        var result = mainService.Execute();

        Gc();

        return new JobMessage
        {
            JobStatus = result.JobStatus,
            Schedule = schedule,
            Mail = result.Mail
        };
    }

    private static void Gc()
    {
        const int megabyte = 1024 * 1024;
        var stringBuilder = new StringBuilder();
        var sw = Stopwatch.StartNew();
        sw.Start();

        using var currentProcess = Process.GetCurrentProcess();
        currentProcess.Refresh();
        var memory = currentProcess.WorkingSet64 / megabyte;

        stringBuilder.Append($"Before GC.Collect: {memory} Mb.\r\n");

        GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce;
        GC.Collect(GC.MaxGeneration, GCCollectionMode.Aggressive, true, true);
        GC.WaitForPendingFinalizers();
        GC.WaitForFullGCComplete();

        currentProcess.Refresh();
        memory = currentProcess.WorkingSet64 / megabyte;

        var duration = sw.ElapsedMilliseconds;
        stringBuilder.Append($"After GC.Collect. Usage memory = {memory}.\r\n");
        stringBuilder.Append($"Duration: {duration} milliseconds.\r\n");

        Console.WriteLine(stringBuilder.ToString());
    }
}