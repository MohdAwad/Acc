using Acc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.ViewModels
{
    public class TransServiceBillVM
    {
        public HeaderServiceBillVM HeaderServiceBill { get; set; }
        public IEnumerable<TransActionServiceBill> TransActionServiceBill { get; set; }
    }
}