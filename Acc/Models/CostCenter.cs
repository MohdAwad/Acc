using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Acc.Models
{
    public class CostCenter
    {
        public Company Company { get; set; }
        [Key]
        [Column(Order = 1)]
        public int CompanyID { get; set; }
        [Key]
        [Column(Order = 2)]
        public int CompanyYear { get; set; }
        [Key]
        [Column(Order = 3)]

        [Display(Name = "CostNumber", ResourceType = typeof(Resources.Resource))]
        public string CostNumber { get; set; }

        [Display(Name = "ArabicName", ResourceType = typeof(Resources.Resource))]
        public string ArabicName { get; set; }

        [Display(Name = "EnglishName", ResourceType = typeof(Resources.Resource))]
        public string EnglishName { get; set; }

        [Display(Name = "CostLevel", ResourceType = typeof(Resources.Resource))]
        public int CostLevel { get; set; }

        [Display(Name = "CostFather", ResourceType = typeof(Resources.Resource))]
        public string CostFather { get; set; }

        [Display(Name = "CostFatherName", ResourceType = typeof(Resources.Resource))]
        public string CostFatherName { get; set; }

        [Display(Name = "StoppedCost", ResourceType = typeof(Resources.Resource))]
        public bool StoppedCost { get; set; }

        [Display(Name = "Note", ResourceType = typeof(Resources.Resource))]
        public string Note { get; set; }

        [Display(Name = "InsUserName", ResourceType = typeof(Resources.Resource))]
        public string InsUserID { get; set; }

        [Display(Name = "InsDateTime", ResourceType = typeof(Resources.Resource))]
        public DateTime InsDateTime { get; set; }

        [Display(Name = "IsFirstLevel", ResourceType = typeof(Resources.Resource))]
        public bool IsFirstLevel { get; set; }


        [Display(Name = "AccountChart", ResourceType = typeof(Resources.Resource))]
        public string CostChart { get; set; }

        [Display(Name = "AccountChart", ResourceType = typeof(Resources.Resource))]

        public string CostZero { get; set; }

        public string LevelZero { get; set; }

        public string OrignalCost{ get; set; }
    }
}