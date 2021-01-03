using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Acc.Models

{

    public class CompanyTransactionKind

    {

        public Company Company { get; set; }

        [Key]

        [Column(Order = 1)]

        public int CompanyID { get; set; }

        [Key]

        [Column(Order = 2)]

        [Display(Name = "Serial", ResourceType = typeof(Resources.Resource))]

        public int CompanyTransactionKindID { get; set; }

        public TransactionKind TransactionKind { get; set; }

        [Key]

        [Column(Order = 3)]

        [Display(Name = "TransactionKind", ResourceType = typeof(Resources.Resource))]

        public int TransactionKindID { get; set; }

        [Required]

        [Display(Name = "ArabicName", ResourceType = typeof(Resources.Resource))]

        public string ArabicName { get; set; }

        [Display(Name = "EnglishName", ResourceType = typeof(Resources.Resource))]
        public string EnglishName { get; set; }
        [Display(Name = "AutoSerial", ResourceType = typeof(Resources.Resource))]
        public Boolean AutoSerial { get; set; }
        [Display(Name = "MonthlySerial", ResourceType = typeof(Resources.Resource))]
        public Boolean MonthlySerial { get; set; }
        [Display(Name = "Symbol", ResourceType = typeof(Resources.Resource))]
        public string Symbol { get; set; }
        [Display(Name = "Year", ResourceType = typeof(Resources.Resource))]
        public int Year { get; set; }
        [Display(Name = "Month", ResourceType = typeof(Resources.Resource))]
        public int Month { get; set; }
        [Display(Name = "Serial", ResourceType = typeof(Resources.Resource))]
        public int Serial { get; set; }

        [Display(Name = "InsUserName", ResourceType = typeof(Resources.Resource))]

        public string InsUserID { get; set; }

        [Display(Name = "InsDateTime", ResourceType = typeof(Resources.Resource))]

        public DateTime InsDateTime { get; set; }

    }

}