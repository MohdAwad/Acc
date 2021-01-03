using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Acc.ViewModels
{
    public class TransReportDetailVM
    {
      

        [Display(Name = "AccountNumber", ResourceType = typeof(Resources.Resource))]
        public string AccountNumber { get; set; }

        [Display(Name = "AccountName", ResourceType = typeof(Resources.Resource))]
        public string AccountName { get; set; }

        [Display(Name = "AccountName", ResourceType = typeof(Resources.Resource))]
        public string ArabicName { get; set; }


        [Display(Name = "AccountName", ResourceType = typeof(Resources.Resource))]
        public string EnglishName { get; set; }





        [Display(Name = "Note", ResourceType = typeof(Resources.Resource))]
        public string Note { get; set; }



        [Display(Name = "TransName", ResourceType = typeof(Resources.Resource))]
        public string TransName { get; set; }

        [Display(Name = "TransDate", ResourceType = typeof(Resources.Resource))]
        public DateTime VoucherDate { get; set; }

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

        [Display(Name = "CostCenter", ResourceType = typeof(Resources.Resource))]
        public string CostCenter { get; set; }


        [Display(Name = "CostCenterName", ResourceType = typeof(Resources.Resource))]
        public string CostCenterName { get; set; }


        public int CompanyTransactionKindNo { get; set; }

        public int TransactionKindNo { get; set; }

        public int AccountKind { get; set; }

        public int AccountTypeID { get; set; }

        public int AccountLevel { get; set; }

        public int CompanyYear { get; set; }
        public int iRowID { get; set; }
        public Boolean WorkWithCostCenter { get; set; }

    }
}