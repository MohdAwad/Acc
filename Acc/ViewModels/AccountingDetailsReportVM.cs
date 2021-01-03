using Acc.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Acc.ViewModels
{
    public class AccountingDetailsReportVM
    {
        [Display(Name = "CompanyTransactionKindID", ResourceType = typeof(Resources.Resource))]
        public int CompanyTransactionKindNo { get; set; }
        public IEnumerable<CompanyTransactionKind> CompanyTransactionKind { get; set; }
        [Display(Name = "FromVoucherNumber", ResourceType = typeof(Resources.Resource))]
        public int FromVoucherNumber { get; set; }
        [Display(Name = "ToVoucherNumber", ResourceType = typeof(Resources.Resource))]
        public int ToVoucherNumber { get; set; }
        public Boolean AllTransaction { get; set; }
        public Boolean AllExportTransaction { get; set; }
        public Boolean AllUnExportTransaction { get; set; }
        public Boolean DateApproval { get; set; }
        public Boolean VoucherApproval { get; set; }
        public Boolean NoteApproval { get; set; }
        [Display(Name = "FromDate", ResourceType = typeof(Resources.Resource))]
        public DateTime FromDate { get; set; }

        [Display(Name = "ToDate", ResourceType = typeof(Resources.Resource))]
        public DateTime ToDate { get; set; }
        public int CompanyYear { get; set; }
        public int TransactionKindNo { get; set; }
        public DateTime VoucherDate { get; set; }
        public string VoucherNumber{ get; set; }
        public int VHI { get; set; }
        public string Note { get; set; }
        public int CompanyID { get; set; }
        public string CompanyTransactionKindName { get; set; }
        public string UserName { get; set; }
        public double Amount { get; set; }
        public Boolean WorkWithCostCenter { get; set; }
        public int Exported { get; set; }
        public double Debit { get; set; }
        public double Credit { get; set; }
        public double CreditDebitForeign { get; set; }
        public string AccountNumber { get; set; }
        public string CostCenter { get; set; }
        public string AccountName { get; set; }
        public string CostCenterName { get; set; }
        public int  ServiceNo { get; set; }
        public string ServiceName { get; set; }
        public int Qty { get; set; }
        public double Price { get; set; }
        public double LocalDiscount { get; set; }
        public double PriceAfterLineDisc { get; set; }
        public double Tax { get; set; }
        public string ServiceNot { get; set; }
        public double TotalDiscValue { get; set; }
        public double NetAfterTotalDisc { get; set; }
        public string SearchNote { get; set; }


        public int TheDecimalPointForTheLocalCurrency { get; set; }
        public int TheDecimalPointForTheForeignCurrency { get; set; }


    }
}