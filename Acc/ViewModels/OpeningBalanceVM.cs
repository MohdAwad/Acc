using Acc.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Acc.ViewModels
{
    public class OpeningBalanceVM
    {
        public Header Header { get; set; }

        [Display(Name = "TransactionKind", ResourceType = typeof(Resources.Resource))]
        public int CompanyTransactionKindID { get; set; }
        public IEnumerable<CompanyTransactionKind> CompanyTransactionKind { get; set; }

        public IEnumerable<Transaction> Transaction { get; set; }

        [Display(Name = "Currency", ResourceType = typeof(Resources.Resource))]
        public int CurrencyID { get; set; }

        [Display(Name = "ConversionFactor", ResourceType = typeof(Resources.Resource))]
        public double CurrencyNewValue { get; set; }
        public IEnumerable<Currency> Currency { get; set; }


        [Display(Name = "AccountNumber", ResourceType = typeof(Resources.Resource))]
        public string AccountNumber { get; set; }

        [Required]
        [Display(Name = "ArabicName", ResourceType = typeof(Resources.Resource))]
        public string AccountName { get; set; }

    

        public double CreditDebitForeign { get; set; }



        [Display(Name = "OpeningBalance", ResourceType = typeof(Resources.Resource))]
        public double Debit { get; set; }

        [Display(Name = "OpeningBalance", ResourceType = typeof(Resources.Resource))]
        public double Credit { get; set; }

        public DateTime TransDate { get; set; }

        public int TransKind { get; set; }

        [Display(Name = "NetTotal", ResourceType = typeof(Resources.Resource))]
        public double NetTOT { get; set; }

        [Display(Name = "OpeningBalance", ResourceType = typeof(Resources.Resource))]
        public string sDebit { get; set; }

        [Display(Name = "TotalDebit", ResourceType = typeof(Resources.Resource))]
        public string TotalDebit { get; set; }
        [Display(Name = "TotalCredit", ResourceType = typeof(Resources.Resource))]
        public string TotalCredit { get; set; }
        [Display(Name = "Diff", ResourceType = typeof(Resources.Resource))]
        public string sNetTotal { get; set; }
        public int TheDecimalPointForTheLocalCurrency { get; set; }
        public int TheDecimalPointForTheForeignCurrency { get; set; }


        public bool CanUpdate { get; set; }




    }
}