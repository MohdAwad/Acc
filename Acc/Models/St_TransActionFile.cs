using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Acc.Models
{
    public class St_TransActionFile
    {
        public int Id { get; set; }

        [Display(Name = "VoucherNumber", ResourceType = typeof(Resources.Resource))]
        public string VoucherNumber { get; set; }//1
        public string CompanyTransactionKindNo { get; set; }
        public string TransactionKindNo { get; set; }//10

        [Display(Name = "FileName", ResourceType = typeof(Resources.Resource))]
        public string FileName { get; set; }

        public int Year { get; set; }

        public int CompanyId { get; set; }

        public string DocPath { get; set; }

        public string DocName { get; set; }
    }
}