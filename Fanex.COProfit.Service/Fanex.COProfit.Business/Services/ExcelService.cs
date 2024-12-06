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
    public string Export()
    {
        using var excelPackage = new ExcelPackage(new FileInfo("Templates/Template.xlsx"));
        using var sheetMain = excelPackage.Workbook.Worksheets["Report"];
        using var sheetDetail = excelPackage.Workbook.Worksheets["Detail"];
        using var sheetTpl = excelPackage.Workbook.Worksheets["Template"];

        var (dataSummary, dataDetail) = dataService.QueryData();
        var dataResult = refineDataService.Execute(dataSummary, dataDetail);

        RenderMainSheet(sheetTpl, sheetMain, dataResult.Item1.ToList());
        RenderDetailSheet(sheetDetail, dataResult.Item2.ToList());

        var date = dateServiceProvider.DateService().EndDate;
        var fileName = $"Monthly_Basketball_Cash_Out_Profit_Report_For_{date:MMM_yyyy}.xlsx";
        return SaveExcelFile(excelPackage, fileName);
    }

    private void RenderMainSheet(ExcelWorksheet sheetTpl, ExcelWorksheet sheetMain, List<SummaryDTO> data)
    {
        var date = dateServiceProvider.DateService();
        sheetMain.Cells["A2"].Value = $"Monthly Basketball Cash Out Profit Report For {date.EndDate:MMMM yyyy}";
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