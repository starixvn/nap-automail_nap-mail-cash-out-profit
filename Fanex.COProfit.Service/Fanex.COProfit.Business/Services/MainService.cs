using Fanex.MailDelivery.Core.Models;
using Fanex.COProfit.Business.Models;
using System;

namespace Fanex.COProfit.Business.Services;

public interface MainService
{
    JobMessage Execute();
}

public class MainServiceImp(
    Log log,
    MailService mailService,
    ExcelService excelService)
    : MainService
{
    public JobMessage Execute()
    {
        var result = new ExecuteResultModel();
        var jobMessage = new JobMessage();
        try
        {
            result = SendNotification();
        }
        catch (Exception e)
        {
            result.Success = false;
            result.Message = e.Message;
            log.Log(e);
        }
        finally
        {
            jobMessage.Mail = result.Mail;
            jobMessage.JobStatus = result.Success ? JobStatus.Success : JobStatus.DataIsNotReady;
            jobMessage.TaskLog = new TaskLog
            {
                LogTime = DateTime.Now,
                Message = result.Message
            };
        }

        return jobMessage;
    }

    private ExecuteResultModel SendNotification()
    {
        var excelFile = excelService.Export();
        var mail = mailService.CreateMail(excelFile);
        return new ExecuteResultModel
        {
            Mail = mail,
            Success = true,
            Message = string.Empty
        };
    }
}