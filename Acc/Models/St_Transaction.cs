using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Acc.Models
{
    public class St_Transaction
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
        [Key]
        [Column(Order = 7)]
        public int RowNumber { get; set; }
        [Key]
        [Column(Order = 8)]
        public int IsDeleted { get; set; }
        public int VHI { get; set; }
        public DateTime VoucherDate { get; set; }
        public string ItemCode { get; set; }
        public string SimilarItemCode { get; set; }
        public double Quantity { get; set; }
        public double Bonus { get; set; }
        public double QuantityInputOutput { get; set; }
        public double BonusInputOutput { get; set; }
        public double TaxRate { get; set; }
        public int TaxType { get; set; }
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
        public double DiscountPercentage { get; set; }
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
        public string Remark { get; set; }
        public string Hint { get; set; }
        public DateTime ExpierDate { get; set; }
        public string BatchNumber { get; set; }
        public string InsUserID { get; set; }
        public DateTime InsDateTime { get; set; }
        public int TransferType { get; set; }
        public int Categorie_1 { get; set; }

    }
}