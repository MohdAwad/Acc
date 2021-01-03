using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Acc.Models
{
    public class O_PurchaseOrderHeaderH
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
        public int VHI { get; set; }
        public string BranchCode { get; set; }
        public DateTime VoucherDate { get; set; }
        public DateTime DueDate { get; set; }
        public string SaleOrder { get; set; }
        public string SupplierAccountNumber { get; set; }
        public double TotalQuantity { get; set; }
        public double NetTotal { get; set; }
        public string Remark { get; set; }
        public string Hint { get; set; }
        public int RowCount { get; set; }
        public int LinkWithVoucher { get; set; }
        public int Approval { get; set; }
        public int StopIt { get; set; }
        public string InsUserID { get; set; }
        public DateTime InsDateTime { get; set; }
        public string UpdateDeleteNote { get; set; }
    }
}