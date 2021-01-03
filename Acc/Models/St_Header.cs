using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Acc.Models
{
    public class St_Header
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
        public string VoucherNumber { get; set; }
        public int VHI { get; set; }
        public DateTime VoucherDate { get; set; }
        public string AccountNumber { get; set; }
        public int SaleID { get; set; }
        public int TaxType { get; set; }
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
        public int DriverID { get; set; }
        public string OriginalVoucherNumber { get; set; }
        public double ShippingValueLocal { get; set; }
        public double ShippingValueForeign { get; set; }
        public string ShippingAccountNumber { get; set; }
        public string ShippingCostCenter { get; set; }
        public double OtherExpensesValueLocal { get; set; }
        public double OtherExpensesValueForeign { get; set; }
        public string OtherExpensesAccountNumber { get; set; }
        public string OtherExpensesCostCenter { get; set; }
        public double NetTotalOfGoodsLocal { get; set; }
        public double NetTotalOfGoodsForeign { get; set; }
        public double ExtraExpensesLocal1 { get; set; }
        public double ExtraExpensesLocal2 { get; set; }
        public double ExtraExpensesLocal3 { get; set; }
        public double ExtraExpensesLocal4 { get; set; }
        public double ExtraExpensesLocal5 { get; set; }
        public double ExtraExpensesLocal6 { get; set; }
        public double ExtraExpensesLocal7 { get; set; }
        public double ExtraExpensesLocal8 { get; set; }
        public double ExtraExpensesLocal9 { get; set; }
        public double ExtraExpensesLocal10 { get; set; }
        public double ExtraExpensesLocal11 { get; set; }
        public double ExtraExpensesLocal12 { get; set; }
        public double ExtraExpensesLocal13 { get; set; }
        public double ExtraExpensesLocal14 { get; set; }
        public double ExtraExpensesLocal15 { get; set; }
        public double ExtraExpensesLocal16 { get; set; }
        public double ExtraExpensesLocal17 { get; set; }
        public double ExtraExpensesLocal18 { get; set; }
        public double ExtraExpensesLocal19 { get; set; }
        public double ExtraExpensesLocal20 { get; set; }
        public double ExtraExpensesForeign1 { get; set; }
        public double ExtraExpensesForeign2 { get; set; }
        public double ExtraExpensesForeign3 { get; set; }
        public double ExtraExpensesForeign4 { get; set; }
        public double ExtraExpensesForeign5 { get; set; }
        public double ExtraExpensesForeign6 { get; set; }
        public double ExtraExpensesForeign7 { get; set; }
        public double ExtraExpensesForeign8 { get; set; }
        public double ExtraExpensesForeign9 { get; set; }
        public double ExtraExpensesForeign10 { get; set; }
        public double ExtraExpensesForeign11 { get; set; }
        public double ExtraExpensesForeign12 { get; set; }
        public double ExtraExpensesForeign13 { get; set; }
        public double ExtraExpensesForeign14 { get; set; }
        public double ExtraExpensesForeign15 { get; set; }
        public double ExtraExpensesForeign16 { get; set; }
        public double ExtraExpensesForeign17 { get; set; }
        public double ExtraExpensesForeign18 { get; set; }
        public double ExtraExpensesForeign19 { get; set; }
        public double ExtraExpensesForeign20 { get; set; }
        public double NetTotalExtraExpensesLocal { get; set; }
        public double NetTotalExtraExpensesForeign { get; set; }
        public string InsUserID { get; set; }
        public DateTime InsDateTime { get; set; }
        public string FromStockCode { get; set; }
        public string ToStockCode { get; set; }
    }
}