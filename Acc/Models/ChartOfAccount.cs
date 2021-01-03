using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Acc.Models
{
    public class ChartOfAccount
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

        [Required]
        [Display(Name = "AccountNumber", ResourceType = typeof(Resources.Resource))]
        public string AccountNumber { get; set; }

        [Required]
        [Display(Name = "ArabicName", ResourceType = typeof(Resources.Resource))]
        public string ArabicName { get; set; }

        [Display(Name = "EnglishName", ResourceType = typeof(Resources.Resource))]
        public string EnglishName { get; set; }

        [Display(Name = "AccountLevel", ResourceType = typeof(Resources.Resource))]
        public int AccountLevel { get; set; }

        [Display(Name = "AccountFather", ResourceType = typeof(Resources.Resource))]
        public string AccountFather { get; set; }

        [Display(Name = "AccountFatherName", ResourceType = typeof(Resources.Resource))]
        public string AccountFatherName { get; set; }

        public AccountType AccountType { get; set; }
        [Display(Name = "AccountType", ResourceType = typeof(Resources.Resource))]
        [Required]
        public int AccountTypeID { get; set; }

        [Display(Name = "AccountKind", ResourceType = typeof(Resources.Resource))]
        [Required]
        public int AccountKind { get; set; }


        [Display(Name = "OpeningBalance", ResourceType = typeof(Resources.Resource))]
        public double OpeningBalanceDebit { get; set; }

        [Display(Name = "OpeningBalance", ResourceType = typeof(Resources.Resource))]
        public double OpeningBalanceCredit { get; set; }

        [Display(Name = "ChequeBalance", ResourceType = typeof(Resources.Resource))]
        public double ChequeBalanceDebit { get; set; }

        [Display(Name = "ChequeBalance", ResourceType = typeof(Resources.Resource))]
        public double ChequeBalanceCredit { get; set; }

        [Display(Name = "BillBalance", ResourceType = typeof(Resources.Resource))]
        public double BillBalanceDebit { get; set; }

        [Display(Name = "BillBalance", ResourceType = typeof(Resources.Resource))]
        public double BillBalanceCredit { get; set; }

        [Display(Name = "Currency", ResourceType = typeof(Resources.Resource))]
        public Currency Currency { get; set; }

        [Required]
        public int CurrencyID { get; set; }

        [Display(Name = "StoppedAccount", ResourceType = typeof(Resources.Resource))]
        public bool StoppedAccount { get; set; }

        [Display(Name = "Note", ResourceType = typeof(Resources.Resource))]
        public string Note { get; set; }

        [Display(Name = "InsUserName", ResourceType = typeof(Resources.Resource))]
        public string InsUserID { get; set; }

        [Display(Name = "InsDateTime", ResourceType = typeof(Resources.Resource))]
        public DateTime InsDateTime { get; set; }

        [Display(Name = "IsFirstLevel", ResourceType = typeof(Resources.Resource))]
        public bool IsFirstLevel { get; set; }


        public string LevelZero { get; set; }

        public string OrignalAccount { get; set; }

        [Display(Name = "Sales", ResourceType = typeof(Resources.Resource))]
        public int SalesID { get; set; }

    }
}