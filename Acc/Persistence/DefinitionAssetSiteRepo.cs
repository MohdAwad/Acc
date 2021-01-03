using Acc.Models;
using Acc.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Persistence
{
    public class DefinitionAssetSiteRepo : IDefinitionAssetSiteRepo
    {
        private readonly ApplicationDbContext _context;
        public DefinitionAssetSiteRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(DefinitionAssetSite ObjSave)
        {
            _context.DefinitionAssetSites.Add(ObjSave);
        }
        public void Update(DefinitionAssetSite ObjUpdate)
        {
            var ObjToUpdate = _context.DefinitionAssetSites.FirstOrDefault(m => m.CompanyID == ObjUpdate.CompanyID && m.Id == ObjUpdate.Id);
            if (ObjToUpdate != null)
            {
                ObjToUpdate.TrusteeID = ObjUpdate.TrusteeID;
                ObjToUpdate.DeliveryNote = ObjUpdate.DeliveryNote;
                ObjToUpdate.DeliveryDate = ObjUpdate.DeliveryDate;
                ObjToUpdate.DeliveryInsDateTime = ObjUpdate.DeliveryInsDateTime;
                ObjToUpdate.DeliveryInsUserID = ObjUpdate.DeliveryInsUserID;
            }
        }
    }
}