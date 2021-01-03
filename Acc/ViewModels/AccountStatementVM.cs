using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.ViewModels
{
    public class AccountStatementVM
    {
        public DateTime TransDate { get; set; }
        public string TransName { get; set; }
        public double Debit { get; set; }

        public double Credit { get; set; }

        public double Balance { get; set; }

        public string Statment { get; set; }



        public string sBalance { get; set; }
        public string sDebit { get; set; }

        public string sCredit { get; set; }


        public int TransactionKindID { get; set; }
        public int CompanyTransactionKindID { get; set; }

        public int VHFI { get; set; }
        public string VoucherNumber { get; set; }
    }
}