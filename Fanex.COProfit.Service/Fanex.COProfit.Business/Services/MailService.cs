using Fanex.MailDelivery.Core.Models;
using RazorEngineCore;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;

namespace Fanex.COProfit.Business.Services;

public interface MailService
{
    Mail CreateMail(string fileName);
}

public class MailServiceImp : MailService
{
    private static readonly ConcurrentDictionary<int, IRazorEngineCompiledTemplate> CompiledTemplateCache = new();

    public Mail CreateMail(string fileName)
    {
        var attachments = new List<MailAttachment>
        {
            new(fileName, FileExtension.MS_EXCEL)
        };
        var shortName = new FileInfo(fileName).Name;

        var mail = new Mail
        {
            Title = "Auto Generated Report Administrator",
            Subject = $"Automail Report - Monthly Basketball Cash Out Profit Report - {DateTime.Today:yyyy/MM/dd}",
            Body = LoadBody(shortName),
            Attachments = attachments
        };

        return mail;
    }

    private static string LoadBody(string fileName)
    {
        const string templatePath = "Templates/EmailTemplate.cshtml";
        var hashCode = templatePath.GetHashCode();
        var compiledTemplate = CompiledTemplateCache.GetOrAdd(hashCode, _ =>
        {
            var template = File.ReadAllText(templatePath);
            var razorEngine = new RazorEngine();
            return razorEngine.Compile(template);
        });

        var tuple = new Tuple<string, DateTime>(fileName, DateTime.Today);
        return compiledTemplate.Run(tuple);
    }
}