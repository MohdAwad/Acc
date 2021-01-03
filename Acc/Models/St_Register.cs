using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Acc.Models
{
    public class St_Register
    {
        [Key]
        public int RegisterID { get; set; }
        public int CompanyID { get; set; }
       
        public string RegisterName { get; set; }
        public string RegisterNote { get; set; }
        public int RegisterCase { get; set; }
    }
}