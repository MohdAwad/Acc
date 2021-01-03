using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Acc.Models
{
    public class BankGuarantee
    {
        public Company Company { get; set; }
        [Key]
        [Column(Order = 1)]
        public int CompanyID { get; set; }
        [Key]
        [Column(Order = 2)]
        public int CompanyYear { get; set; }
        public CompanyTransactionKind CompanyTransactionKind { get; set; }
        [Key]
        [Column(Order = 3)]
        public int CompanyTransactionKindNo { get; set; }
        [Key]
        [Column(Order = 4)]
        public int TransactionKindNo { get; set; }
        [Key]
        [Column(Order = 5)]
        [Display(Name = "VoucherNumber", ResourceType = typeof(Resources.Resource))]
        public string VoucherNumber { get; set; }
        [Display(Name = "VoucherDate", ResourceType = typeof(Resources.Resource))]
        public DateTime VoucherDate { get; set; }
        [Display(Name = "WarrantyNumber", ResourceType = typeof(Resources.Resource))]
        public string WarrantyNumber { get; set; }
        [Display(Name = "DueDate", ResourceType = typeof(Resources.Resource))]
        public DateTime DueDate { get; set; }
        [Display(Name = "IssuedBy", ResourceType = typeof(Resources.Resource))]
        public string IssuedBy { get; set; }
        [Display(Name = "TheBeneficiary", ResourceType = typeof(Resources.Resource))]
        public string TheBeneficiary { get; set; }
        [Display(Name = "WarrantyAmount", ResourceType = typeof(Resources.Resource))]
        public double WarrantyAmount { get; set; }
        [Display(Name = "ExpensesAmount", ResourceType = typeof(Resources.Resource))]
        public double ExpensesAmount { get; set; }
        [Display(Name = "DepositValue", ResourceType = typeof(Resources.Resource))]
        public double DepositValue { get; set; }
        [Display(Name = "OrderNo", ResourceType = typeof(Resources.Resource))]
        public string OrderNo { get; set; }
        [Display(Name = "InsUserName", ResourceType = typeof(Resources.Resource))]
        public string InsUserID { get; set; }
        [Display(Name = "InsDateTime", ResourceType = typeof(Resources.Resource))]
        public DateTime InsDateTime { get; set; }
    }
}