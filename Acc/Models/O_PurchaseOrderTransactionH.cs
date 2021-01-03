using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Acc.Models
{
    public class O_PurchaseOrderTransactionH
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
        public string VoucherNumber { get; set; }
        [Key]
        [Column(Order = 6)]
        public int RowNumber { get; set; }
        [Key]
        [Column(Order = 7)]
        public int IsDeleted { get; set; }
        public int VHI { get; set; }
        public string BranchCode { get; set; }
        public DateTime VoucherDate { get; set; }
        public string ItemCode { get; set; }
        public double Quantity { get; set; }
        public double Price { get; set; }
        public double Total { get; set; }
        public int LinkWithVoucher { get; set; }
        public string LinkVoucherNumber { get; set; }
        public double UsingQuantity { get; set; }
        public string InsUserID { get; set; }
        public DateTime InsDateTime { get; set; }
    }
}