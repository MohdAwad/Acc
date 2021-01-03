using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Models
{
    public class O_ReceiptBillTransactionHistoryH
    {
        public int Id { get; set; }
        public int CompanyID { get; set; }
        public int CompanyYear { get; set; }
        public int CompanyTransactionKindNo { get; set; }
        public int TransactionKindNo { get; set; }
        public string StockCode { get; set; }
        public string BranchCode { get; set; }
        public string VoucherNumber { get; set; }
        public int VHI { get; set; }
        public DateTime VoucherDate { get; set; }
        public string OrderNumber { get; set; }
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