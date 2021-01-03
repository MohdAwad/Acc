using Acc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace Acc.ViewModels
{
    public class St_TransActionFileVM
    {
        public int Id { get; set; }

        public int Year { get; set; }
        public string VoucherNumber { get; set; }//1
        public string CompanyTransactionKindNo { get; set; }
        public string TransactionKindNo { get; set; }//10

        public int CompanyId { get; set; }
        public IEnumerable<St_TransActionFile> TransActionType { get; set; }

    }
}