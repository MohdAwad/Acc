using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Acc.Models
{
    public class DefinitionAssetSite
    {
        public int Id { get; set; }
        public int CompanyID { get; set; }
        [Display(Name = "AssetID", ResourceType = typeof(Resources.Resource))]
        public int AssetID { get; set; }
        [Display(Name = "AssetTypeID", ResourceType = typeof(Resources.Resource))]
        public int AssetTypeID { get; set; }
        [Display(Name = "AssetAdministration", ResourceType = typeof(Resources.Resource))]
        public int AdministrationID { get; set; }
        [Display(Name = "AssetCircle", ResourceType = typeof(Resources.Resource))]
        public int CircleID { get; set; }
        [Display(Name = "AssetSection", ResourceType = typeof(Resources.Resource))]
        public int SectionID { get; set; }
        [Display(Name = "AssetSite", ResourceType = typeof(Resources.Resource))]
        public int SiteID { get; set; }
        [Display(Name = "TransferDate", ResourceType = typeof(Resources.Resource))]
        public DateTime TransferDate { get; set; }
        [Display(Name = "Note", ResourceType = typeof(Resources.Resource))]
        public string Note { get; set; }
        [Display(Name = "AssetTrustee", ResourceType = typeof(Resources.Resource))]
        public int TrusteeID { get; set; }
        [Display(Name = "DeliveryDate", ResourceType = typeof(Resources.Resource))]
        public DateTime DeliveryDate { get; set; }
        [Display(Name = "Note", ResourceType = typeof(Resources.Resource))]
        public string DeliveryNote { get; set; }
        [Display(Name = "InsUserName", ResourceType = typeof(Resources.Resource))]
        public string InsUserID { get; set; }
        [Display(Name = "InsDateTime", ResourceType = typeof(Resources.Resource))]
        public DateTime InsDateTime { get; set; }
        [Display(Name = "InsUserName", ResourceType = typeof(Resources.Resource))]
        public string DeliveryInsUserID { get; set; }
        [Display(Name = "InsDateTime", ResourceType = typeof(Resources.Resource))]
        public DateTime DeliveryInsDateTime { get; set; }

    }
}