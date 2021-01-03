using Acc.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Acc.ViewModels
{
    public class St_BranchAccountHVM
    {
        public int CompanyID { get; set; }
        [Display(Name = "BranchCode", ResourceType = typeof(Resources.Resource))]
        public string BranchCode { get; set; }
        [Display(Name = "Branches", ResourceType = typeof(Resources.Resource))]
        public IEnumerable<St_BranchH> St_BranchH { get; set; }
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
        public IEnumerable<CompanyTransactionKind> CompanyTransactionKindReceiptVoucher { get; set; }
        public int ReceiptVoucherNo { get; set; }
        [Display(Name = "PaymentVoucher", ResourceType = typeof(Resources.Resource))]
        public IEnumerable<CompanyTransactionKind> CompanyTransactionKindPaymentVoucher { get; set; }
        public int PaymentVoucherNo { get; set; }
        [Display(Name = "ReceiptChequeVoucher", ResourceType = typeof(Resources.Resource))]
        public IEnumerable<CompanyTransactionKind> CompanyTransactionKindReceiptChequeVoucher { get; set; }
        public int ReceiptChequeVoucherNo { get; set; }
        [Display(Name = "TransferFeesAndInstallationVoucher", ResourceType = typeof(Resources.Resource))]
        public IEnumerable<CompanyTransactionKind> CompanyTransactionKindTransferFeesAndInstallationVoucher { get; set; }
        public int TransferFeesAndInstallationVoucherNo { get; set; }
        [Display(Name = "MaintenanceCardVoucher", ResourceType = typeof(Resources.Resource))]
        public IEnumerable<CompanyTransactionKind> CompanyTransactionKindMaintenanceCardVoucher { get; set; }
        public int MaintenanceCardVoucherNo { get; set; }
        [Display(Name = "TransferVoucher", ResourceType = typeof(Resources.Resource))]
        public IEnumerable<CompanyTransactionKind> CompanyTransactionKindTransferVoucher { get; set; }
        public int TransferVoucherNo { get; set; }
        [Display(Name = "ReturningSoldItemVoucher", ResourceType = typeof(Resources.Resource))]
        public IEnumerable<CompanyTransactionKind> CompanyTransactionKindReturningSoldItemVoucher { get; set; }
        public int ReturningSoldItemVoucherNo { get; set; }
        [Display(Name = "InstallmentSchedulingVoucher", ResourceType = typeof(Resources.Resource))]
        public IEnumerable<CompanyTransactionKind> CompanyTransactionKindInstallmentSchedulingVoucher { get; set; }
        public int InstallmentSchedulingVoucherNo { get; set; }
        [Display(Name = "ClosingAFundVoucher", ResourceType = typeof(Resources.Resource))]
        public IEnumerable<CompanyTransactionKind> CompanyTransactionKindClosingAFundVoucher { get; set; }
        public int ClosingAFundVoucherNo { get; set; }
        [Display(Name = "ChequeFundClosingVoucher", ResourceType = typeof(Resources.Resource))]
        public IEnumerable<CompanyTransactionKind> CompanyTransactionKindChequeFundClosingVoucher { get; set; }
        public int ChequeFundClosingVoucherNo { get; set; }
        [Display(Name = "ClosingAFinancingFundVoucher", ResourceType = typeof(Resources.Resource))]
        public IEnumerable<CompanyTransactionKind> CompanyTransactionKindClosingAFinancingFundVoucher { get; set; }
        public int ClosingAFinancingFundVoucherNo { get; set; }
        [Display(Name = "FinancingVoucher", ResourceType = typeof(Resources.Resource))]
        public IEnumerable<CompanyTransactionKind> CompanyTransactionKindFinancingVoucher { get; set; }
        public int FinancingVoucherNo { get; set; }
        [Display(Name = "InstallationVoucher", ResourceType = typeof(Resources.Resource))]
        public IEnumerable<CompanyTransactionKind> CompanyTransactionKindInstallationVoucher { get; set; }
        public int InstallationVoucherNo { get; set; }
        [Display(Name = "InsUserName", ResourceType = typeof(Resources.Resource))]
        public string InsUserID { get; set; }
        [Display(Name = "InsDateTime", ResourceType = typeof(Resources.Resource))]
        public DateTime InsDateTime { get; set; }
        [Display(Name = "ExpenseAccountNumber", ResourceType = typeof(Resources.Resource))]
        public string ExpenseAccountNumber { get; set; }
        [Display(Name = "OtherExpenseAccountNumber", ResourceType = typeof(Resources.Resource))]
        public string OtherExpenseAccountNumber { get; set; }
        public string FundAccountName { get; set; }
        public string SalesAccountName { get; set; }
        public string MaintenanceCardAccountName { get; set; }
        public string ReturnCardAccountName { get; set; }
        public string TransferFeesAndInstallationAccountName { get; set; }
        public string ChequeFundAccountName { get; set; }
        public string RelocationAccountName { get; set; }
        public string VisaAccountName { get; set; }
        public string MasterAccountName { get; set; }
        public string AmericanAccountName { get; set; }
        public string ArabiCashAccountName { get; set; }
        public string ExpenseAccountName { get; set; }
        public string OtherExpenseAccountName { get; set; }
        public string BranchName { get; set; }
        public int ExpenseAccountRowNumber { get; set; }
        public int OtherExpenseAccountRowNumber { get; set; }
        public IEnumerable<St_BranchExpenseAccountH> St_BranchExpenseAccountH { get; set; }
        public IEnumerable<St_BranchOtherExpenseAccountH> St_BranchOtherExpenseAccountH { get; set; }
    }
}