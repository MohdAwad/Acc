using Acc.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
namespace Acc.ViewModels
{
    public class HeaderServiceBillVM
    {


        public static object HeaderData { get; internal set; }
        public IEnumerable<CompanyTransactionKind> CompanyTransactionKind { get; set; }
        [Display(Name = "CompanyTransactionKindID", ResourceType = typeof(Resources.Resource))]
        public int CompanyTransactionKindID { get; set; }

        public string CompanyTransactionKindName { get; set; }
        public int CompanyID { get; set; }
     
        [Display(Name = "Serial", ResourceType = typeof(Resources.Resource))]
        public int BillID { get; set; }
        [Display(Name = "Serial", ResourceType = typeof(Resources.Resource))]
        public int TransactionKindNo { get; set; }
        public int CompanyYear { get; set; }
        [Display(Name = "Date", ResourceType = typeof(Resources.Resource))]
        public DateTime BillDate { get; set; }
        [Display(Name = "AccountNumber", ResourceType = typeof(Resources.Resource))]
        public string AccountNumber { get; set; }
        [Display(Name = "AccountName", ResourceType = typeof(Resources.Resource))]
        public string AccountName { get; set; }
        [Display(Name = "NoTax", ResourceType = typeof(Resources.Resource))]
        public bool NoTax { get; set; }
        [Display(Name = "AccountNumber", ResourceType = typeof(Resources.Resource))]
        public string DebitAccountNumber { get; set; }
        [Display(Name = "AccountNumber", ResourceType = typeof(Resources.Resource))]
        public string CreditAccountNumber { get; set; }
        [Display(Name = "TaxAccountNumber", ResourceType = typeof(Resources.Resource))]
        public string TaxAccountNumber { get; set; }
        [Display(Name = "DebitCostNumber", ResourceType = typeof(Resources.Resource))]
        public string DebitCostNumber { get; set; }
        [Display(Name = "CreditCostNumber", ResourceType = typeof(Resources.Resource))]
        public string CreditCostNumber { get; set; }
        [Display(Name = "TaxCostNumber", ResourceType = typeof(Resources.Resource))]
        public string TaxCostNumber { get; set; }
        [Display(Name = "Note", ResourceType = typeof(Resources.Resource))]
        public string Note { get; set; }
        [Display(Name = "Discount", ResourceType = typeof(Resources.Resource))]
        public double Discount { get; set; }
        [Display(Name = "Tax", ResourceType = typeof(Resources.Resource))]
        public double Tax { get; set; }
        [Display(Name = "Total", ResourceType = typeof(Resources.Resource))]
        public double Total { get; set; }
        [Display(Name = "NetTotal", ResourceType = typeof(Resources.Resource))]
        public double NetTotal { get; set; }
        [Display(Name = "DebitCostNumber", ResourceType = typeof(Resources.Resource))]
        public string DebitCostName { get; set; }
        [Display(Name = "CreditCostNumber", ResourceType = typeof(Resources.Resource))]
        public string CreditCostName { get; set; }

        [Display(Name = "TaxCostNumber", ResourceType = typeof(Resources.Resource))]
        public string TaxCostName { get; set; }
        [Display(Name = "AccountNumber", ResourceType = typeof(Resources.Resource))]
        public string DebitAccountName { get; set; }
        [Display(Name = "AccountNumber", ResourceType = typeof(Resources.Resource))]
        public string CreditAccountName { get; set; }
        [Display(Name = "TaxAccountNumber", ResourceType = typeof(Resources.Resource))]
        public string TaxAccountName { get; set; }
        [Display(Name = "Sales", ResourceType = typeof(Resources.Resource))]
        public int SaleManNo { get; set; }
        public IEnumerable<Sale> SaleMan { get; set; }
        public string SaleManName { get; set; }
        public bool WorkWithCostCenter { get; set; }
        [Display(Name = "CurrencyID", ResourceType = typeof(Resources.Resource))]
        public int CurrencyID { get; set; }

        [Display(Name = "ConversionFactor", ResourceType = typeof(Resources.Resource))]
        public double CurrencyNewValue { get; set; }
        public IEnumerable<Currency> Currency { get; set; }
        public string CurrencyName { get; set; }
        [Display(Name = "Total", ResourceType = typeof(Resources.Resource))]
        public string sTotal { get; set; }
        [Display(Name = "NetTotal", ResourceType = typeof(Resources.Resource))]
        public string sNetTotal { get; set; }
        [Display(Name = "Tax", ResourceType = typeof(Resources.Resource))]
        public string sTax { get; set; }
        public string OrignailTaxAccountNumber { get; set; }
        public string OrignailTaxAccountName { get; set; }
        [Display(Name = "TotalForeignAmount", ResourceType = typeof(Resources.Resource))]
        public double ForeignAmount { get; set; }
        [Display(Name = "NetTotalForeignAmount", ResourceType = typeof(Resources.Resource))]
        public double NetTotalForeignAmount { get; set; }
        [Display(Name = "ForeignAmountTax", ResourceType = typeof(Resources.Resource))]
        public double ForeignAmountTax { get; set; }
        public string sForeignAmount { get; set; }
        public string sNetTotalForeignAmount { get; set; }
        public string sForeignAmountTax { get; set; }
        public int TheDecimalPointForTheLocalCurrency { get; set; }
        public int TheDecimalPointForTheForeignCurrency { get; set; }

        public int CurrentYear { get; set; }
    }
}