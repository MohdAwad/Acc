using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Acc.ViewModels
{
    public class TransReportVM
    {
        [Display(Name = "ByAcc", ResourceType = typeof(Resources.Resource))]
        public bool ByAcc { get; set; }

        [Display(Name = "AccountNumber", ResourceType = typeof(Resources.Resource))]
        public string AccountNumber { get; set; }

        [Display(Name = "AccountName", ResourceType = typeof(Resources.Resource))]
        public string AccountName { get; set; }

        [Display(Name = "ByDate", ResourceType = typeof(Resources.Resource))]
        public bool  ByDate { get; set; }

        [Display(Name = "FromDate", ResourceType = typeof(Resources.Resource))]
        public DateTime FromDate { get; set; }

        [Display(Name = "ToDate", ResourceType = typeof(Resources.Resource))]
        public DateTime ToDate { get; set; }

        [Display(Name = "DebitCredit", ResourceType = typeof(Resources.Resource))]
        public int DebitCredit { get; set; }


        [Display(Name = "ByValue", ResourceType = typeof(Resources.Resource))]
        public bool ByValue { get; set; }

        [Display(Name = "FromValue", ResourceType = typeof(Resources.Resource))]
        public double FromValue { get; set; }

        [Display(Name = "ToValue", ResourceType = typeof(Resources.Resource))]
        public double ToValue { get; set; }

        [Display(Name = "ByNote", ResourceType = typeof(Resources.Resource))]
        public bool ByNote { get; set; }

        [Display(Name = "Note", ResourceType = typeof(Resources.Resource))]
        public string Note { get; set; }


        [Display(Name = "Note", ResourceType = typeof(Resources.Resource))]
        public string TransName { get; set; }

        [Display(Name = "TransDate", ResourceType = typeof(Resources.Resource))]
        public DateTime TransDate { get; set; }

        [Display(Name = "Debit", ResourceType = typeof(Resources.Resource))]
        public double Debit { get; set; }
        [Display(Name = "Credit", ResourceType = typeof(Resources.Resource))]
        public double Credit { get; set; }

        [Display(Name = "DebitForeign", ResourceType = typeof(Resources.Resource))]
        public double DebitForeign { get; set; }

        [Display(Name = "CreditForeign", ResourceType = typeof(Resources.Resource))]
        public double CreditForeign { get; set; }


        [Display(Name = "Number", ResourceType = typeof(Resources.Resource))]
        public int VHI { get; set; }

        [Display(Name = "VoucherNumber", ResourceType = typeof(Resources.Resource))]
        public string VoucherNumber { get; set; }


        [Display(Name = "Serial", ResourceType = typeof(Resources.Resource))]
        public int RowNumber { get; set; }

        public bool WorkWithCostCenter { get; set; }


        public int TheDecimalPointForTheLocalCurrency { get; set; }
        public int TheDecimalPointForTheForeignCurrency { get; set; }





    }
}