using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.ViewModels
{
    public class MutilUseSearchVM
    {
        public string AccountNumber { get; set; }
        public string AccountName { get; set; }
        public string CostNumber { get; set; }
        public string CostName { get; set; }
        public Boolean StoppedAccount { get; set; }
        public Boolean StoppedCost { get; set; }
        public string EmployeeId { get; set; }
        public string EmployeeFullName { get; set; }
        

    }
}