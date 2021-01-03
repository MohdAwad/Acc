using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Acc.Models
{
    public class St_ItemGallary
    {

        public int Id { get; set; }
        public int CompanyID { get; set; }

        [Display(Name = "ItemCode", ResourceType = typeof(Resources.Resource))]
        public string ItemCode { get; set; }

        [Display(Name = "FileName", ResourceType = typeof(Resources.Resource))]
        public string FileName { get; set; }

       // public string MyProperty { get; set; }


    }
}