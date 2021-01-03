using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Acc.Models
{
    public class DefinitionBank
    {
        public Company Company { get; set; }
        [Key]
        [Column(Order = 1)]
        public int CompanyID { get; set; }
        [Key]
        [Column(Order = 2)]
        [Display(Name = "Serial", ResourceType = typeof(Resources.Resource))]
        public int BankID { get; set; }
        [Display(Name = "BankAccountNumber", ResourceType = typeof(Resources.Resource))]
        public string BankAccountNumber { get; set; }
        [Display(Name = "ChequeUnderCollection", ResourceType = typeof(Resources.Resource))]
        public string ChequeUnderCollectionAccountNumber { get; set; }
        [Display(Name = "PostdatedCheque", ResourceType = typeof(Resources.Resource))]
        public string PostdatedChequeAccountNumber { get; set; }
        [Display(Name = "BillsOfExchange", ResourceType = typeof(Resources.Resource))]
        public string BillsOfExchangeAccountNumber { get; set; }

        [Display(Name = "InsUserName", ResourceType = typeof(Resources.Resource))]
        public string InsUserID { get; set; }
        [Display(Name = "InsDateTime", ResourceType = typeof(Resources.Resource))]
        public DateTime InsDateTime { get; set; }

        public int IsDeleted { get; set; }
    }
}