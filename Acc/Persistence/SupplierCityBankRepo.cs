using Acc.Models;
using Acc.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Persistence
{
    public class SupplierCityBankRepo : ISupplierCityBankRepo
    {
        private readonly ApplicationDbContext _context;

        public SupplierCityBankRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(SupplierCityBank ObjSave)
        {
            _context.SupplierCityBanks.Add(ObjSave);
        }

        public void Delete(SupplierCityBank ObjDelete)
        {
            var ObjToDelete = _context.SupplierCityBanks.SingleOrDefault(m => m.CompanyID == ObjDelete.CompanyID && m.SupplierCityBankID == ObjDelete.SupplierCityBankID);
            if (ObjToDelete != null)
            {
                _context.SupplierCityBanks.Remove(ObjToDelete);
            }
        }


        public int GetMaxSerial(int CompanyID)
        {
            var Obj = _context.SupplierCityBanks.FirstOrDefault(m => m.CompanyID == CompanyID);
            if (Obj == null)
            {
                return 1;
            }
            else
            {
                return _context.SupplierCityBanks.Where(m => m.CompanyID == CompanyID).Max(p => p.SupplierCityBankID) + 1;
            }
        }
        public void Update(SupplierCityBank ObjUpdate)
        {
            var ObjToUpdate = _context.SupplierCityBanks.FirstOrDefault(m => m.CompanyID == ObjUpdate.CompanyID && m.SupplierCityBankID == ObjUpdate.SupplierCityBankID);
            if (ObjToUpdate != null)
            {
                ObjToUpdate.ArabicName = ObjUpdate.ArabicName;
                ObjToUpdate.EnglishName = ObjUpdate.EnglishName;
                ObjToUpdate.SupplierCountryBankID = ObjUpdate.SupplierCountryBankID;
                ObjToUpdate.InsDateTime = ObjUpdate.InsDateTime;
                ObjToUpdate.InsUserID = ObjUpdate.InsUserID;
            }
        }
        public IEnumerable<SupplierCityBank> GetAllSupplierCityBank(int CompanyID)
        {
            return _context.SupplierCityBanks.Where(m => m.CompanyID == CompanyID).ToList();
        }
        public IEnumerable<SupplierCityBank> GetAllSupplierCityBankByCountryBankID(int CompanyID,int SupplierCountryBankID)
        {
            return _context.SupplierCityBanks.Where(m => m.CompanyID == CompanyID && m.SupplierCountryBankID == SupplierCountryBankID).ToList();
        }
    }
}