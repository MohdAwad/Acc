using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Acc.Models
{
    public class PapersCase
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CaseID { get; set; }
        public string ArabicName { get; set; }
        public string EnglishName { get; set; }
    }
}