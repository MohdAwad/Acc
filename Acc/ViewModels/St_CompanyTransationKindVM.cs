using Acc.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Acc.ViewModels
{
    public class St_CompanyTransationKindVM
    {
        public int CompanyID { get; set; }
        [Display(Name = "Serial", ResourceType = typeof(Resources.Resource))]
        public int St_CompanyTransactionKindID { get; set; }

        [Display(Name = "TransactionKind", ResourceType = typeof(Resources.Resource))]
        public int St_TransactionKindID { get; set; }

        [Display(Name = "StockCode", ResourceType = typeof(Resources.Resource))]
        public string StockCode { get; set; }
        [Display(Name = "AutoSerial", ResourceType = typeof(Resources.Resource))]
        public Boolean AutoSerial { get; set; }
        [Display(Name = "SymbolSerial", ResourceType = typeof(Resources.Resource))]
        public Boolean SymbolSerial { get; set; }
        [Display(Name = "Symbol", ResourceType = typeof(Resources.Resource))]
        public string Symbol { get; set; }
        [Display(Name = "Serial", ResourceType = typeof(Resources.Resource))]
        public int Serial { get; set; }
        [Display(Name = "InsUserName", ResourceType = typeof(Resources.Resource))]
        public string InsUserID { get; set; }
        [Display(Name = "InsDateTime", ResourceType = typeof(Resources.Resource))]
        public DateTime InsDateTime { get; set; }
        public string TransactionKindName { get; set; }
        public string WarehouseName { get; set; }
        [Display(Name = "Transaction", ResourceType = typeof(Resources.Resource))]
        public IEnumerable<St_TransactionKind> St_TransactionKind { get; set; }

        [Display(Name = "Example", ResourceType = typeof(Resources.Resource))]
        public string Example { get; set; }
        public int iRowTable { get; set; }
        public string CaseSerial { get; set; }
    }
}