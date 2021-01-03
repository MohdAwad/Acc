using Acc.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Acc.ViewModels
{
    public class St_MeasurementDetailVM
    {
        public int CompanyID { get; set; }
        [Display(Name = "Serial", ResourceType = typeof(Resources.Resource))]
        public int MeasurementDetailID { get; set; }
        [Display(Name = "ArabicName", ResourceType = typeof(Resources.Resource))]
        public string ArabicName { get; set; }
        [Display(Name = "EnglishName", ResourceType = typeof(Resources.Resource))]
        public string EnglishName { get; set; }
        [Display(Name = "Measurements", ResourceType = typeof(Resources.Resource))]
        public int MeasurementID { get; set; }
        [Display(Name = "InsUserName", ResourceType = typeof(Resources.Resource))]
        public string InsUserID { get; set; }
        [Display(Name = "InsDateTime", ResourceType = typeof(Resources.Resource))]
        public DateTime InsDateTime { get; set; }
        [Display(Name = "DescriptionName", ResourceType = typeof(Resources.Resource))]
        public string MeasurementName { get; set; }
        [Display(Name = "DescriptionDetailName", ResourceType = typeof(Resources.Resource))]
        public string MeasurementDetailName { get; set; }
        [Display(Name = "Measurements", ResourceType = typeof(Resources.Resource))]
        public IEnumerable<St_Measurement> St_Measurement { get; set; }
    }
}