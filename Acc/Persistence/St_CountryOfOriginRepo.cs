using Acc.Models;
using Acc.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Persistence
{
    public class St_CountryOfOriginRepo : ISt_CountryOfOriginRepo
    {

        private readonly ApplicationDbContext _context;

        public St_CountryOfOriginRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(St_CountryOfOrigin ObjSave)
        {
            _context.St_CountryOfOrigins.Add(ObjSave);
        }

        public void Delete(St_CountryOfOrigin ObjDelete)
        {
            var ObjToDelete = _context.St_CountryOfOrigins.SingleOrDefault(m => m.CompanyID == ObjDelete.CompanyID && m.CountryOfOriginID == ObjDelete.CountryOfOriginID);
            if (ObjToDelete != null)
            {
                _context.St_CountryOfOrigins.Remove(ObjToDelete);
            }
        }

        public IEnumerable<St_CountryOfOrigin> GetAllSt_CountryOfOrigin(int CompanyID)
        {
            return _context.St_CountryOfOrigins.Where(m => m.CompanyID == CompanyID).ToList();
        }

        public int GetMaxSerial(int CompanyID)
        {
            var Obj = _context.St_CountryOfOrigins.FirstOrDefault(m => m.CompanyID == CompanyID);
            if (Obj == null)
            {
                return 1;
            }
            else
            {
                return _context.St_CountryOfOrigins.Where(m => m.CompanyID == CompanyID).Max(p => p.CountryOfOriginID) + 1;
            }
        }

        public St_CountryOfOrigin GetSt_CountryOfOriginByID(int CompanyID, int CountryOfOriginID)
        {
            return _context.St_CountryOfOrigins.FirstOrDefault(m => m.CompanyID == CompanyID && m.CountryOfOriginID == CountryOfOriginID);
        }

        public void Update(St_CountryOfOrigin ObjUpdate)
        {
            var ObjToUpdate = _context.St_CountryOfOrigins.FirstOrDefault(m => m.CompanyID == ObjUpdate.CompanyID && m.CountryOfOriginID == ObjUpdate.CountryOfOriginID);
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