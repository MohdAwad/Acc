using Acc.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Acc.ViewModels
{
    public class TransactionFixedVM
    {
        public IEnumerable<Paper> Paper { get; set; }
        public Header Header { get; set; }
        [Display(Name = "CompanyTransactionKindID", ResourceType = typeof(Resources.Resource))]
        public int CompanyTransactionKindID { get; set; }
        public IEnumerable<CompanyTransactionKind> CompanyTransactionKind { get; set; }
        public Transaction  TransactionDebit { get; set; }
        public Transaction TransactionCredit { get; set; }
        public IEnumerable<Transaction> TransactionCreditList { get; set; }
        public IEnumerable<Transaction> TransactionDebitList { get; set; }
        public Transaction TransactionTax { get; set; }

        [Display(Name = "CurrencyID", ResourceType = typeof(Resources.Resource))]
        public int CurrencyID { get; set; }

        [Display(Name = "ConversionFactor", ResourceType = typeof(Resources.Resource))]
        public double CurrencyNewValue { get; set; }
        public IEnumerable<Currency> Currency { get; set; }
        [Display(Name = "FromAccountNumber", ResourceType = typeof(Resources.Resource))]
        public string FromAccountNumber { get; set; }
        public string AccountDebitName { get; set; }
        [Display(Name = "ToAccountNumber", ResourceType = typeof(Resources.Resource))]
        public string ToAccountNumber { get; set; }
        public string AccountCreditName { get; set; }
        [Display(Name = "TaxAccountNumber", ResourceType = typeof(Resources.Resource))]
        public string TaxAccountNumber { get; set; }
        public string TaxAccountName { get; set; }
        [Display(Name = "FromCostCenter", ResourceType = typeof(Resources.Resource))]
        public string FromCostCenter { get; set; }
        public string CostCenterDebitName { get; set; }
        [Display(Name = "ToCostCenter", ResourceType = typeof(Resources.Resource))]
        public string ToCostCenter { get; set; }
        public string CostCenterCreditName { get; set; }
        [Display(Name = "TaxCostCenter", ResourceType = typeof(Resources.Resource))]
        public string TaxCostCenter { get; set; }
        public string TaxCostCenterName { get; set; }
        [Display(Name = "Amount", ResourceType = typeof(Resources.Resource))]
        public double Amount { get; set; }
        [Display(Name = "Tax", ResourceType = typeof(Resources.Resource))]
        public double Tax { get; set; }

        [Display(Name = "TotalAmount", ResourceType = typeof(Resources.Resource))]
        public double TotalAmount { get; set; }

        [Display(Name = "ForeignAmount", ResourceType = typeof(Resources.Resource))]
        public double ForeignAmount { get; set; }

        [Display(Name = "TotalForeign", ResourceType = typeof(Resources.Resource))]
        public double TotalForeign { get; set; }

        [Display(Name = "TaxForeign", ResourceType = typeof(Resources.Resource))]
        public double TaxForeign { get; set; }
        public bool WorkWithCostCenter { get; set; }
        public string CompanyTransactionKindName { get; set; }
        public string CurrencyName { get; set; }
        public int CompanyYear { get; set; }
        public int Export { get; set; }
        [Display(Name = "Sales", ResourceType = typeof(Resources.Resource))]
        public int SaleManNo { get; set; }
        public IEnumerable<Sale> SaleMan { get; set; }
        public string SaleManName { get; set; }
        public string BankAccountNumber { get; set; }
        public string BankAccountName { get; set; }
        public string ChequeNumber { get; set; }
        public DateTime ChequeDate { get; set; }
        public double ChequeAmount { get; set; }
        public string ChequeCaseName { get; set; }
        public string DrawerName { get; set; }
        public string  BankAccountNameJustForFill { get; set; }
        public string AccountNumberThird { get; set; }
        public string AccountNumberSecond { get; set; }
        public int RowNumber { get; set; }
        public bool Sirs { get; set; }
        public bool Mr { get; set; }
        public bool Co { get; set; }
        public bool First { get; set; }
        public bool Mrs { get; set; }
        public string PostdatedAccountNumber { get; set; }

        [Display(Name = "Amount", ResourceType = typeof(Resources.Resource))]
        public string sAmount { get; set; }
        [Display(Name = "Tax", ResourceType = typeof(Resources.Resource))]
        public string sTax { get; set; }

        [Display(Name = "TotalAmount", ResourceType = typeof(Resources.Resource))]
        public string sTotalAmount { get; set; }

        [Display(Name = "ForeignAmount", ResourceType = typeof(Resources.Resource))]
        public string sForeignAmount { get; set; }

        [Display(Name = "TotalForeign", ResourceType = typeof(Resources.Resource))]
        public string sTotalForeign { get; set; }

        [Display(Name = "TaxForeign", ResourceType = typeof(Resources.Resource))]
        public string sTaxForeign { get; set; }
        public double Credit { get; set; }
        public double Debit { get; set; }
        public double CreditForeign { get; set; }
        public double DebitForeign { get; set; }
        public string AccountNumber { get; set; }
        public string AccountName { get; set; }
        public string CostCenter { get; set; }
        public string CostCenterName { get; set; }
        public string Note { get; set; }
        public int TheDecimalPointForTheLocalCurrency { get; set; }
        public int TheDecimalPointForTheForeignCurrency { get; set; }
        public int CurrentYear { get; set; }

    }
}