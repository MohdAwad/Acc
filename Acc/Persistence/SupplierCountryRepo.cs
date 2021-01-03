using Acc.Models;
using Acc.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Persistence
{
    public class SupplierCountryRepo : ISupplierCountryRepo
    {
        private readonly ApplicationDbContext _context;

        public SupplierCountryRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(SupplierCountry ObjSave)
        {
            _context.SupplierCountrys.Add(ObjSave);
        }

        public void Delete(SupplierCountry ObjDelete)
        {
            var ObjToDelete = _context.SupplierCountrys.SingleOrDefault(m => m.CompanyID == ObjDelete.CompanyID && m.SupplierCountryID == ObjDelete.SupplierCountryID);
            if (ObjToDelete != null)
            {
                _context.SupplierCountrys.Remove(ObjToDelete);
            }
        }

        public IEnumerable<SupplierCountry> GetAllSupplierCountry(int CompanyID)
        {
            return _context.SupplierCountrys.Where(m => m.CompanyID == CompanyID).ToList();
        }

        public int GetMaxSerial(int CompanyID)
        {
            var Obj = _context.SupplierCountrys.FirstOrDefault(m => m.CompanyID == CompanyID);
            if (Obj == null)
            {
                return 1;
            }
            else
            {
                return _context.SupplierCountrys.Where(m => m.CompanyID == CompanyID).Max(p => p.SupplierCountryID) + 1;
            }
        }

        public SupplierCountry GetSupplierCountryByID(int CompanyID, int SupplierCountryID)
        {
            return _context.SupplierCountrys.FirstOrDefault(m => m.CompanyID == CompanyID && m.SupplierCountryID == SupplierCountryID);
        }
        public void Update(SupplierCountry ObjUpdate)
        {
            var ObjToUpdate = _context.SupplierCountrys.FirstOrDefault(m => m.CompanyID == ObjUpdate.CompanyID && m.SupplierCountryID == ObjUpdate.SupplierCountryID);
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