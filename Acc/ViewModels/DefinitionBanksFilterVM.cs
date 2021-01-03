using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Acc.ViewModels
{
    public class DefinitionBanksFilterVM
    {
        public int BankID { get; set; }
        [Display(Name = "AccountNumber", ResourceType = typeof(Resources.Resource))]
        public string AccountNumber { get; set; }
        public string AccountName { get; set; }
        public string BankAccountName { get; set; }
        public string ChequeUnderCollectionAccountName { get; set; }
        public string PostdatedChequeAccountName { get; set; }
        public string BillsOfExchangeAccountName { get; set; }
        public string BankAccountNumber { get; set; }
        public string ChequeUnderCollectionAccountNumber { get; set; }
        public string PostdatedChequeAccountNumber { get; set; }
        public string BillsOfExchangeAccountNumber { get; set; }
        public string UserName { get; set; }
        public Boolean StoppedAccountBank { get; set; }
        public Boolean StoppedAccountChequeUnderCollection { get; set; }
        public Boolean StoppedAccountPostdatedCheque { get; set; }
        public Boolean StoppedAccountBillsOfExchange { get; set; }
    }
}