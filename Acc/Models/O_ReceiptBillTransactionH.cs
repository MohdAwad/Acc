using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Acc.Models
{
    public class O_ReceiptBillTransactionH
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
        [Key]
        [Column(Order = 8)]
        public int RowNumber { get; set; }
        public string BillNumber { get; set; }
        public DateTime BillDate { get; set; }
        public double BillAmount { get; set; }
        public int BillStatus { get; set; }
        public int BillCase { get; set; }
        public string PersonName { get; set; }
        public string InsUserID { get; set; }
        public DateTime InsDateTime { get; set; }
        public string sBillDate { get; set; }

    }
}