using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.ViewModels
{
    public class AccountStatementHeaderVM
    {
        public double   TotalDebit { get; set; }
        public double TotalCredit { get; set; }
        public double NetTotal { get; set; }
        public double TotalUnpaidChequesReceived { get; set; }
        public double TotalUnpaidChequesPayment { get; set; }
        public double TotalUnpaidBill { get; set; }
        public string sTotalDebit { get; set; }
        public string sTotalCredit { get; set; }
        public string sNetTotal { get; set; }
        public string sTotalCustomerCheque { get; set; }
        public string sTotalSupplierCheque { get; set; }
        public string sUnpaidBill { get; set; }
        public IEnumerable <AccountStatementVM> AccountStatementVM { get; set; }
    }
}