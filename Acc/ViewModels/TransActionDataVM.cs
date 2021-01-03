using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Acc.ViewModels
{
    public class TransActionDataVM
    {

        [Display(Name = "AccountNumber", ResourceType = typeof(Resources.Resource))]
        public string AccountNumber { get; set; }

     
        [Display(Name = "ArabicName", ResourceType = typeof(Resources.Resource))]
        public string ArabicName { get; set; }

        [Display(Name = "EnglishName", ResourceType = typeof(Resources.Resource))]
        public string EnglishName { get; set; }



        [Display(Name = "VoucherNumber", ResourceType = typeof(Resources.Resource))]
        public string VoucherNumber { get; set; }


        [Display(Name = "VoucherNumber", ResourceType = typeof(Resources.Resource))]
        public int VHI { get; set; }



        [Display(Name = "VoucherDate", ResourceType = typeof(Resources.Resource))]
        public DateTime VoucherDate { get; set; }

        [Display(Name = "Debit", ResourceType = typeof(Resources.Resource))]
        public double Debit { get; set; }

        [Display(Name = "Credit", ResourceType = typeof(Resources.Resource))]
        public double Credit { get; set; }
        

        [Display(Name = "Note", ResourceType = typeof(Resources.Resource))]
        public string Note { get; set; }

        public int CompanyTransactionKindNo { get; set; }

        public int TransactionKindNo { get; set; }

        public double Value { get; set; }

        public string TransName { get; set; }

        public int iRowID { get; set; }




    }
}