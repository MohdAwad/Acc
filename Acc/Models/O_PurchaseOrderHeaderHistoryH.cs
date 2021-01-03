using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Models
{
    public class O_PurchaseOrderHeaderHistoryH
    {
        public int Id { get; set; }
        public int CompanyID { get; set; }
        public int CompanyYear { get; set; }
        public int CompanyTransactionKindNo { get; set; }
        public int TransactionKindNo { get; set; }
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
        public int IsDelete { get; set; }
    }
}