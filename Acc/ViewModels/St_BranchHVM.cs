using Acc.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Acc.ViewModels
{
    public class St_BranchHVM
    {
        public int CompanyID { get; set; }
        [Display(Name = "BranchCode", ResourceType = typeof(Resources.Resource))]
        public string BranchCode { get; set; }
        [Display(Name = "ArabicName", ResourceType = typeof(Resources.Resource))]
        public string ArabicName { get; set; }
        [Display(Name = "EnglishName", ResourceType = typeof(Resources.Resource))]
        public string EnglishName { get; set; }
        [Display(Name = "Address", ResourceType = typeof(Resources.Resource))]
        public string Address { get; set; }
        [Display(Name = "Telephone", ResourceType = typeof(Resources.Resource))]
        public string Telephone { get; set; }
        [Display(Name = "BranchStockCode", ResourceType = typeof(Resources.Resource))]
        public string BranchStockCode { get; set; }
        [Display(Name = "MainStockCode", ResourceType = typeof(Resources.Resource))]
        public string MainStockCode { get; set; }

        [Display(Name = "InsUserName", ResourceType = typeof(Resources.Resource))]
        public string InsUserID { get; set; }
        [Display(Name = "InsDateTime", ResourceType = typeof(Resources.Resource))]
        public DateTime InsDateTime { get; set; }
        public IEnumerable<St_WarehouseH> St_WarehouseHMainStock { get; set; }
        public IEnumerable<St_WarehouseH> St_WarehouseHBranchStock { get; set; }
        [Display(Name = "BranchStockCode", ResourceType = typeof(Resources.Resource))]
        public string MainStockName { get; set; }
        [Display(Name = "MainStockCode", ResourceType = typeof(Resources.Resource))]
        public string BranchStockName { get; set; }
        [Display(Name = "BranchName", ResourceType = typeof(Resources.Resource))]
        public string BranchName { get; set; }
    }
}