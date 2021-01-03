using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Acc.ViewModels
{
    public class ReceivableAgeFilter
    {
        public int DebitCredit { get; set; }

        [Display(Name = "AccountNumber", ResourceType = typeof(Resources.Resource))]
        public string AccountNumber { get; set; }


        [Display(Name = "AccountName", ResourceType = typeof(Resources.Resource))]
        public string AccountName { get; set; }

        [Display(Name = "ToDate", ResourceType = typeof(Resources.Resource))]
        public DateTime ToDate { get; set; }

        [Display(Name = "NumberOfDays", ResourceType = typeof(Resources.Resource))]
        public int NumberOfDays { get; set; }



    }
}