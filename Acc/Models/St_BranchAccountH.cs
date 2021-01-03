using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Acc.Models
{
    public class St_BranchAccountH
    {
        public Company Company { get; set; }
        [Key]
        [Column(Order = 1)]
        public int CompanyID { get; set; }
        [Key]
        [Column(Order = 2)]
        [Display(Name = "BranchCode", ResourceType = typeof(Resources.Resource))]
        public string BranchCode { get; set; }
        [Display(Name = "FundAccountNumber", ResourceType = typeof(Resources.Resource))]
        public string FundAccountNumber { get; set; }
        [Display(Name = "SalesAccountNumber", ResourceType = typeof(Resources.Resource))]
        public string SalesAccountNumber { get; set; }
        [Display(Name = "MaintenanceCardAccountNumber", ResourceType = typeof(Resources.Resource))]
        public string MaintenanceCardAccountNumber { get; set; }
        [Display(Name = "ReturnCardAccountNumber", ResourceType = typeof(Resources.Resource))]
        public string ReturnCardAccountNumber { get; set; }
        [Display(Name = "TransferFeesAndInstallationAccountNumber", ResourceType = typeof(Resources.Resource))]
        public string TransferFeesAndInstallationAccountNumber { get; set; }
        [Display(Name = "ChequeFundAccountNumber", ResourceType = typeof(Resources.Resource))]
        public string ChequeFundAccountNumber { get; set; }
        [Display(Name = "RelocationAccountNumber", ResourceType = typeof(Resources.Resource))]
        public string RelocationAccountNumber { get; set; }
        [Display(Name = "VisaAccountNumber", ResourceType = typeof(Resources.Resource))]
        public string VisaAccountNumber { get; set; }
        [Display(Name = "MasterAccountNumber", ResourceType = typeof(Resources.Resource))]
        public string MasterAccountNumber { get; set; }
        [Display(Name = "AmericanAccountNumber", ResourceType = typeof(Resources.Resource))]
        public string AmericanAccountNumber { get; set; }
        [Display(Name = "ArabiCashAccountNumber", ResourceType = typeof(Resources.Resource))]
        public string ArabiCashAccountNumber { get; set; }
        [Display(Name = "ReceiptVoucher", ResourceType = typeof(Resources.Resource))]
        public int ReceiptVoucherNo { get; set; }
        [Display(Name = "PaymentVoucher", ResourceType = typeof(Resources.Resource))]
        public int PaymentVoucherNo { get; set; }
        [Display(Name = "ReceiptChequeVoucher", ResourceType = typeof(Resources.Resource))]
        public int ReceiptChequeVoucherNo { get; set; }
        [Display(Name = "TransferFeesAndInstallationVoucher", ResourceType = typeof(Resources.Resource))]
        public int TransferFeesAndInstallationVoucherNo { get; set; }
        [Display(Name = "MaintenanceCardVoucher", ResourceType = typeof(Resources.Resource))]
        public int MaintenanceCardVoucherNo { get; set; }
        [Display(Name = "TransferVoucher", ResourceType = typeof(Resources.Resource))]
        public int TransferVoucherNo { get; set; }
        [Display(Name = "ReturningSoldItemVoucher", ResourceType = typeof(Resources.Resource))]
        public int ReturningSoldItemVoucherNo { get; set; }
        [Display(Name = "InstallmentSchedulingVoucher", ResourceType = typeof(Resources.Resource))]
        public int InstallmentSchedulingVoucherNo { get; set; }
        [Display(Name = "ClosingAFundVoucher", ResourceType = typeof(Resources.Resource))]
        public int ClosingAFundVoucherNo { get; set; }
        [Display(Name = "ChequeFundClosingVoucher", ResourceType = typeof(Resources.Resource))]
        public int ChequeFundClosingVoucherNo { get; set; }
        [Display(Name = "ClosingAFinancingFundVoucher", ResourceType = typeof(Resources.Resource))]
        public int ClosingAFinancingFundVoucherNo { get; set; }
        [Display(Name = "FinancingVoucher", ResourceType = typeof(Resources.Resource))]
        public int FinancingVoucherNo { get; set; }
        [Display(Name = "InstallationVoucher", ResourceType = typeof(Resources.Resource))]
        public int InstallationVoucherNo { get; set; }
        [Display(Name = "InsUserName", ResourceType = typeof(Resources.Resource))]
        public string InsUserID { get; set; }
        [Display(Name = "InsDateTime", ResourceType = typeof(Resources.Resource))]
        public DateTime InsDateTime { get; set; }
    }
}