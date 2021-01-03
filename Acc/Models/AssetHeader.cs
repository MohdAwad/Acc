using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Acc.Models
{
    public class AssetHeader
    {
        [Key]
        [Column(Order = 1)]
        [Required]
        public int CompanyID { get; set; }


        [Key]
        [Column(Order = 2)]
        [Required]
        public int VouchrNo { get; set; }

        public DateTime SaveDate { get; set; }

        public double Total { get; set; }


        public int Exported { get; set; }

        public DateTime VouchrDate { get; set; }
        

    }
}