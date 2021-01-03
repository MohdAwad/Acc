using Acc.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Acc.ViewModels
{
    public class TransActionVM
    {
        public Header Header { get; set; }

        [Display(Name = "TransactionKind", ResourceType = typeof(Resources.Resource))] 
        public int CompanyTransactionKindID { get; set; }
        public IEnumerable <CompanyTransactionKind> CompanyTransactionKind { get; set; }
        public IEnumerable<Transaction> Transaction { get; set; }
        [Display(Name = "Currency", ResourceType = typeof(Resources.Resource))]
        public int CurrencyID { get; set; }

        [Display(Name = "ConversionFactor", ResourceType = typeof(Resources.Resource))]
        public double CurrencyNewValue { get; set; }
        public IEnumerable<Currency> Currency { get; set; }
        public BankGuarantee BankGuarantee { get; set; }
        [Display(Name = "Diff", ResourceType = typeof(Resources.Resource))]
        public double NetTotal { get; set; }
        public bool WorkWithCostCenter { get; set; }
        [Display(Name = "TotalDebit", ResourceType = typeof(Resources.Resource))]
        public string TotalDebit { get; set; }
        [Display(Name = "TotalCredit", ResourceType = typeof(Resources.Resource))]
        public string TotalCredit { get; set; }
        [Display(Name = "Diff", ResourceType = typeof(Resources.Resource))]
        public string sNetTotal { get; set; }
        [Display(Name = "FromDate", ResourceType = typeof(Resources.Resource))]
        public DateTime FromDate { get; set; }

        [Display(Name = "ToDate", ResourceType = typeof(Resources.Resource))]
        public DateTime ToDate { get; set; }
        [Display(Name = "VoucherNumber", ResourceType = typeof(Resources.Resource))]
        public string VoucherNumber { get; set; }
        [Display(Name = "CompanyTransactionKindID", ResourceType = typeof(Resources.Resource))]
        public int CompanyTransactionKindNo { get; set; }
        public int iRowTable { get; set; }
        public string CurrencyName { get; set; }
        public int Exported { get; set; }
        public string CompanyTransactionName { get; set; }
        public int CompanyYear { get; set; }
        public int TransactionKindNo { get; set; }
        [Display(Name = "VoucherDate", ResourceType = typeof(Resources.Resource))]
        public DateTime VoucherDate { get; set; }
        public double TotalAmount { get; set; }
        public double ForeignTotal { get; set; }
        public int TheDecimalPointForTheLocalCurrency { get; set; }
        public int TheDecimalPointForTheForeignCurrency { get; set; }
        public string AccountName { get; set; }
        public string CostName { get; set; }
        public double Debit { get; set; }
        public double Credit { get; set; }
        public double DebitForeign { get; set; }
        public double CreditForeign { get; set; }
        public double CreditDebitForeign { get; set; }
        public string AccountNumber { get; set; }
        public string Note { get; set; }
        public string CostCenter { get; set; }
        public int RowNumber { get; set; }
        public int CurrentYear { get; set; }
    }
}