using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Acc.Models
{
    public class HeaderServiceBill
    {
        public Company Company { get; set; }
        [Key]
        [Column(Order = 1)]
        public int CompanyID { get; set; }
        [Key]
        [Column(Order = 2)]
        [Display(Name = "Serial", ResourceType = typeof(Resources.Resource))]
        public int BillID { get; set; }
        [Key]
        [Column(Order = 3)]
        public int TransactionKindNo { get; set; }
        [Key]
        [Column(Order = 4)]
        public int CompanyYear { get; set; }
        [Display(Name = "Date", ResourceType = typeof(Resources.Resource))]
        public DateTime BillDate { get; set; } 

        [Display(Name = "AccountNumber", ResourceType = typeof(Resources.Resource))]
        public string AccountNumber { get; set; }

        [Display(Name = "NoTax", ResourceType = typeof(Resources.Resource))]
        public bool NoTax { get; set; }
        public double ConversionFactor { get; set; }
        public int FCurrencyID { get; set; }

        [Display(Name = "AccountNumber", ResourceType = typeof(Resources.Resource))]
        public string DebitAccountNumber { get; set; }

        [Display(Name = "AccountNumber", ResourceType = typeof(Resources.Resource))]
        public string CreditAccountNumber { get; set; }

        [Display(Name = "TaxAccountNumber", ResourceType = typeof(Resources.Resource))]
        public string TaxAccountNumber { get; set; }

        [Display(Name = "DebitCostNumber", ResourceType = typeof(Resources.Resource))]
        public string DebitCostNumber { get; set; }

        [Display(Name = "CreditCostNumber", ResourceType = typeof(Resources.Resource))]
        public string CreditCostNumber { get; set; }

        [Display(Name = "TaxCostNumber", ResourceType = typeof(Resources.Resource))]
        public string TaxCostNumber { get; set; }

        [Display(Name = "Note", ResourceType = typeof(Resources.Resource))]
        public string Note { get; set; }
        [Display(Name = "Discount", ResourceType = typeof(Resources.Resource))]
        public double Discount { get; set; }

        [Display(Name = "Tax", ResourceType = typeof(Resources.Resource))]
        public double Tax { get; set; }

        [Display(Name = "Total", ResourceType = typeof(Resources.Resource))]
        public double Total { get; set; }

        [Display(Name = "NetTotal", ResourceType = typeof(Resources.Resource))]
        public double NetTotal { get; set; }
        [Display(Name = "TotalForeignAmount", ResourceType = typeof(Resources.Resource))]
        public double ForeignAmount { get; set; }
        [Display(Name = "NetTotalForeignAmount", ResourceType = typeof(Resources.Resource))]
        public double NetTotalForeignAmount { get; set; }
        [Display(Name = "ForeignAmountTax", ResourceType = typeof(Resources.Resource))]
        public double ForeignAmountTax { get; set; }
        public int CompanyTransactionKindNo { get; set; }

        public int SaleManNo { get; set; }


    }
}