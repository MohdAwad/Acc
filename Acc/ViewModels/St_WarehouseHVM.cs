using Acc.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Acc.ViewModels
{
    public class St_WarehouseHVM
    {
        public int CompanyID { get; set; }
        [Display(Name = "StockCode", ResourceType = typeof(Resources.Resource))]
        public string StockCode { get; set; }
        [Display(Name = "ArabicName", ResourceType = typeof(Resources.Resource))]
        public string ArabicName { get; set; }
        [Display(Name = "EnglishName", ResourceType = typeof(Resources.Resource))]
        public string EnglishName { get; set; }
        [Display(Name = "Address", ResourceType = typeof(Resources.Resource))]
        public string Address { get; set; }
        [Display(Name = "Telephone", ResourceType = typeof(Resources.Resource))]
        public string Telephone { get; set; }

        [Display(Name = "StockName", ResourceType = typeof(Resources.Resource))]
        public string StockName { get; set; }

        [Display(Name = "InsUserName", ResourceType = typeof(Resources.Resource))]
        public string InsUserID { get; set; }
        [Display(Name = "InsDateTime", ResourceType = typeof(Resources.Resource))]
        public DateTime InsDateTime { get; set; }
        public int iRowTable { get; set; }
        public int StockCase { get; set; }
        [Display(Name = "ManufacturingWarehouse", ResourceType = typeof(Resources.Resource))]
        public Boolean ManufacturingWarehouse { get; set; }
    }
}