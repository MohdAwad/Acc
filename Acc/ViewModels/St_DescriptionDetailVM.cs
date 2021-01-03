using Acc.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace Acc.ViewModels
{
    public class St_DescriptionDetailVM
    {
        public int CompanyID { get; set; }
        [Display(Name = "Serial", ResourceType = typeof(Resources.Resource))]
        public int DescriptionDetailID { get; set; }
        [Display(Name = "ArabicName", ResourceType = typeof(Resources.Resource))]
        public string ArabicName { get; set; }
        [Display(Name = "EnglishName", ResourceType = typeof(Resources.Resource))]
        public string EnglishName { get; set; }
        [Display(Name = "Descriptions", ResourceType = typeof(Resources.Resource))]
        public int DescriptionID { get; set; }
        [Display(Name = "InsUserName", ResourceType = typeof(Resources.Resource))]
        public string InsUserID { get; set; }
        [Display(Name = "InsDateTime", ResourceType = typeof(Resources.Resource))]
        public DateTime InsDateTime { get; set; }
        [Display(Name = "DescriptionName", ResourceType = typeof(Resources.Resource))]
        public string DescriptionName { get; set; }
        [Display(Name = "DescriptionDetailName", ResourceType = typeof(Resources.Resource))]
        public string DescriptionDetailName { get; set; }
        [Display(Name = "Descriptions", ResourceType = typeof(Resources.Resource))]
        public IEnumerable<St_Description> St_Description { get; set; }
    }
}