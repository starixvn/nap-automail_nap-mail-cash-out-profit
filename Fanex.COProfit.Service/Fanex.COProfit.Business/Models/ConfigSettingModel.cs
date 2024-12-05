using System;

namespace Fanex.COProfit.Business.Models;

public class ConfigSettingModel
{
    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public string League { get; set; }

    public bool IsCheckEnoughData { get; set; }

    public bool IsManual { get; set; }

    public string Path { get; set; }

    public string Password { get; set; }
}