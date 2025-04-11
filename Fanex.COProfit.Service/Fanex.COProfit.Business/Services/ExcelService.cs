using Fanex.COProfit.Business.Models;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Fanex.COProfit.Business.Services;

public interface ExcelService
{
    string Export();
}

public class ExcelServiceImp(
    DataService dataService,
    RefineDataService refineDataService,
    DateServiceProvider dateServiceProvider)
    : ExcelService
{
    private static readonly Dictionary<int, string> Sports = new()
    {
        { 2, "Basketball" },
        { 1, "Soccer" },
    };

    public string Export()
    {
        using var excelPackage = CreateExcelPackage();
        var templates = LoadTemplates(excelPackage);

        foreach (var sport in Sports)
        {
            ProcessSportData(excelPackage, templates, sport);
        }

        return SaveExcelFile(excelPackage, GenerateFileName());
    }

    private static ExcelPackage CreateExcelPackage()
    {
        return new ExcelPackage(new FileInfo("Templates/Template.xlsx"));
    }

    private static (ExcelWorksheet Summary, ExcelWorksheet Detail, ExcelWorksheet Template) LoadTemplates(
        ExcelPackage excelPackage)
    {
        return (
            excelPackage.Workbook.Worksheets["Summary"],
            excelPackage.Workbook.Worksheets["Detail"],
            excelPackage.Workbook.Worksheets["Template"]
        );
    }

    private void ProcessSportData(ExcelPackage excelPackage,
        (ExcelWorksheet Summary, ExcelWorksheet Detail, ExcelWorksheet Template) templates,
        KeyValuePair<int, string> sport)
    {
        var (sheetMain, sheetDetail) = CreateSportSheets(excelPackage, templates, sport.Value);
        ConfigureSheetFormats(sheetMain, templates.Summary, sheetDetail, templates.Detail);

        var (dataSummary, dataDetail) = dataService.QueryData(sport.Key);
        var dataResult = refineDataService.Execute(dataSummary, dataDetail);

        RenderMainSheet(templates.Template, sheetMain, dataResult.Item1.ToList(), sport.Value);
        RenderDetailSheet(sheetDetail, dataResult.Item2.ToList());
    }

    private static (ExcelWorksheet Main, ExcelWorksheet Detail) CreateSportSheets(
        ExcelPackage excelPackage,
        (ExcelWorksheet Summary, ExcelWorksheet Detail, ExcelWorksheet Template) templates,
        string sportName)
    {
        var sheetMain = excelPackage.Workbook.Worksheets.Add($"{sportName} Summary");
        var sheetDetail = excelPackage.Workbook.Worksheets.Add($"{sportName} Detail");

        templates.Summary.Cells["A1:F10"].Copy(sheetMain.Cells["A1:F10"]);
        templates.Detail.Cells["A1:G3"].Copy(sheetDetail.Cells["A1:G3"]);

        return (sheetMain, sheetDetail);
    }

    private static void ConfigureSheetFormats(
        ExcelWorksheet sheetMain,
        ExcelWorksheet sheetMainTpl,
        ExcelWorksheet sheetDetail,
        ExcelWorksheet sheetDetailTpl)
    {
        ConfigureMainSheetFormat(sheetMain, sheetMainTpl);
        ConfigureDetailSheetFormat(sheetDetail, sheetDetailTpl);
    }

    private static void ConfigureMainSheetFormat(ExcelWorksheet sheetMain, ExcelWorksheet sheetMainTpl)
    {
        sheetMain.View.FreezePanes(10, 1);
        sheetMain.View.ShowGridLines = false;
        sheetMain.Row(9).Height = sheetMainTpl.Row(9).Height;

        for (var col = 1; col <= 6; col++)
        {
            sheetMain.Column(col).Width = sheetMainTpl.Column(col).Width;
        }
    }

    private static void ConfigureDetailSheetFormat(ExcelWorksheet sheetDetail, ExcelWorksheet sheetDetailTpl)
    {
        sheetDetail.View.FreezePanes(3, 1);
        sheetDetail.View.ShowGridLines = false;
        sheetDetail.Row(2).Height = sheetDetailTpl.Row(2).Height;

        for (var col = 1; col <= 7; col++)
        {
            sheetDetail.Column(col).Width = sheetDetailTpl.Column(col).Width;
        }
    }

    private string GenerateFileName()
    {
        var date = dateServiceProvider.DateService().EndDate;
        return $"Monthly_JackTime_Cash_Out_Profit_Report_For_{date:MMM_yyyy}.xlsx";
    }

    private void RenderMainSheet(ExcelWorksheet sheetTpl, ExcelWorksheet sheetMain, List<SummaryDTO> data,
        string sportName)
    {
        var date = dateServiceProvider.DateService();
        sheetMain.Cells["A2"].Value = $"Monthly {sportName} Cash Out Profit Report For {date.EndDate:MMMM yyyy}";
        sheetMain.Cells["A3"].Value = $"Generated on {DateTime.Now}";

        sheetMain.Cells["A10"].LoadFromCollection(data, false);
        const int startRow = 10;
        var endRow = data.Count + startRow - 1;
        const int startCol = 1;
        const int endCol = 6;
        const int tplRow = 2;
        const double height = 19.5d;
        for (var col = startCol; col <= endCol; col++)
        {
            for (var row = startRow; row <= endRow; row++)
            {
                sheetMain.Cells[row, col].StyleID = sheetMain.Cells[startRow, col].StyleID;
                if (row == endRow)
                {
                    sheetMain.Cells[row, col].StyleID = sheetTpl.Cells[tplRow, col].StyleID;
                }

                sheetMain.Row(row).Height = height;
            }
        }
    }

    private static void RenderDetailSheet(ExcelWorksheet sheetDetail, List<DetailDTO> data)
    {
        sheetDetail.Cells["A3"].LoadFromCollection(data, false);
        const int startRow = 3;
        var endRow = data.Count + startRow - 1;
        const int startCol = 1;
        const int endCol = 7;
        const double height = 19.5d;
        for (var col = startCol; col <= endCol; col++)
        {
            for (var row = startRow; row <= endRow; row++)
            {
                sheetDetail.Cells[row, col].StyleID = sheetDetail.Cells[startRow, col].StyleID;
                sheetDetail.Row(row).Height = height;
            }
        }
    }

    private static string SaveExcelFile(ExcelPackage excelPackage, string fileName)
    {
        excelPackage.Workbook.Worksheets.Delete("Template");
        excelPackage.Workbook.Worksheets.Delete("Summary");
        excelPackage.Workbook.Worksheets.Delete("Detail");
        var date = DateTime.Now;
        var name = $"{fileName}";
        var path = $"{AppConfigService.Config.Path}/{date:yyyyMMdd}";
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        path = $"{path}/{name}";
        excelPackage.SaveAs(new FileInfo(path), AppConfigService.Config.Password);

        return path;
    }
}