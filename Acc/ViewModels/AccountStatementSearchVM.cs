using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.AccessControl;
using System.Web;

namespace Acc.ViewModels
{
    public class AccountStatementSearchVM
    {


        [Display(Name = "FromAccount", ResourceType = typeof(Resources.Resource))]
        public string FromAccAccount { get; set; }

        [Display(Name = "Name", ResourceType = typeof(Resources.Resource))]
        public string FromAccName { get; set; }


        [Display(Name = "ToAccount", ResourceType = typeof(Resources.Resource))]
        public string ToAccAccount { get; set; }

        [Display(Name = "Name", ResourceType = typeof(Resources.Resource))]
        public string ToAccName { get; set; }


        [Display(Name = "FromDate", ResourceType = typeof(Resources.Resource))]
        public DateTime FromDate { get; set; }

        [Display(Name = "ToDate", ResourceType = typeof(Resources.Resource))]
        public DateTime ToDate { get; set; }

        [Display(Name = "ByCostCenter", ResourceType = typeof(Resources.Resource))]
        public bool ByCostCenter { get; set; }

        [Display(Name = "CostCenter", ResourceType = typeof(Resources.Resource))]
        public string CostCenterNumber { get; set; }

        [Display(Name = "Name", ResourceType = typeof(Resources.Resource))]
        public string CostCenterName { get; set; }

        public int CostSearchType { get; set; }

        [Display(Name = "Partofthenumber", ResourceType = typeof(Resources.Resource))]
        public bool Partofthenumber { get; set; }
        public bool WorkWithCostCenter { get; set; }
        public string sTotalDebit { get; set; }
        public string sTotalCredit { get; set; }
        public string sNetTotal { get; set; }
        public string sTotalCustomerCheque { get; set; }
        public string sTotalSupplierCheque { get; set; }
        public string sUnpaidBill { get; set; }

        public string Email { get; set; }
        public int TheDecimalPointForTheLocalCurrency { get; set; }
        public int TheDecimalPointForTheForeignCurrency { get; set; }

    }
}