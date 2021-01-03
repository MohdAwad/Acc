using Acc.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Acc.ViewModels
{
    public class PaperFilterVM
    {
        public string CompanyTransactionName { get; set; }

        [Display(Name = "CompanyTransactionKindID", ResourceType = typeof(Resources.Resource))]
        public int CompanyTransactionKindNo { get; set; }
        [Display(Name = "CompanyTransactionKindID", ResourceType = typeof(Resources.Resource))]
        public int CompanyTransactionKindToTransferNo { get; set; }
        public IEnumerable<CompanyTransactionKind> CompanyTransactionKind { get; set; }
        public IEnumerable<CompanyTransactionKind> CompanyTransactionKindToTransfer { get; set; }
        public IEnumerable<Sale> Sale { get; set; }
        [Display(Name = "ReceiptVoucherNumber", ResourceType = typeof(Resources.Resource))]
        public string VoucherNumber { get; set; }
        public DateTime VoucherDate { get; set; }
        [Display(Name = "CustomerName", ResourceType = typeof(Resources.Resource))]
        public string CustomerName { get; set; }
        [Display(Name = "BankName", ResourceType = typeof(Resources.Resource))]
        public string BankName { get; set; }
        [Display(Name = "ChequeBoxName", ResourceType = typeof(Resources.Resource))]
        public string ChequeBoxName { get; set; }
        [Display(Name = "ChequeNumber", ResourceType = typeof(Resources.Resource))]
        public string ChequeNumber { get; set; }
        public DateTime ChequeDate { get; set; }
        public double ChequeAmount { get; set; }
        public string DrawerName { get; set; }
        public string UserName { get; set; }
        public int CompanyYear { get; set; }
        public int TransactionKindNo { get; set; }
        public int RowNumber { get; set; }
        public string AccountNumberFirst { get; set; }
        public string AccountNumberSecond { get; set; }
        public string AccountNumberThird { get; set; }
        public string AccountNumberFourth { get; set; }
        public string AccountNumberFifth { get; set; }
        public string FromCostCenter { get; set; }
        public string ToCostCenter { get; set; }
        public string FromCostCenterName { get; set; }
        public string ToCostCenterName { get; set; }
        public int iRowTable { get; set; }
        [Display(Name = "FromDateCheque", ResourceType = typeof(Resources.Resource))]
        public DateTime FromDate { get; set; }

        [Display(Name = "ToDate", ResourceType = typeof(Resources.Resource))]
        public DateTime ToDate { get; set; }
        public int IsBill { get; set; }
        public string BillCustomerNumber { get; set; }
        public string BillCustomerName { get; set; }
        public int PaperCount { get; set; }
        public int CheckPaperCount { get; set; }
        public int UnderCollectionPaperCount { get; set; }
        public int EndorsementPaperCount { get; set; }
        public int DrawingUnderCollectionPaperCount { get; set; }
        public int ReturnedChequePaperCount { get; set; }
        public int PaymentUnderCollectionPaperCount { get; set; }
        public int ClearingDepositPaperCount { get; set; }
        public int CourtFundPaperCount { get; set; }
        public int PostdatedChequeCount { get; set; }
        public int ReturnPostdatedChequeCount { get; set; }
        public int ReturnPaidPostdatedChequeCount { get; set; }
        public string PaperAmount { get; set; }
        public string CheckedPaperAmount { get; set; }
        public string UnderCollectionPaperAmount { get; set; }
        public string EndorsementPaperAmount { get; set; }
        public string DrawingUnderCollectionPaperAmount { get; set; }
        public string ReturnedChequePaperAmount { get; set; }
        public string PaymentUnderCollectionPaperAmount { get; set; }
        public string ClearingDepositPaperAmount { get; set; }
        public string CourtFundPaperAmount { get; set; }
        public string PostdatedChequeAmount { get; set; }
        public string ReturnPostdatedChequeAmount { get; set; }
        public string ReturnPaidPostdatedChequeAmount { get; set; }
        public double ConversionFactor { get; set; }
        public int FCurrencyID { get; set; }
        [Display(Name = "CurrencyID", ResourceType = typeof(Resources.Resource))]
        public int CurrencyID { get; set; }
        public IEnumerable<Currency> Currency { get; set; }

        public IEnumerable<Paper> Paper { get; set; }
        public Header Header { get; set; }
        public Transaction TransactionDebit { get; set; }
        public Transaction TransactionCredit { get; set; }
        public Boolean WorkWithCostCenter { get; set; }
        public string UnderCollectionAccountName { get; set; }
        [Display(Name = "UnderCollectionBankAccountNumber", ResourceType = typeof(Resources.Resource))]
        public string UnderCollectionAccountNumber { get; set; }
        public double CurrencyNewValue { get; set; }
        public int ChequeYear { get; set; }
        public string sChequeDate { get; set; }
        public string ClearingDepositAccountName { get; set; }
        [Display(Name = "ClearingDepositBankAccountNumber", ResourceType = typeof(Resources.Resource))]
        public string ClearingDepositAccountNumber { get; set; }
        public string ChequeEndorsementAccountName { get; set; }
        [Display(Name = "ChequeEndorsementAccountNumber", ResourceType = typeof(Resources.Resource))]
        public string ChequeEndorsementAccountNumber { get; set; }
        public string EndorsementAccountName { get; set; }
        public string BankUnderCollectionAccountName { get; set; }
        public string BankUnderCollectionAccountNumber { get; set; }
        public string BankClearingDepositAccountName { get; set; }
        [Display(Name = "Note", ResourceType = typeof(Resources.Resource))]
        public string ReturnNote { get; set; }
        public string BankOrEndorsementAccountName { get; set; }
        [Display(Name = "CaseNumber", ResourceType = typeof(Resources.Resource))]
        public string CaseNumber { get; set; }
        [Display(Name = "CaseNumber", ResourceType = typeof(Resources.Resource))]
        public string UpdateCaseNumber { get; set; }
        public Boolean ChequeFund { get; set; }
        public Boolean FundDrawnFromUnderCollection { get; set; }
        public Boolean ReturnedChequeFundCheque { get; set; }
        public Boolean CourtFundCheque { get; set; }
        public Boolean UnderCollection { get; set; }
        public Boolean PaymentUnderCollection { get; set; }
        public Boolean ClearingDepositCheque { get; set; }
        public Boolean EndorsementCheque { get; set; }
        public Boolean PaymentEndorsement { get; set; }
        public Boolean ReturnDrawingChequeToCustomer { get; set; }
        public Boolean ReturnChequeToCustomer { get; set; }
        public Boolean CompromiseInCourt { get; set; }
        public Boolean ReturnFromChequeBox { get; set; }

        public Boolean PostdatedCheques { get; set; }
        public Boolean PaidPostdatedCheque { get; set; }
        public Boolean ReturnPostdatedCheque { get; set; }
        public Boolean ReturnPaidPostdatedCheque { get; set; }
        public int VHI { get; set; }
        public string PaidInAccountNumber { get; set; }
        public string BankOrEndorsementAccountNumber { get; set; }
        public string PaidInAccountName { get; set; }
        public string OriginalVoucherNumber { get; set; }
        public string OriginalCompanyTransactionKind { get; set; }
        public string CompanyTransactionKindName { get; set; }
        public string CaseName { get; set; }
        public int ChequeCase { get; set; }
        public int OriginalCompanyTransactionKindNo { get; set; }
        public int OriginalTransactionKindNo { get; set; }
        public string CaseDetail { get; set; }
        public string PostdatedAccountName { get; set; }
        public string PostdatedChequeName { get; set; }
        public string PaidToAccount { get; set; }
        public string PaidToAccountNumber { get; set; }
        public string BankAccountNumber { get; set; }
        public string PostdatedAccountNumber { get; set; }
        public string BankAccountName { get; set; }
        public string PaidToAccountName { get; set; }
        public int SaleID { get; set; }
        public string SaleName { get; set; }
        public int TheDecimalPointForTheLocalCurrency { get; set; }
        public int TheDecimalPointForTheForeignCurrency { get; set; }
        public string FromAccAccount { get; set; }
        public int CurrentYear { get; set; }

    }
}