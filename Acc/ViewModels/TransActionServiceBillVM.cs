using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Acc.ViewModels
{
    public class TransActionServiceBillVM
    {
        public int Id { get; set; }
        public int CompanyID { get; set; }
        public int BillID { get; set; }
        public int ServiceType { get; set; }
        public int CompanyYear { get; set; }

        [Display(Name = "ServiceNo", ResourceType = typeof(Resources.Resource))]
        public int ServiceNo { get; set; }
        [Display(Name = "Price", ResourceType = typeof(Resources.Resource))]
        public double Price { get; set; }
        [Display(Name = "ForeignPrice", ResourceType = typeof(Resources.Resource))]
        public double ForeignPrice { get; set; }
        [Display(Name = "Qty", ResourceType = typeof(Resources.Resource))]
        public int Qty { get; set; }

        [Display(Name = "LocalDiscount", ResourceType = typeof(Resources.Resource))]
        public double LocalDiscount { get; set; }
        [Display(Name = "ForeignDiscount", ResourceType = typeof(Resources.Resource))]
        public double ForeignDiscount { get; set; }
        [Display(Name = "TotalLocal", ResourceType = typeof(Resources.Resource))]
        public double TotalLocal { get; set; }
        [Display(Name = "TotalForeign", ResourceType = typeof(Resources.Resource))]
        public double TotalForeign { get; set; }
        [Display(Name = "Tax", ResourceType = typeof(Resources.Resource))]
        public double Tax { get; set; }
        [Display(Name = "Note", ResourceType = typeof(Resources.Resource))]
        public string Note { get; set; }
        [Display(Name = "ServiceNot", ResourceType = typeof(Resources.Resource))]
        public string ServiceNot { get; set; }
        public int iRowID { get; set; }
        [Display(Name = "PriceAfterLineDisc", ResourceType = typeof(Resources.Resource))]
        public double PriceAfterLineDisc { get; set; }
        [Display(Name = "PriceAfterLineDisc", ResourceType = typeof(Resources.Resource))]
        public double ForeignPriceAfterLineDisc { get; set; }
        [Display(Name = "TaxValue", ResourceType = typeof(Resources.Resource))]
        public double TaxValue { get; set; }
        [Display(Name = "TaxValue", ResourceType = typeof(Resources.Resource))]
        public double TotlTaxPrec { get; set; }
        [Display(Name = "NetAfterLineTax", ResourceType = typeof(Resources.Resource))]
        public double NetAfterLineTax { get; set; }
        [Display(Name = "TotalDiscPrec", ResourceType = typeof(Resources.Resource))]
        public double TotalDiscPrec { get; set; }
        [Display(Name = "TotalDiscValue", ResourceType = typeof(Resources.Resource))]
        public double TotalDiscValue { get; set; }
        [Display(Name = "NetAfterTotalDisc", ResourceType = typeof(Resources.Resource))]
        public double NetAfterTotalDisc { get; set; }
        [Display(Name = "NetTotalTax", ResourceType = typeof(Resources.Resource))]
        public double NetTotalTax { get; set; }
        public double ConversionFactor { get; set; }
        public int FCurrencyID { get; set; }
        public int TransactionKindNo { get; set; }
        public int CompanyTransactionKindNo { get; set; }
        [Display(Name = "AccountNumber", ResourceType = typeof(Resources.Resource))]
        public string AccountNumber { get; set; }
        [Display(Name = "CostCenter", ResourceType = typeof(Resources.Resource))]
        public string CostNumber { get; set; }
        public string AccountName { get; set; }
        public string CostName { get; set; }
        public string ServiceName { get; set; }
    }
}