using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Acc.ViewModels
{
    public class HeaderReportVM
    {
     
        public int CompanyID { get; set; }
     
        public int CompanyYear { get; set; }
 
        public int CompanyTransactionKindNo { get; set; }
       
        public int TransactionKindNo { get; set; }
        
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
        [Display(Name = "Note", ResourceType = typeof(Resources.Resource))]
        public string Note { get; set; }
        public int RowCount { get; set; } // Grid Row Count
        public int Exported { get; set; } // Default 0
        public int IsBill { get; set; }  // // Default 0 للكمبيالات
        public string CustomerNo { get; set; } // Default null للكمبيالات
        public string CustomerName { get; set; } // Default null للكمبيالات
        public string BillNumber { get; set; } // Default null للكمبيالات
        [Display(Name = "InsUserName", ResourceType = typeof(Resources.Resource))]
        public string InsUserID { get; set; }
        [Display(Name = "InsDateTime", ResourceType = typeof(Resources.Resource))]
        public DateTime InsDateTime { get; set; }

        public string ArabicName { get; set; }
        public string EnglishName { get; set; }
        public string BankAcc { get; set; }
        public string BankName { get; set; }

        public int FCurrencyID { get; set; }

        [Display(Name = "FromAccount", ResourceType = typeof(Resources.Resource))]
        public string FromAccount { get; set; }

        [Display(Name = "Name", ResourceType = typeof(Resources.Resource))]
        public string FromAccName { get; set; }


        [Display(Name = "ToAccount", ResourceType = typeof(Resources.Resource))]
        public string ToAccount { get; set; }

        [Display(Name = "Name", ResourceType = typeof(Resources.Resource))]
        public string ToAccName { get; set; }


        [Display(Name = "PrepaidStartDate", ResourceType = typeof(Resources.Resource))]
        public DateTime PrepaidStartDate { get; set; }

        [Display(Name = "PaidPeriod", ResourceType = typeof(Resources.Resource))]
        public int PrePaidPeriod { get; set; }

        [Display(Name = "Ammount", ResourceType = typeof(Resources.Resource))]
        public float TotalPrepaidValue { get; set; }


        [Display(Name = "Statement", ResourceType = typeof(Resources.Resource))]
        public string PrepaidStatement { get; set; }

        [Display(Name = "PrepaidStartDate", ResourceType = typeof(Resources.Resource))]
        public DateTime RevenueStartDate { get; set; }

        [Display(Name = "PaidPeriod", ResourceType = typeof(Resources.Resource))]
        public int RevenuePeriod { get; set; }

        [Display(Name = "Ammount", ResourceType = typeof(Resources.Resource))]
        public float TotalRevenueValue { get; set; }

        [Display(Name = "Statement", ResourceType = typeof(Resources.Resource))]
        public string RevenueStatement { get; set; }
        [Display(Name = "Collected", ResourceType = typeof(Resources.Resource))]
        public float TotalCollectedRevenue { get; set; }

        public int iRowTable { get; set; }

    }
}