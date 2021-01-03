using Acc.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Acc.ViewModels
{
    public class AssetFilterVM
    {
        public IEnumerable <AssetType> AssetType { get; set; }

        [Display(Name = "AssetType", ResourceType = typeof(Resources.Resource))]
        public int AssetTypeID { get; set; }


        [Display(Name = "FromDate", ResourceType = typeof(Resources.Resource))]
        public DateTime FromDate { get; set; }

        [Display(Name = "ToDate", ResourceType = typeof(Resources.Resource))]
        public DateTime ToDate { get; set; }
        [Display(Name = "AssetID", ResourceType = typeof(Resources.Resource))]
        public string AssetID { get; set; }
        [Display(Name = "Name", ResourceType = typeof(Resources.Resource))]
        public string AssetName { get; set; }

        public int TheDecimalPointForTheLocalCurrency { get; set; }
        public int TheDecimalPointForTheForeignCurrency { get; set; }

        public int CompanyYear { get; set; }

        public int CurrentYear { get; set; }



    }
}