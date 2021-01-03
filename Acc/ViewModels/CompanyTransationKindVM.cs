using Acc.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Acc.ViewModels
{
    public class CompanyTransationKindVM
    {
        public int CompanyID { get; set; }
        [Display(Name = "Serial", ResourceType = typeof(Resources.Resource))]
        public int CompanyTransactionKindID { get; set; }

        [Display(Name = "TransactionKind", ResourceType = typeof(Resources.Resource))]
        public int TransactionKindID { get; set; }
        [Display(Name = "Transaction", ResourceType = typeof(Resources.Resource))]
        public IEnumerable<TransactionKind> TransactionKind { get; set; }
        [Required]
        [Display(Name = "ArabicName", ResourceType = typeof(Resources.Resource))]
        public string ArabicName { get; set; }
        [Display(Name = "EnglishName", ResourceType = typeof(Resources.Resource))]
        public string EnglishName { get; set; }
        [Display(Name = "AutoSerial", ResourceType = typeof(Resources.Resource))]
        public Boolean AutoSerial { get; set; }
        [Display(Name = "InsUserName", ResourceType = typeof(Resources.Resource))]
        public string InsUserID { get; set; }
        [Display(Name = "InsDateTime", ResourceType = typeof(Resources.Resource))]
        public DateTime InsDateTime { get; set; }
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
        [Display(Name = "Example", ResourceType = typeof(Resources.Resource))]
        public string Example { get; set; }
        public string TransactionKindName { get; set; }
        public string CompanyTransactionKindName { get; set; }
        public string CaseSerial { get; set; }
    }
}