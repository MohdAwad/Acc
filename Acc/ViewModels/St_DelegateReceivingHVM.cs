using Acc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.ViewModels
{
    public class St_DelegateReceivingHVM
    {

        public Company Company { get; set; }
       
        public int CompanyID { get; set; }

        public int DelegateReceivingID { get; set; }

        public string ArabicName { get; set; }
     
        public string EnglishName { get; set; }
        public string Telephone { get; set; }
        public string HrID { get; set; }

        public string InsUserID { get; set; }
        public DateTime InsDateTime { get; set; }
        public Boolean ActiveInHR { get; set; }

        public string DelegateReceivingName { get; set; }
        public string HrIDCaseName { get; set; }
    }
}