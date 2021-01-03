using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Acc.Models
{
    public class O_DailyCloseH
    {
        public int Id { get; set; }
        public int CompanyID { get; set; }
        public string BranchCode { get; set; }
        [Display(Name = "Today", ResourceType = typeof(Resources.Resource))]
        public DateTime DailyClose { get; set; }
        [Display(Name = "InsUserName", ResourceType = typeof(Resources.Resource))]
        public string InsUserID { get; set; }
        [Display(Name = "InsDateTime", ResourceType = typeof(Resources.Resource))]
        public DateTime InsDateTime { get; set; }
    }
}