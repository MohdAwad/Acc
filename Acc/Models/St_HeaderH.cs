using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Acc.Models
{
    public class St_HeaderH
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
        public string AccountNumber { get; set; }
        public int SaleID { get; set; }
        public double NetTotalLocalBeforDiscount { get; set; }
        public double NetTotalForeignBeforDiscount { get; set; }
        public double NetTotalLineDiscountLocal { get; set; }
        public double NetTotalLineDiscountForeign { get; set; }
        public double NetTotalLocalAfterLineDiscount { get; set; }
        public double NetTotalForeignAfterLineDiscount { get; set; }
        public double NetTotalTaxAfterLineDiscountLocal { get; set; }
        public double NetTotalTaxAfterLineDiscounForeign { get; set; }
        public double NetTotalAfterLineDiscountBeforDiscountAllLocal { get; set; }
        public double NetTotalAfterLineDiscountBeforDiscountAllForeign { get; set; }
        public double NetTotalDiscountLocal { get; set; }
        public double NetTotalDiscountForeign { get; set; }
        public double DiscountPercentage { get; set; }
        public double NetTotalLocalAfterDiscount { get; set; }
        public double NetTotalForeignAfterDiscount { get; set; }
        public double NetTotalTaxLocal { get; set; }
        public double NetTotalTaxForeign { get; set; }
        public double NetTotalLocal { get; set; }
        public double NetTotalForeign { get; set; }
        public string Remark { get; set; }
        public string Hint { get; set; }
        public int RowCount { get; set; }
        public int Exported { get; set; }
        public int CurrencyID { get; set; }
        public double ConversionFactor { get; set; }
        public double ConversionFactorAfterExpenses { get; set; }
        public int VoucherCase { get; set; }
        public double CashLocal { get; set; }
        public int CreditCardType1 { get; set; }
        public double CreditCardLocal1 { get; set; }
        public int CreditCardType2 { get; set; }
        public double CreditCardLocal2 { get; set; }
        public double ChequeLocal { get; set; }
        public double CreditLocal { get; set; }
        public double CashForeign { get; set; }
        public double CreditCardForeign1 { get; set; }
        public double CreditCardForeign2 { get; set; }
        public double ChequeForeign { get; set; }
        public double CreditForeign { get; set; }
        public DateTime DueDate { get; set; }
        public string OfferNumber { get; set; }
        public string OrderNumber { get; set; }
        public double LocalCost { get; set; }
        public double ForeignCost { get; set; }
        public string OriginalVoucherNumber { get; set; }
        public double OtherExpensesValueLocal { get; set; }
        public double OtherExpensesValueForeign { get; set; }
        public string OtherExpensesAccountNumber { get; set; }
        public string Website { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
        public string Mobile { get; set; }
        public string TeleFax { get; set; }
        public int CityID { get; set; }
        public int AreaID { get; set; }
        public string NextTo { get; set; }
        public string StreetName { get; set; }
        public string BuildingNo { get; set; }
        public string FloorNo { get; set; }
        public string KnownTo { get; set; }
        public string InsUserID { get; set; }
        public DateTime InsDateTime { get; set; }
        public string FromStockCode { get; set; }
        public string ToStockCode { get; set; }
    }
}