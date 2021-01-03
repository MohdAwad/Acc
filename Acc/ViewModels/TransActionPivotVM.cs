using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.ViewModels
{
    public class TransActionPivotVM
    {
        public string AccountNumber { get; set; }
        public string Name { get; set; }


        public string CostCenterNumber { get; set; }
        public string CostCenterName { get; set; }
        public string AccountType { get; set; }

        public double Credit { get; set; }
        public double Debit { get; set; }
        public double NetTot { get; set; }



        public double DebitForeign { get; set; }
        public double CreditForeign { get; set; }
        public double NetForeign { get; set; }

        public string TransName { get; set; }

 
        public int CompanyTransactionKindNo { get; set; }

    
        public DateTime VoucherDate { get; set; }

        public string sVoucherDate { get; set; }


    }
}