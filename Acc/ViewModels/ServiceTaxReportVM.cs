﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Acc.ViewModels
{
    public class ServiceTaxReportVM
    {
        public Boolean SaleService { get; set; }
        public Boolean SaleMultiService { get; set; }
        public Boolean ReturnService { get; set; }
        public Boolean ReturnMultiService { get; set; }
        public Boolean PurchaseService { get; set; }
        public Boolean PurchaseMultiService { get; set; }
        public Boolean ReturnPurchaseService { get; set; }
        public Boolean ReturnPurchaseMultiService { get; set; }
        public Boolean Taxable { get; set; }
        public Boolean TaxExempt { get; set; }
        public Boolean Detailed { get; set; }
        public Boolean Collection { get; set; }
        [Display(Name = "FromDate", ResourceType = typeof(Resources.Resource))]
        public DateTime FromDate { get; set; }

        [Display(Name = "ToDate", ResourceType = typeof(Resources.Resource))]
        public DateTime ToDate { get; set; }
        public string AccountNumber { get; set; }
        public string AccountName { get; set; }
        public int TransactionKindNo { get; set; }
        public string TransactionKindName { get; set; }
        public string SumNetTotal { get; set; }
        public string SumTotal { get; set; }
        public string SumTax { get; set; }
        public string UserName { get; set; }
        public int BillID { get; set; }
        public DateTime BillDate { get; set; }
        public double Total { get; set; }
        public double Tax { get; set; }
        public double NetTotal { get; set; }
        public double TaxPercentage { get; set; }
        public string ServiceName { get; set; }
        public int ServiceNo { get; set; }
        public int Count { get; set; }
        public int TheDecimalPointForTheLocalCurrency { get; set; }
        public int TheDecimalPointForTheForeignCurrency { get; set; }
    }
}