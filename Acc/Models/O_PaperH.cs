using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Acc.Models
{
    public class O_PaperH
    {
        public int Id { get; set; }
        public Company Company { get; set; }
        public int CompanyID { get; set; }
        public int CompanyYear { get; set; }
        public string StockCode { get; set; }
        public string BranchCode { get; set; }
        public int CompanyTransactionKindNo { get; set; }
        public int TransactionKindNo { get; set; }

        [Display(Name = "VoucherNumber", ResourceType = typeof(Resources.Resource))]
        public string VoucherNumber { get; set; }
        [Display(Name = "VoucherNumber", ResourceType = typeof(Resources.Resource))]
        public int VHI { get; set; }
        [Display(Name = "VoucherDate", ResourceType = typeof(Resources.Resource))]
        public DateTime VoucherDate { get; set; }
        public int RowNumber { get; set; }
        [Required]
        [Display(Name = "ChequeNumber", ResourceType = typeof(Resources.Resource))]
        public string ChequeNumber { get; set; }
        [Required]
        [Display(Name = "ChequeDate", ResourceType = typeof(Resources.Resource))]
        public DateTime ChequeDate { get; set; }

        [Display(Name = "ChequeAmount", ResourceType = typeof(Resources.Resource))]
        public double ChequeAmount { get; set; }
        public int ChequeCase { get; set; }
        public string OldVoucherNumber { get; set; }
        public int OldVHI { get; set; }
        public int OldCompanyTransactionKindNo { get; set; }
        public int OldTransactionKindNo { get; set; }
        public string AccountNumberFirst { get; set; }
        public string AccountNumberSecond { get; set; }
        public string AccountNumberThird { get; set; }
        public string AccountNumberFourth { get; set; }
        public string AccountNumberFifth { get; set; }
        public string DrawerName { get; set; }
        public string Remark { get; set; }
        public string ReturnNote { get; set; }
        public string ReturnCustomerNote { get; set; }
        public string UnderReturnNote { get; set; }
        public string UnderReturnCustomerNote { get; set; }
        public string EndorReturnNote { get; set; }
        public double OldSupplierBalance { get; set; }
        [Display(Name = "InsUserName", ResourceType = typeof(Resources.Resource))]
        public string InsUserID { get; set; }
        [Display(Name = "InsDateTime", ResourceType = typeof(Resources.Resource))]
        public DateTime InsDateTime { get; set; }
        public string CaseNumber { get; set; }
        public bool Sirs { get; set; }
        public bool Mr { get; set; }
        public bool Co { get; set; }
        public bool First { get; set; }
        public bool Mrs { get; set; }
        public string BankAccountNameJustForFill { get; set; }
        public string OriginalVoucherNumber { get; set; }
        public int OriginalVHI { get; set; }
        public int OriginalCompanyTransactionKindNo { get; set; }
        public int OriginalTransactionKindNo { get; set; }
        public int IsBill { get; set; }
        public string BillCustomerAccountNumber { get; set; }
        public string BillNumber { get; set; }
        public double ConversionFactor { get; set; }
        public int FCurrencyID { get; set; }
        public string sChequeDate { get; set; }
        public string PaymentReturnNote { get; set; }
        public int SaleID { get; set; }
        public string PaymentPaidReturnNote { get; set; }
    }
}