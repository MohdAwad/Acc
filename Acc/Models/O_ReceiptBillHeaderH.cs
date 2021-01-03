using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Acc.Models
{
    public class O_ReceiptBillHeaderH
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
        [Key]
        [Column(Order = 5)]
        public string StockCode { get; set; }
        [Key]
        [Column(Order = 6)]
        public string BranchCode { get; set; }
        [Key]
        [Column(Order = 7)]
        public string VoucherNumber { get; set; }
        public int VHI { get; set; }
        public DateTime VoucherDate { get; set; }
        public string OrderNumber { get; set; }
        public string CustomerAccountNumber { get; set; }
        public string BillAccountNumber { get; set; }
        public double NetTotal { get; set; }
        public string Note { get; set; }
        public int Exported { get; set; }
        [Display(Name = "InsUserName", ResourceType = typeof(Resources.Resource))]
        public string InsUserID { get; set; }
        [Display(Name = "InsDateTime", ResourceType = typeof(Resources.Resource))]
        public DateTime InsDateTime { get; set; }
    }
}