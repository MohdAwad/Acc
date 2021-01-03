using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Acc.Models
{
    public class St_ManufacturingStageH
    {
        public Company Company { get; set; }
        [Key]
        [Column(Order = 1)]
        public int CompanyID { get; set; }
        [Key]
        [Column(Order = 2)]
        [Display(Name = "ItemCode", ResourceType = typeof(Resources.Resource))]
        public string ItemCode { get; set; }
        [Key]
        [Column(Order = 3)]
        public int RowNumber { get; set; }
        [Display(Name = "FactoryName", ResourceType = typeof(Resources.Resource))]
        public int FactoryNo { get; set; }
        [Display(Name = "NumberOfDays", ResourceType = typeof(Resources.Resource))]
        public int NumberOfDays { get; set; }
    }
}