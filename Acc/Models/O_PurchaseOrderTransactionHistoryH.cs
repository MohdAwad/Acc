using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Acc.Models
{
    public class O_PurchaseOrderTransactionHistoryH
    {
        public int Id { get; set; }
        public int CompanyID { get; set; }
        public int CompanyYear { get; set; }
        public int CompanyTransactionKindNo { get; set; }
        public int TransactionKindNo { get; set; }
        public string VoucherNumber { get; set; }
        public int RowNumber { get; set; }
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
        public int IsDelete { get; set; }
    }
}