using Acc.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace Acc.ViewModels
{
    public class CustomerAreaVM
    {
        public int CompanyID { get; set; }

        [Display(Name = "Cities", ResourceType = typeof(Resources.Resource))]
        public int CustomerCityID { get; set; }
        [Display(Name = "Serial", ResourceType = typeof(Resources.Resource))]
        public int CustomerAreaID { get; set; }
        [Display(Name = "ArabicName", ResourceType = typeof(Resources.Resource))]
        public string ArabicName { get; set; }
        [Display(Name = "EnglishName", ResourceType = typeof(Resources.Resource))]
        public string EnglishName { get; set; }
        [Display(Name = "InsUserName", ResourceType = typeof(Resources.Resource))]
        public string InsUserID { get; set; }
        [Display(Name = "InsDateTime", ResourceType = typeof(Resources.Resource))]
        public DateTime InsDateTime { get; set; }
        [Display(Name = "Cities", ResourceType = typeof(Resources.Resource))]
        public IEnumerable<CustomerCity> CustomerCity { get; set; }
        [Display(Name = "CityName", ResourceType = typeof(Resources.Resource))]
        public string CustomerCityName { get; set; }
        [Display(Name = "AreaName", ResourceType = typeof(Resources.Resource))]
        public string CustomerAreaName { get; set; }
    }
}