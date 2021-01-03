using Acc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.ViewModels
{
    public class ResettingAccountsVM
    {

        public IEnumerable<AccountType> AccountType { get; set; }
        public int FromYear { get; set; }
        public int ToYear { get; set; }
        public string AccountNumber { get; set; }
        public string AccountName { get; set; }
        public int TheDecimalPointForTheForeignCurrency { get; set; }
        public int TheDecimalPointForTheLocalCurrency { get; set; }

    }
}