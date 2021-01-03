using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.ViewModels
{
    public class ProfitLossVM
    {
        public double SALES { get; set; }
        public double PURCHASES { get; set; }
        public double GOODSSTART { get; set; }
        public double GOODSEND { get; set; }
        public double SALESCOST { get; set; }

        public double SALESEXPENSES { get; set; }

        public double GrossProfit { get; set; }
        public double OTHEREARNING { get; set; }
        public double ADMINISTRATIONEXPENSES { get; set; }
        public double FINANCIALEXPENSES  { get; set; }
        public double NetTotal { get; set; }
   

    }
}