using Acc.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Acc.ViewModels
{
    public class DefinitionBankVM
    {
        public DefinitionBank DefinitionBank { get; set; }
        public string BankAccountName { get; set; }
        public string ChequeUnderCollectionAccountName { get; set; }
        public string PostdatedChequeAccountName { get; set; }
        public string BillsOfExchangeAccountName { get; set; }
        public Boolean StoppedAccountBank { get; set; }
        public Boolean StoppedAccountChequeUnderCollection { get; set; }
        public Boolean StoppedAccountPostdatedCheque { get; set; }
        public Boolean StoppedAccountBillsOfExchange { get; set; }
    }
}