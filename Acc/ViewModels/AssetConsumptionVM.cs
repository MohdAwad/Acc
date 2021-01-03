using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.ViewModels
{
    public class AssetConsumptionVM
    {
        public List<AssetVM> AssetData { get; set; }
        public DateTime ToDate { get; set; }

    }
}