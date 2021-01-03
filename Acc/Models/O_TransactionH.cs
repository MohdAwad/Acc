using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Acc.Models
{
    public class O_TransactionH
    {
        public Company Company { get; set; }
        [Key]
        [Column(Order = 1)]
        public int CompanyID { get; set; }
        [Key]
        [Column(Order = 2)]
        public int CompanyYear { get; set; }
        [Key]
        [Column(Order = 3)]
        public int CompanyTransactionKindNo { get; set; }
        [Key]
        [Column(Order = 4)]
        public int TransactionKindNo { get; set; }
        [Display(Name = "OrderNumber", ResourceType = typeof(Resources.Resource))]
        public string OrderNumber { get; set; }
        [Key]
        [Column(Order = 5)]
        [Display(Name = "VoucherNumber", ResourceType = typeof(Resources.Resource))]
        public string VoucherNumber { get; set; }
        [Key]
        [Column(Order = 6)]
        public int RowNumber { get; set; }
        [Key]
        [Column(Order = 7)]
        public int IsDeleted { get; set; }
        [Key]
        [Column(Order = 8)]
        public string BranchCode { get; set; }

        [Display(Name = "VoucherNumber", ResourceType = typeof(Resources.Resource))]
        public int VHI { get; set; }

        [Display(Name = "VoucherDate", ResourceType = typeof(Resources.Resource))]
        public DateTime VoucherDate { get; set; }
        [Display(Name = "Debit", ResourceType = typeof(Resources.Resource))]
        public double Debit { get; set; }
        [Display(Name = "Credit", ResourceType = typeof(Resources.Resource))]
        public double Credit { get; set; }
        [Display(Name = "AccountNumber", ResourceType = typeof(Resources.Resource))]
        public string AccountNumber { get; set; }
        [Display(Name = "Note", ResourceType = typeof(Resources.Resource))]
        public string Note { get; set; }
        [Display(Name = "InsUserName", ResourceType = typeof(Resources.Resource))]
        public string InsUserID { get; set; }
        [Display(Name = "InsDateTime", ResourceType = typeof(Resources.Resource))]
        public DateTime InsDateTime { get; set; }
        public int CaseID { get; set; }
    }
}