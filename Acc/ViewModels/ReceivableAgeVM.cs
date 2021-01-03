using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Acc.ViewModels
{
    public class ReceivableAgeVM
    {
        
        [Display(Name = "AccountNumber", ResourceType = typeof(Resources.Resource))]
        public string AccountNumber { get; set; }

         
        [Display(Name = "AccountName", ResourceType = typeof(Resources.Resource))]
        public string AccountName { get; set; }

        [Display(Name = "NetTotal", ResourceType = typeof(Resources.Resource))]
        public double NetTotal { get; set; }


        public string Col1 { get; set; }
        public string Col2 { get; set; }
        public string Col3 { get; set; }
        public string Col4 { get; set; }
        public string Col5 { get; set; }


        public string sCol1 { get; set; }
        public string sCol2 { get; set; }
        public string sCol3 { get; set; }
        public string sCol4 { get; set; }
        public string sCol5 { get; set; }

    }
}