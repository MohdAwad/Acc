using Acc.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Acc.ViewModels
{
    public class SalesReportVM
    {
        public Boolean ReceiptCash { get; set; }
        public Boolean ReceiptBank { get; set; }
        public Boolean DebitNote { get; set; }
        public Boolean SaleService { get; set; }
        public Boolean SaleMultiService { get; set; }
        public Boolean ReturnService { get; set; }
        public Boolean ReturnMultiService { get; set; }
        public Boolean ReceiptVoucherCashMultiAccount { get; set; }
        
        [Display(Name = "FromDate", ResourceType = typeof(Resources.Resource))]
        public DateTime FromDate { get; set; }

        [Display(Name = "ToDate", ResourceType = typeof(Resources.Resource))]
        public DateTime ToDate { get; set; }
        public int TransactionKindNo { get; set; }
        public string TransactionKindName { get; set; }
        public DateTime VoucherDate { get; set; }
        [Display(Name = "VoucherNumber", ResourceType = typeof(Resources.Resource))]
        public string VoucherNumber { get; set; }
        public double Amount { get; set; }
        [Display(Name = "Sales", ResourceType = typeof(Resources.Resource))]
        public int SaleID { get; set; }
        public IEnumerable<Sale> SaleMan { get; set; }
        public string SaleName { get; set; }
        public string NetTotal { get; set; }
        public string UserName { get; set; }
        public string AccountNumber { get; set; }
        public string AccountName { get; set; }
        public int TheDecimalPointForTheLocalCurrency { get; set; }
        public int TheDecimalPointForTheForeignCurrency { get; set; }


    }
}