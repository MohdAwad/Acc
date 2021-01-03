using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Models
{
    public class FreezeTransaction
    {
        public int Id { get; set; }
        public int CompanyID { get; set; }
        public DateTime MinmumDate { get; set; }
        public DateTime MaximumDate { get; set; }
        public int MonthID { get; set; }
        public string InsUserID { get; set; }
        public DateTime InsDateTime { get; set; }
        public int CompanyYear { get; set; }
    }
}