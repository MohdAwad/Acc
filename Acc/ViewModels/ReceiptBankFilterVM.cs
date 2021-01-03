using Acc.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Acc.ViewModels
{
    public class ReceiptBankFilterVM
    {
        public string CompanyTransactionName { get; set; }
        public string Export { get; set; }
        public string CurrencyName { get; set; }
        public string UserName { get; set; }
        [Display(Name = "CompanyTransactionKindID", ResourceType = typeof(Resources.Resource))]
        public int CompanyTransactionKindNo { get; set; }
        public IEnumerable<CompanyTransactionKind> CompanyTransactionKind { get; set; }
        public IEnumerable<Sale> Sale { get; set; }
        [Display(Name = "VoucherNumber", ResourceType = typeof(Resources.Resource))]
        public string VoucherNumber { get; set; }
        public DateTime VoucherDate { get; set; }
        public double TotalAmount { get; set; }
        public double ForeignTotal { get; set; }
        [Display(Name = "CurrencyID", ResourceType = typeof(Resources.Resource))]
        public int CurrencyID { get; set; }
        public IEnumerable<Currency> Currency { get; set; }
        [Display(Name = "FromDate", ResourceType = typeof(Resources.Resource))]
        public DateTime FromDate { get; set; }
        [Display(Name = "ToDate", ResourceType = typeof(Resources.Resource))]
        public DateTime ToDate { get; set; }
        public int Exported { get; set; }
        public int CompanyYear { get; set; }
        public int iRowTable { get; set; }
        public int TransactionKindNo { get; set; }
        public int AllUsedPapers { get; set; }
        public int AllPapers { get; set; }
        public string PaidAccountName { get; set; }
        public string PaidAccountNumber { get; set; }
        public int SaleID { get; set; }
        public string SaleName { get; set; }
        public int TheDecimalPointForTheLocalCurrency { get; set; }
        public int TheDecimalPointForTheForeignCurrency { get; set; }
        public int CheckCase { get; set; }
    }
}