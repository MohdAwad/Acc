using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Acc.ViewModels
{
    public class ClientSuplierFilterVM
    {
        public int ClientSup { get; set; }
        [Display(Name = "Sales", ResourceType = typeof(Resources.Resource))]
        public bool Sales { get; set; }
        [Display(Name = "ToDate", ResourceType = typeof(Resources.Resource))]
        public DateTime ToDate { get; set; }
    }
}