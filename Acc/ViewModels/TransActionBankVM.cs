using Acc.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Acc.ViewModels
{
    public class TransActionBankVM
    {
        [Display(Name = "BankAcc", ResourceType = typeof(Resources.Resource))]
        public string BankAcc { get; set; }
        public string BankName { get; set; }

        [Display(Name = "BankCostCenter", ResourceType = typeof(Resources.Resource))]
        public string BankCostCenter { get; set; }

        public string BankCostCenterName { get; set; }

        [Display(Name = "TotalDebit", ResourceType = typeof(Resources.Resource))]
        public float TotalDebit { get; set; }

        [Display(Name = "Date", ResourceType = typeof(Resources.Resource))]
        public DateTime TransDate { get; set; }

 
        public string RefID { get; set; }

        [Display(Name = "Note", ResourceType = typeof(Resources.Resource))]
        public string Note { get; set; }

        public Header Header { get; set; }

        [Display(Name = "TransactionKind", ResourceType = typeof(Resources.Resource))]
        public int CompanyTransactionKindID { get; set; }
        public IEnumerable<CompanyTransactionKind> CompanyTransactionKind { get; set; }

        public IEnumerable<Transaction> Transaction { get; set; }

        [Display(Name = "Currency", ResourceType = typeof(Resources.Resource))]
        public int CurrencyID { get; set; }

        [Display(Name = "ConversionFactor", ResourceType = typeof(Resources.Resource))]
        public double CurrencyNewValue { get; set; }
        public IEnumerable<Currency> Currency { get; set; }
        public Transaction TransactionDebit { get; set; }
        [Display(Name = "TotalAmount", ResourceType = typeof(Resources.Resource))]
        public double TotalAmount { get; set; }

        [Display(Name = "ForeignAmount", ResourceType = typeof(Resources.Resource))]
        public double ForeignAmount { get; set; }

        [Display(Name = "TotalForeign", ResourceType = typeof(Resources.Resource))]
        public double TotalForeign { get; set; }
        [Display(Name = "Amount", ResourceType = typeof(Resources.Resource))]
        public double Amount { get; set; }
        public string CurrencyName { get; set; }
        public int CompanyYear { get; set; }
        public string CostCenterName { get; set; }
        public string CostCenter { get; set; }
        public double CreditForeign { get; set; }
        public string AccountName { get; set; }
        public string AccountNumber { get; set; }
        public double Credit { get; set; }
        public Boolean WorkWithCostCenter { get; set; }
        [Display(Name = "Amount", ResourceType = typeof(Resources.Resource))]
        public string sAmount { get; set; }
        [Display(Name = "ForeignAmount", ResourceType = typeof(Resources.Resource))]
        public string sForeignAmount { get; set; }
        public int TheDecimalPointForTheLocalCurrency { get; set; }
        public int TheDecimalPointForTheForeignCurrency { get; set; }

        public int CurrentYear { get; set; }

    }

}