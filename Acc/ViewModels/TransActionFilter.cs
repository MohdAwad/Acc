using Acc.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Acc.ViewModels
{
    public class TransActionFilter
    {
        public string CompanyTransactionName { get; set; }
        public string Export { get; set; }
        public string CurrencyName { get; set; }
        public string UserName { get; set; }
        [Display(Name = "CompanyTransactionKindID", ResourceType = typeof(Resources.Resource))]
        public int CompanyTransactionKindNo { get; set; }
        public int TransactionKindNo { get; set; }
        public IEnumerable<CompanyTransactionKind> CompanyTransactionKind { get; set; }

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
        [Display(Name = "AccountNumber", ResourceType = typeof(Resources.Resource))]
        public string AccountNumber { get; set; }
        [Display(Name = "AccountName", ResourceType = typeof(Resources.Resource))]
        public string AccountName { get; set; }
        [Display(Name = "Sales", ResourceType = typeof(Resources.Resource))]
        public int SaleManNo { get; set; }
        public IEnumerable<Sale> SaleMan { get; set; }
        public string SaleManName { get; set; }
        public int TheDecimalPointForTheLocalCurrency { get; set; }
        public int TheDecimalPointForTheForeignCurrency { get; set; }
        public int CheckCase { get; set; }
    }
}