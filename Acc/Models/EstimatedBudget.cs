using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Acc.Models
{
    public class EstimatedBudget
    {
        public int Id { get; set; }
      
        public int CompanyID { get; set; }
       
        public int CompanyYear { get; set; }
       
        [Display(Name = "AccountNumber", ResourceType = typeof(Resources.Resource))]
        public string AccountNumber { get; set; }
        [Display(Name = "AccountName", ResourceType = typeof(Resources.Resource))]
        public string AccountName { get; set; }

        [Display(Name = "CostCenter", ResourceType = typeof(Resources.Resource))]
        public string CostCenterNumber { get; set; }
        [Display(Name = "CostCenterName", ResourceType = typeof(Resources.Resource))]
        public string CostName { get; set; }

        [Display(Name = "BudgetType", ResourceType = typeof(Resources.Resource))]
        public int BudgetType { get; set; }

        [Display(Name = "YearBudget", ResourceType = typeof(Resources.Resource))]
        public double YearBudget { get; set; }

        [Display(Name = "January", ResourceType = typeof(Resources.Resource))]
        public double January { get; set; }

        [Display(Name = "February", ResourceType = typeof(Resources.Resource))]
        public double February { get; set; }

        [Display(Name = "March", ResourceType = typeof(Resources.Resource))]
        public double March { get; set; }

        [Display(Name = "April", ResourceType = typeof(Resources.Resource))]
        public double April { get; set; }

        [Display(Name = "May", ResourceType = typeof(Resources.Resource))]
        public double May { get; set; }

        [Display(Name = "June", ResourceType = typeof(Resources.Resource))]
        public double June { get; set; }

        [Display(Name = "July", ResourceType = typeof(Resources.Resource))]
        public double July { get; set; }

        [Display(Name = "August", ResourceType = typeof(Resources.Resource))]
        public double August { get; set; }

        [Display(Name = "September", ResourceType = typeof(Resources.Resource))]
        public double September { get; set; }

        [Display(Name = "October", ResourceType = typeof(Resources.Resource))]
        public double October { get; set; }

        [Display(Name = "November", ResourceType = typeof(Resources.Resource))]
        public double November { get; set; }

        [Display(Name = "December", ResourceType = typeof(Resources.Resource))]
        public double December { get; set; }

        public bool Yeardividingitbymonths { get; set; }


    }
}