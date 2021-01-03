using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Models
{
    public class O_ReceiptBillHeaderHistoryH
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
        public string CustomerAccountNumber { get; set; }
        public string BillAccountNumber { get; set; }
        public double NetTotal { get; set; }
        public string Note { get; set; }
        public int Exported { get; set; }
        public string InsUserID { get; set; }
        public DateTime InsDateTime { get; set; }
    }
}