using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Models
{
    public class UserActiveYear
    {
        public int Id { get; set; }
        public int CompanyID { get; set; }
        public string UserID { get; set; }
        public int ActiveYear { get; set; }

    }
}