using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Acc.ViewModels
{
    public class TransactionTotVM
    {
        [Display(Name = "AccountNumber", ResourceType = typeof(Resources.Resource))]
        public string AccountNumber { get; set; }
        public DateTime VoucherDate { get; set; }
        [Display(Name = "Debit", ResourceType = typeof(Resources.Resource))]
        public double Debit { get; set; }
        [Display(Name = "Credit", ResourceType = typeof(Resources.Resource))]
        public double Credit { get; set; }
        [Display(Name = "DebitForeign", ResourceType = typeof(Resources.Resource))]
        public double DebitForeign { get; set; }
        [Display(Name = "CreditForeign", ResourceType = typeof(Resources.Resource))]
        public double CreditForeign { get; set; }
  
    }
}