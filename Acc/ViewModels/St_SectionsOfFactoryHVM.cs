using Acc.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
namespace Acc.ViewModels
{
    public class St_SectionsOfFactoryHVM
    {
        public int CompanyID { get; set; }

        [Display(Name = "FactoryName", ResourceType = typeof(Resources.Resource))]
        public int FactoryID { get; set; }

        [Display(Name = "Serial", ResourceType = typeof(Resources.Resource))]
        public int SectionsOfFactoryID { get; set; }
        [Display(Name = "ArabicName", ResourceType = typeof(Resources.Resource))]
        public string ArabicName { get; set; }
        [Display(Name = "EnglishName", ResourceType = typeof(Resources.Resource))]
        public string EnglishName { get; set; }
        [Display(Name = "InsUserName", ResourceType = typeof(Resources.Resource))]
        public string InsUserID { get; set; }
        [Display(Name = "InsDateTime", ResourceType = typeof(Resources.Resource))]
        public DateTime InsDateTime { get; set; }
        public IEnumerable<St_FactoryH> St_Factory { get; set; }
        [Display(Name = "FactoryName", ResourceType = typeof(Resources.Resource))]
        public string FactoryName { get; set; }
        [Display(Name = "SectionsOfFactoryName", ResourceType = typeof(Resources.Resource))]
        public string SectionsOfFactoryName { get; set; }
    }
}