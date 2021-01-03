﻿using Acc.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Acc.ViewModels
{
    public class St_HeaderVM
    {
        public IEnumerable<Currency> Currency { get; set; }
        public IEnumerable<Sale> SaleMan { get; set; }
        public IEnumerable<St_Warehouse> St_Warehouse { get; set; }
        public IEnumerable<St_Warehouse> St_WarehouseFrom { get; set; }
        public IEnumerable<St_Warehouse> St_WarehouseTo { get; set; }
        public IEnumerable<St_Transaction> St_Transaction { get; set; }
        public IEnumerable<St_Transaction> St_TransactionFrom { get; set; }
        public IEnumerable<St_Transaction> St_TransactionTo { get; set; }
        public int CompanyID { get; set; }
        public int CompanyYear { get; set; }
        public int CompanyTransactionKindNo { get; set; }
        public int TransactionKindNo { get; set; }
        public string StockCode { get; set; }
        public string FromStockCode { get; set; }
        public string ToStockCode { get; set; }
        public string VoucherNumber { get; set; }
        public int VHI { get; set; }
        public DateTime VoucherDate { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string AccountNumber { get; set; }
        public string AccountName { get; set; }
        public int SaleID { get; set; }
        public string SaleName { get; set; }
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
        public string sNetTotalLocalAfterLineDiscount { get; set; }
        public string sNetTotalDiscountLocal { get; set; }
        public string sNetTotalLocalAfterDiscount { get; set; }
        public string sNetTotalTaxLocal { get; set; }
        public string sNetTotalLocal { get; set; }
        public string sDiscountPercentage { get; set; }
        public string sNetTotalForeignAfterLineDiscount { get; set; }
        public string sNetTotalDiscountForeign { get; set; }
        public string sNetTotalForeignAfterDiscount { get; set; }
        public string sNetTotalTaxForeign { get; set; }
        public string sNetTotalForeign { get; set; }
        public string sDiscountPercentageForeign { get; set; }
        public string Remark { get; set; }
        public string Hint { get; set; }
        public int RowCount { get; set; }
        public int Exported { get; set; }
        public string Export { get; set; }
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
        public string DebitAccountNumber { get; set; }
        public string CreditAccountNumber { get; set; }
        public string TaxAccountNumber { get; set; }
        public string DebitCostNumber { get; set; }
        public string CreditCostNumber { get; set; }
        public string TaxCostNumber { get; set; }
        public string DebitAccountName { get; set; }
        public string CreditAccountName { get; set; }
        public string TaxAccountName { get; set; }
        public string DebitCostName { get; set; }
        public string CreditCostName { get; set; }
        public string TaxCostName { get; set; }
        public string OrignailTaxAccountNumber { get; set; }
        public string OrignailTaxAccountName { get; set; }
        public bool WorkWithCostCenter { get; set; }
        public int TheDecimalPointForTheLocalCurrency { get; set; }
        public int TheDecimalPointForTheForeignCurrency { get; set; }
        public int IsDeleted { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public int ItemUnitNo { get; set; }
        public string ItemUnitName { get; set; }
        public string SimilarItemCode { get; set; }
        public double Quantity { get; set; }
        public double Bonus { get; set; }
        public double QuantityInputOutput { get; set; }
        public double BonusInputOutput { get; set; }
        public double TaxRate { get; set; }
        public double TotalLocalBeforDiscount { get; set; }
        public double TotalForeignBeforDiscount { get; set; }
        public double TotalLineDiscountLocal { get; set; }
        public double TotalLineDiscountForeign { get; set; }
        public double TotalLocalAfterLineDiscount { get; set; }
        public double TotalForeignAfterLineDiscount { get; set; }
        public double LineDiscountPercentage { get; set; }
        public double TotalTaxAfterLineDiscountLocal { get; set; }
        public double TotalTaxAfterLineDiscounForeign { get; set; }
        public double TotalAfterLineDiscountBeforDiscountAllLocal { get; set; }
        public double TotalAfterLineDiscountBeforDiscountAllForeign { get; set; }
        public double TotalDiscountLocal { get; set; }
        public double TotalDiscountForeign { get; set; }
        public double TotalLocalAfterDiscount { get; set; }
        public double TotalForeignAfterDiscount { get; set; }
        public double TotalTaxLocal { get; set; }
        public double TotalTaxForeign { get; set; }
        public double TotalLocal { get; set; }
        public double TotalForeign { get; set; }
        public double TotalCostLocal { get; set; }
        public double TotalCostForeign { get; set; }
        public double PricePieceLocalBeforDiscount { get; set; }
        public double PricePieceForeignBeforDiscount { get; set; }
        public double PricePieceLineDiscountLocal { get; set; }
        public double PricePieceLineDiscountForeign { get; set; }
        public double PricePieceLocalAfterLineDiscount { get; set; }
        public double PricePieceForeignAfterLineDiscount { get; set; }
        public double PricePieceTaxAfterLineDiscountLocal { get; set; }
        public double PricePieceTaxAfterLineDiscounForeign { get; set; }
        public double PricePieceAfterLineDiscountBeforDiscountAllLocal { get; set; }
        public double PricePieceAfterLineDiscountBeforDiscountAllForeign { get; set; }
        public double PricePieceDiscountLocal { get; set; }
        public double PricePieceDiscountForeign { get; set; }
        public double PricePieceLocalAfterDiscount { get; set; }
        public double PricePieceForeignAfterDiscount { get; set; }
        public double PricePieceTaxLocal { get; set; }
        public double PricePieceTaxForeign { get; set; }
        public double PricePieceTotalLocal { get; set; }
        public double PricePieceTotalForeign { get; set; }
        public double CostPieceLocal { get; set; }
        public double CostPieceForeign { get; set; }
        public DateTime ExpierDate { get; set; }
        public string BatchNumber { get; set; }
        public string UserName { get; set; }
        public string StockName { get; set; }
        public string TaxTypeName { get; set; }
        public string VoucherCaseName { get; set; }
        public int CheckCase { get; set; }
        public int RowNumber { get; set; }
        public int iRowTable { get; set; }
        public int TransferType { get; set; }
        public int Categorie_1 { get; set; }
        public string TransferTypeName { get; set; }

    }
}