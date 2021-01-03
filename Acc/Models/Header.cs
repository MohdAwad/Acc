using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Acc.Models
{
    public class Header
    {
        public Company Company { get; set; }
        [Key]
        [Column(Order = 1)]
        public int CompanyID { get; set; }
        [Key]
        [Column(Order = 2)]
        public int CompanyYear { get; set; }
        public CompanyTransactionKind CompanyTransactionKind { get; set; }
        [Key]
        [Column(Order = 3)]
        public int CompanyTransactionKindNo { get; set; }
        [Key]
        [Column(Order = 4)]
        public int TransactionKindNo { get; set; }
        [Key]
        [Column(Order = 5)]
        [Display(Name = "VoucherNumber", ResourceType = typeof(Resources.Resource))]
        public string VoucherNumber { get; set; }


        [Display(Name = "VoucherNumber", ResourceType = typeof(Resources.Resource))]
        public int VHI { get; set; }

        [Display(Name = "Date", ResourceType = typeof(Resources.Resource))]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime VoucherDate { get; set; }

        [Display(Name = "TotalDebit", ResourceType = typeof(Resources.Resource))]
        public double TotalDebit { get; set; }
        [Display(Name = "TotalCredit", ResourceType = typeof(Resources.Resource))]
        public double TotalCredit { get; set; }
        [Display(Name = "TotalDebitForeign", ResourceType = typeof(Resources.Resource))]
        public double TotalDebitForeign { get; set; }

        [Display(Name = "TotalCreditForeign", ResourceType = typeof(Resources.Resource))]
        public double TotalCreditForeign { get; set; }
        public double ConversionFactor { get; set; }
        [Display(Name = "Statement", ResourceType = typeof(Resources.Resource))]
        public string Note { get; set; }
        public int RowCount { get; set; } // Grid Row Count
        public int Exported { get; set; } // Default 0
        public int IsBill { get; set; }  // // Default 0 للكمبيالات
        public string CustomerNo { get; set; } // Default null للكمبيالات
        public string BillNumber { get; set; } // Default null للكمبيالات
        [Display(Name = "InsUserName", ResourceType = typeof(Resources.Resource))]
        public string InsUserID { get; set; }
        [Display(Name = "InsDateTime", ResourceType = typeof(Resources.Resource))]
        public DateTime InsDateTime { get; set; }
        public int FCurrencyID { get; set; }
        [Display(Name = "Sales", ResourceType = typeof(Resources.Resource))]
        public int SaleID { get; set; }
    }
}