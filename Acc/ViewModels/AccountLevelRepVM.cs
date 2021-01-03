using Acc.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Acc.ViewModels
{
    public class AccountLevelRepVM
    {
        public IEnumerable<AccountLevelDropVM> AccountLevelDropVM { get; set; }

        public IEnumerable<PivotMonthTypeVM> PivotMonthTypeVM { get; set; }
        [Display(Name = "DateType", ResourceType = typeof(Resources.Resource))]
        public int RepMonthType { get; set; }

        [Display(Name = "Level", ResourceType = typeof(Resources.Resource))]
        public int AccountLevelDropVMID { get; set; }

        [Display(Name = "FromDate", ResourceType = typeof(Resources.Resource))]
        public DateTime FromDate { get; set; }

        [Display(Name = "ToDate", ResourceType = typeof(Resources.Resource))]
        public DateTime ToDate { get; set; }

        [Display(Name = "AccountNumber", ResourceType = typeof(Resources.Resource))]
        public string AccNo { get; set; }


        [Display(Name = "AccountName", ResourceType = typeof(Resources.Resource))]
        public string AccName { get; set; }

        public Boolean ShowZeroBalance { get; set; }

        public Boolean Detail { get; set; }

        [Display(Name = "Month", ResourceType = typeof(Resources.Resource))]
        public int Month { get; set; }

        [Display(Name = "ShowEstimatedZero", ResourceType = typeof(Resources.Resource))]
        public bool ShowEstimatedZero { get; set; }


        [Display(Name = "ShowMainAccountValue", ResourceType = typeof(Resources.Resource))]
        public bool ShowMainAccountValue { get; set; }

        [Display(Name = "ShowOnlyaccountswithcostcenter", ResourceType = typeof(Resources.Resource))]
        public bool ShowOnlyaccountswithcostcenter { get; set; }

        [Display(Name = "ByCostCenter", ResourceType = typeof(Resources.Resource))]
        public bool ByCostCenter { get; set; }

        [Display(Name = "CostCenter", ResourceType = typeof(Resources.Resource))]
        public string CostCenterNumber { get; set; }

        [Display(Name = "Name", ResourceType = typeof(Resources.Resource))]
        public string CostCenterName { get; set; }
        public int CostSearchType { get; set; }

        [Display(Name = "Partofthenumber", ResourceType = typeof(Resources.Resource))]
        public bool Partofthenumber { get; set; }


        //--------- For Search------------//
        public string TempCostID { get; set; }
        public int TempCostType { get; set; }

        [Display(Name = "AccountType", ResourceType = typeof(Resources.Resource))]
        public int AccountTypeID { get; set; }

        public IEnumerable<AccountType> AccountType { get; set; }


        [Display(Name = "GOODSSTART", ResourceType = typeof(Resources.Resource))]
        public double GoodsFirst { get; set; }

        [Display(Name = "GOODSEND", ResourceType = typeof(Resources.Resource))]
        public double GoodsEnd { get; set; }

        public double ProfitLostNetTotal { get; set; }
        public bool WorkWithCostCenter { get; set; }

        [Display(Name = "OrderBy", ResourceType = typeof(Resources.Resource))]
        public int OrderBy { get; set; }

        public IEnumerable<CustomerArea> CustomerArea { get; set; }
        public IEnumerable<CustomerCity> CustomerCity { get; set; }

        [Display(Name = "CityName", ResourceType = typeof(Resources.Resource))]
        public int CityID { get; set; }
        public string CityName { get; set; }

        [Display(Name = "AreaName", ResourceType = typeof(Resources.Resource))]
        public int AreaID { get; set; }
        public string AreaName { get; set; }


        public string Email { get; set; }


        public int TheDecimalPointForTheLocalCurrency { get; set; }
        public int TheDecimalPointForTheForeignCurrency { get; set; }


       

    }
}