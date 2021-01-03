using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.ViewModels
{
    public class TransActionDeleteVM
    {
      
        public int CompanyTransactionKindNo { get; set; }
 
        public int TransactionKindNo { get; set; }
        
        public string VoucherNumber { get; set; }

        public int CompanyYear { get; set; }

        public int CompanyID { get; set; }


    }
}