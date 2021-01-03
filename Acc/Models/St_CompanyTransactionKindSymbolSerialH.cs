using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Models
{
    public class St_CompanyTransactionKindSymbolSerialH
    {
        public int Id { get; set; }
        public int CompanyID { get; set; }
        public string StockCode { get; set; }
        public int St_CompanyTransactionKindID { get; set; }
        public int LastSerial { get; set; }
    }
}