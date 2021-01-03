using Acc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.ViewModels
{
    public class AccCustomerInfoVM
    {
        public CustomerInformation CustomerInformation { get; set; }

        public ChartOfAccountVM ChartOfAccountVM { get; set; }
    }
}