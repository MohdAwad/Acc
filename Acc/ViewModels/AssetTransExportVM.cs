using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.ViewModels
{
    public class AssetTransExportVM
    {
        public int VouchrNo { get; set; }
        public int CoTranKind { get; set; }

        public string AccountNo { get; set; }
        public string CostCenter { get; set; }

        public int PostedEntry { get; set; }
        public DateTime TransDate { get; set; }

    }
}