using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Acc.ViewModels
{
    public class AssetTypeVM
    {
        public int iRow { get; set; }
        public int Id { get; set; }

        [Display(Name = "Serial", ResourceType = typeof(Resources.Resource))]
        public int AssetTypeID { get; set; }
        public int CompanyID { get; set; }

        [Display(Name = "Name", ResourceType = typeof(Resources.Resource))]
        public string Name { get; set; }

        [Display(Name = "ConsRatio", ResourceType = typeof(Resources.Resource))]
        public double ConsRatio { get; set; }
        [Display(Name = "InsUserName", ResourceType = typeof(Resources.Resource))]
        public string InsUserID { get; set; }
        [Display(Name = "InsDateTime", ResourceType = typeof(Resources.Resource))]
        public DateTime InsDateTime { get; set; }
        public int CompanyYear { get; set; }
        public int CurrentYear { get; set; }
    }
}