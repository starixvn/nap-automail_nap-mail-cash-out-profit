namespace Fanex.COProfit.Business.Criteria;

using Data.Repository;
using System;

public class BetBuilderProfitCriteria : CriteriaBase
{
    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public override string GetSettingKey() => "Acc_Rpt_Automail_BetBuilderProfit";

    public override bool IsValid() => true;
}