using Acc.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Acc.ViewModels
{
    public class TransActionModelVM
    {
         
        public int CompanyID { get; set; }
        
        public int CompanyYear { get; set; }
        public CompanyTransactionKind CompanyTransactionKind { get; set; }
       
        public int CompanyTransactionKindNo { get; set; }
         
        public int TransactionKindNo { get; set; }
       
        [Display(Name = "VoucherNumber", ResourceType = typeof(Resources.Resource))]
        public string VoucherNumber { get; set; }

        [Display(Name = "VoucherNumber", ResourceType = typeof(Resources.Resource))]
        public int VHI { get; set; }

        [Display(Name = "VoucherDate", ResourceType = typeof(Resources.Resource))]
        public DateTime VoucherDate { get; set; }
        [Display(Name = "Debit", ResourceType = typeof(Resources.Resource))]
        public double Debit { get; set; }
        [Display(Name = "Credit", ResourceType = typeof(Resources.Resource))]
        public double Credit { get; set; }
        [Display(Name = "DebitForeign", ResourceType = typeof(Resources.Resource))]
        public double DebitForeign { get; set; }
        [Display(Name = "CreditForeign", ResourceType = typeof(Resources.Resource))]
        public double CreditForeign { get; set; }
        [Display(Name = "AccountNumber", ResourceType = typeof(Resources.Resource))]
        public string AccountNumber { get; set; }


        [Display(Name = "ArabicName", ResourceType = typeof(Resources.Resource))]
        public string AccountName { get; set; }

        [Display(Name = "CostCenter", ResourceType = typeof(Resources.Resource))]
        public string CostCenter { get; set; }

        [Display(Name = "Note", ResourceType = typeof(Resources.Resource))]
        public string Note { get; set; }
         
        public int RowNumber { get; set; }
        public int Exported { get; set; } // Default 0
        public int IsBill { get; set; }  // // Default 0 للكمبيالات
        public string CustomerNo { get; set; } // Default null للكمبيالات
        public string CustomerName { get; set; } // Default null للكمبيالات
        public string BillNumber { get; set; } // Default null للكمبيالات
        [Display(Name = "InsUserName", ResourceType = typeof(Resources.Resource))]
        public string InsUserID { get; set; }
        [Display(Name = "InsDateTime", ResourceType = typeof(Resources.Resource))]
        public DateTime InsDateTime { get; set; }

        [Display(Name = "Foreign", ResourceType = typeof(Resources.Resource))]
        public double CreditDebitForeign { get; set; }


    }
}