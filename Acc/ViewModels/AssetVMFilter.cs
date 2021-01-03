using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Acc.ViewModels
{
    public class AssetVMFilter
    {
        [Display(Name = "AssetType", ResourceType = typeof(Resources.Resource))]
        public int? FAssetTypeID { get; set; }

        [Display(Name = "ToDate", ResourceType = typeof(Resources.Resource))]
        public DateTime ToDate { get; set; }
    }
}