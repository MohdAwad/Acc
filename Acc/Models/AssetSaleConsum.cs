using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Models
{
    public class AssetSaleConsum
    {
        public int Id { get; set; }
        public int CompanyID { get; set; }
        public int Type { get; set; }
        public int Serial { get; set; }
        public DateTime TrDate { get; set; }

        public double Ammount { get; set; }

        public double BookAmmount { get; set; }

        public string Note { get; set; }

        public int AssetID { get; set; }

        public string SoldBy { get; set; }


    }
}