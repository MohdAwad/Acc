using Acc.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Acc.ViewModels
{
    public class AssetToAccVM
    {
        [Display(Name = "TransactionKind", ResourceType = typeof(Resources.Resource))]
        public int CompanyTransactionKindID { get; set; }
        public IEnumerable<CompanyTransactionKind> CompanyTransactionKind { get; set; }

        [Display(Name = "PostedEntry", ResourceType = typeof(Resources.Resource))]
        public int PostedEntry { get; set; }

        [Display(Name = "TransNo", ResourceType = typeof(Resources.Resource))]
        public int TransNo { get; set; }

        [Display(Name = "TransDate", ResourceType = typeof(Resources.Resource))]
        public DateTime TransDate { get; set; }

        [Display(Name = "DepreciationExpenseNo", ResourceType = typeof(Resources.Resource))]
        public string DebitAccountNo { get; set; }//مصروف الاستهلاك

        [Display(Name = "DepreciationExpenseName", ResourceType = typeof(Resources.Resource))]
        public string DebitAccountName { get; set; }


        [Display(Name = "CostCenter", ResourceType = typeof(Resources.Resource))]
        public string DebitCostNo { get; set; }//مصروف الاستهلاك

       [Display(Name = "Name", ResourceType = typeof(Resources.Resource))]
       public string DebitCostName { get; set; }

       [Display(Name = "Note", ResourceType = typeof(Resources.Resource))]
       public string Note { get; set; }
        public Boolean WorkWithCostCenter { get; set; }
        public int TheDecimalPointForTheLocalCurrency { get; set; }
        public int TheDecimalPointForTheForeignCurrency { get; set; }
        public string TotalDebit { get; set; }
        public string TotalCredit { get; set; }
        public string Difference { get; set; }

        public int CompanyYear { get; set; }

    }
}