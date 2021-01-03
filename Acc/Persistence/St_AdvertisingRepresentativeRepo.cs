using Acc.Models;
using Acc.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Persistence
{
    public class St_AdvertisingRepresentativeRepo : ISt_AdvertisingRepresentativeRepo
    {

        private readonly ApplicationDbContext _context;

        public St_AdvertisingRepresentativeRepo(ApplicationDbContext context)
        {
            _context = context;
        }


        public void Add(St_AdvertisingRepresentative ObjSave)
        {
            _context.St_AdvertisingRepresentatives.Add(ObjSave);
        }

        public void Delete(St_AdvertisingRepresentative ObjDelete)
        {
            var ObjToDelete = _context.St_AdvertisingRepresentatives.SingleOrDefault(m => m.CompanyID == ObjDelete.CompanyID && m.AdvertisingRepresentativeID == ObjDelete.AdvertisingRepresentativeID);
            if (ObjToDelete != null)
            {
                _context.St_AdvertisingRepresentatives.Remove(ObjToDelete);
            }
        }

        public IEnumerable<St_AdvertisingRepresentative> GetAllAdvertisingRepresentative(int CompanyID)
        {
            return _context.St_AdvertisingRepresentatives.Where(m => m.CompanyID == CompanyID).ToList();
        }

        public int GetMaxSerial(int CompanyID)
        {
            var Obj = _context.St_AdvertisingRepresentatives.FirstOrDefault(m => m.CompanyID == CompanyID);
            if (Obj == null)
            {
                return 1;
            }
            else
            {
                return _context.St_AdvertisingRepresentatives.Where(m => m.CompanyID == CompanyID).Max(p => p.AdvertisingRepresentativeID) + 1;
            }
        }

        public St_AdvertisingRepresentative GetSt_AdvertisingRepresentativeByID(int CompanyID, int AdvertisingRepresentativeID)
        {
            return _context.St_AdvertisingRepresentatives.FirstOrDefault(m => m.CompanyID == CompanyID && m.AdvertisingRepresentativeID == AdvertisingRepresentativeID);
        }

        public void Update(St_AdvertisingRepresentative ObjUpdate)
        {
            var ObjToUpdate = _context.St_AdvertisingRepresentatives.FirstOrDefault(m => m.CompanyID == ObjUpdate.CompanyID && m.AdvertisingRepresentativeID == ObjUpdate.AdvertisingRepresentativeID);
            if (ObjToUpdate != null)
            {
                ObjToUpdate.ArabicName = ObjUpdate.ArabicName;
                ObjToUpdate.EnglishName = ObjUpdate.EnglishName;
                ObjToUpdate.InsDateTime = ObjUpdate.InsDateTime;
                ObjToUpdate.InsUserID = ObjUpdate.InsUserID;
            }
        }
    }
}