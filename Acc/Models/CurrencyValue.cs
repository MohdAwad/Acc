using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Acc.Models
{
    public class CurrencyValue
    {
        public Company Company { get; set; }
        [Key]
        [Column(Order = 1)]
        public int CompanyID { get; set; }
        public Currency Currency { get; set; }
        [Key]
        [Column(Order = 2)]
        [Required]
        [Display(Name = "Currency", ResourceType = typeof(Resources.Resource))]
        public int  CurrencyID { get; set; }
        [Display(Name = "ConversionFactor", ResourceType = typeof(Resources.Resource))]
        public double ConversionFactor { get; set; }
        [Display(Name = "InsUserName", ResourceType = typeof(Resources.Resource))]
        public string InsUserID { get; set; }
        [Key]
        [Column(Order = 3)]
        [Display(Name = "InsDateTime", ResourceType = typeof(Resources.Resource))]
        public DateTime InsDateTime { get; set; }
    }
}