using Acc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace Acc.ViewModels
{
    public class St_OfferPriceVM
    {
        public IEnumerable<St_OfferPriceTransaction> St_OfferPriceTransaction { get; set; }
        public int CompanyID { get; set; }
        public int CompanyYear { get; set; }
        public int CompanyTransactionKindNo { get; set; }
        public int TransactionKindNo { get; set; }
        public string VoucherNumber { get; set; }
        public int VHI { get; set; }
        public DateTime VoucherDate { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string CustomerAccountNumber { get; set; }
        public string CustomerAccountName { get; set; }
        public double TotalQuantity { get; set; }
        public double NetTotal { get; set; }
        public string sTotalQuantity { get; set; }
        public string sNetTotal { get; set; }
        public string Remark { get; set; }
        public string Hint { get; set; }
        public int TaxType { get; set; }
        public string TaxTypeName { get; set; }
        public int RowCount { get; set; }
        public int LinkWithVoucher { get; set; }
        public string LinkWithVoucherName { get; set; }
        public int CurrencyID { get; set; }
        public double ConversionFactor { get; set; }
        public int VoucherCase { get; set; }
        public string VoucherCaseName { get; set; }
        public string InsUserID { get; set; }
        public DateTime InsDateTime { get; set; }
        public int RowNumber { get; set; }
        public int IsDeleted { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public int ItemUnitNo { get; set; }
        public string ItemUnitName { get; set; }
        public string SimilarItemCode { get; set; }
        public double Quantity { get; set; }
        public double Price { get; set; }
        public double Total { get; set; }
        public double UsingQuantity { get; set; }
        public string LinkVoucherNumber { get; set; }
        public int TheDecimalPointForTheLocalCurrency { get; set; }
        public string UserName { get; set; }
    }
}