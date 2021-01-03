using Acc.Models;
using Acc.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Persistence
{
    public class SupplierCityRepo : ISupplierCityRepo
    {
        private readonly ApplicationDbContext _context;

        public SupplierCityRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(SupplierCity ObjSave)
        {
            _context.SupplierCitys.Add(ObjSave);
        }

        public void Delete(SupplierCity ObjDelete)
        {
            var ObjToDelete = _context.SupplierCitys.SingleOrDefault(m => m.CompanyID == ObjDelete.CompanyID && m.SupplierCityID == ObjDelete.SupplierCityID);
            if (ObjToDelete != null)
            {
                _context.SupplierCitys.Remove(ObjToDelete);
            }
        }


        public int GetMaxSerial(int CompanyID)
        {
            var Obj = _context.SupplierCitys.FirstOrDefault(m => m.CompanyID == CompanyID);
            if (Obj == null)
            {
                return 1;
            }
            else
            {
                return _context.SupplierCitys.Where(m => m.CompanyID == CompanyID).Max(p => p.SupplierCityID) + 1;
            }
        }
        public void Update(SupplierCity ObjUpdate)
        {
            var ObjToUpdate = _context.SupplierCitys.FirstOrDefault(m => m.CompanyID == ObjUpdate.CompanyID && m.SupplierCityID == ObjUpdate.SupplierCityID);
            if (ObjToUpdate != null)
            {
                ObjToUpdate.ArabicName = ObjUpdate.ArabicName;
                ObjToUpdate.EnglishName = ObjUpdate.EnglishName;
                ObjToUpdate.SupplierCountryID = ObjUpdate.SupplierCountryID;
                ObjToUpdate.InsDateTime = ObjUpdate.InsDateTime;
                ObjToUpdate.InsUserID = ObjUpdate.InsUserID;
            }
        }
        public IEnumerable<SupplierCity> GetAllSupplierCity(int CompanyID)
        {
            return _context.SupplierCitys.Where(m => m.CompanyID == CompanyID).ToList();
        }
        public IEnumerable<SupplierCity> GetAllSupplierCityByCountryID(int CompanyID, int SupplierCountryID)
        {
            return _context.SupplierCitys.Where(m => m.CompanyID == CompanyID && m.SupplierCountryID == SupplierCountryID).ToList();
        }
    }
}