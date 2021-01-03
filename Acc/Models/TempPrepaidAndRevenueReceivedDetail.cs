using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;


namespace Acc.Models
{
    public class TempPrepaidAndRevenueReceivedDetail
    {
        public Company Company { get; set; }
        [Key]
        [Column(Order = 1)]
        public int CompanyID { get; set; }
        [Key]
        [Column(Order = 2)]
        public int CompanyYear { get; set; }
        public CompanyTransactionKind CompanyTransactionKind { get; set; }
        [Key]
        [Column(Order = 3)]
        public int CompanyTransactionKindNo { get; set; }
        [Key]
        [Column(Order = 4)]
        public int TransactionKindNo { get; set; }
        [Key]
        [Column(Order = 5)]
        [Display(Name = "VoucherNumber", ResourceType = typeof(Resources.Resource))]
        public string VoucherNumber { get; set; }
        [Display(Name = "VoucherNumber", ResourceType = typeof(Resources.Resource))]
        public int VHI { get; set; }

        [Display(Name = "VoucherDate", ResourceType = typeof(Resources.Resource))]
        public DateTime VoucherDate { get; set; }
        [Display(Name = "Ammount", ResourceType = typeof(Resources.Resource))]
        public double Amount { get; set; }

        [Display(Name = "CollectionDate", ResourceType = typeof(Resources.Resource))]
        public DateTime CollectionDate { get; set; }
        
        [Display(Name = "FromAccountNumber", ResourceType = typeof(Resources.Resource))]
        public string FromAccountNumber { get; set; }
        [Display(Name = "ToAccountNumber", ResourceType = typeof(Resources.Resource))]
        public string ToAccountNumber { get; set; }

        [Display(Name = "FromCostCenter", ResourceType = typeof(Resources.Resource))]
        public string FromCostCenter { get; set; }
        [Display(Name = "ToCostCenter", ResourceType = typeof(Resources.Resource))]
        public string ToCostCenter { get; set; }
        [Display(Name = "Note", ResourceType = typeof(Resources.Resource))]
        public string Note { get; set; }
        [Key]
        [Column(Order = 6)]
        public int RowNumber { get; set; }
        public int Exported { get; set; }
        [Display(Name = "InsUserName", ResourceType = typeof(Resources.Resource))]
        public string InsUserID { get; set; }
        [Display(Name = "InsDateTime", ResourceType = typeof(Resources.Resource))]
        public DateTime InsDateTime { get; set; }
    }
}