using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Acc.ViewModels
{
    public class TransferAccountVM
    {
        [Display(Name = "FromAccount", ResourceType = typeof(Resources.Resource))]
        public string FromAccountNumber { get; set; }

        [Display(Name = "AccountName", ResourceType = typeof(Resources.Resource))]
        public string FromAccountName{ get; set; }

        [Display(Name = "ToAccount", ResourceType = typeof(Resources.Resource))]
        public string ToAccountNumber { get; set; }

        [Display(Name = "AccountName", ResourceType = typeof(Resources.Resource))]
        public string ToAccountName{ get; set; }
    }
}