using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Acc.ViewModels
{
    public class St_SalesManHVM
    {
        public int CompanyID { get; set; }
        [Display(Name = "SaleManID", ResourceType = typeof(Resources.Resource))]
        public int SalesManID { get; set; }
        [Display(Name = "ArabicName", ResourceType = typeof(Resources.Resource))]
        public string ArabicName { get; set; }
        [Display(Name = "EnglishName", ResourceType = typeof(Resources.Resource))]
        public string EnglishName { get; set; }
        [Display(Name = "JobNumber", ResourceType = typeof(Resources.Resource))]
        public string HrID { get; set; }
        [Display(Name = "InsUserName", ResourceType = typeof(Resources.Resource))]
        public string InsUserID { get; set; }
        [Display(Name = "InsDateTime", ResourceType = typeof(Resources.Resource))]
        public DateTime InsDateTime { get; set; }
        public Boolean ActiveInHR { get; set; }
        public string SalesManName { get; set; }
        public string HrIDCaseName { get; set; }
    }
}