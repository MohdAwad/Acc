using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Models
{
    public class CompanyTransactionKindMonthlySerial
    {
        public int Id { get; set; }
        public int CompanyID { get; set; }
        public int CompanyTransactionKindID { get; set; }
        public int MonthID { get; set; }
        public int LastSerial { get; set; }
    }
}