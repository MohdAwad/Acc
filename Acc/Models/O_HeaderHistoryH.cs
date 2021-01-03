using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Acc.Models
{
    public class O_HeaderHistoryH
    {
        public int Id { get; set; }
        public int CompanyID { get; set; }
        public int CompanyYear { get; set; }
        public int CompanyTransactionKindNo { get; set; }
        public int TransactionKindNo { get; set; }
        public string BranchCode { get; set; }
        [Display(Name = "OrderNumber", ResourceType = typeof(Resources.Resource))]
        public string OrderNumber { get; set; }
        [Display(Name = "VoucherNumber", ResourceType = typeof(Resources.Resource))]
        public string VoucherNumber { get; set; }

        [Display(Name = "VoucherNumber", ResourceType = typeof(Resources.Resource))]
        public int VHI { get; set; }

        [Display(Name = "Date", ResourceType = typeof(Resources.Resource))]
        public DateTime VoucherDate { get; set; }

        [Display(Name = "TotalDebit", ResourceType = typeof(Resources.Resource))]
        public double TotalDebit { get; set; }
        [Display(Name = "TotalCredit", ResourceType = typeof(Resources.Resource))]
        public double TotalCredit { get; set; }
        [Display(Name = "Statement", ResourceType = typeof(Resources.Resource))]
        public string Note { get; set; }
        public int RowCount { get; set; }
        public int TransferToAcc { get; set; }
        public int IsBill { get; set; }
        public int PaymethodTypeNo { get; set; }
        public string CustomerNo { get; set; }
        public string BillNumber { get; set; }
        [Display(Name = "InsUserName", ResourceType = typeof(Resources.Resource))]
        public string InsUserID { get; set; }
        [Display(Name = "InsDateTime", ResourceType = typeof(Resources.Resource))]
        public DateTime InsDateTime { get; set; }
        [Display(Name = "Note", ResourceType = typeof(Resources.Resource))]
        public string UpdateDeleteNote { get; set; }
        public int IsDelete { get; set; }
        public int VoucherTypeNo { get; set; }
        public int CaseID { get; set; }
    }
}