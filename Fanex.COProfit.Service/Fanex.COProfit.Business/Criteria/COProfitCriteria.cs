namespace Fanex.COProfit.Business.Criteria;

using Data.Repository;
using System;

public class COProfitCriteria : CriteriaBase
{
    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public int SportType { get; set; }

    public override string GetSettingKey() => "Acc_Rpt_Automail_CashOutProfit";

    public override bool IsValid() => true;
}