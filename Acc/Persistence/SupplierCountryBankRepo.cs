using Acc.Models;
using Acc.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Persistence
{
    public class SupplierCountryBankRepo : ISupplierCountryBankRepo
    {
        private readonly ApplicationDbContext _context;

        public SupplierCountryBankRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(SupplierCountryBank ObjSave)
        {
            _context.SupplierCountryBanks.Add(ObjSave);
        }

        public void Delete(SupplierCountryBank ObjDelete)
        {
            var ObjToDelete = _context.SupplierCountryBanks.SingleOrDefault(m => m.CompanyID == ObjDelete.CompanyID && m.SupplierCountryBankID == ObjDelete.SupplierCountryBankID);
            if (ObjToDelete != null)
            {
                _context.SupplierCountryBanks.Remove(ObjToDelete);
            }
        }

        public IEnumerable<SupplierCountryBank> GetAllSupplierCountryBank(int CompanyID)
        {
            return _context.SupplierCountryBanks.Where(m => m.CompanyID == CompanyID).ToList();
        }

        public int GetMaxSerial(int CompanyID)
        {
            var Obj = _context.SupplierCountryBanks.FirstOrDefault(m => m.CompanyID == CompanyID);
            if (Obj == null)
            {
                return 1;
            }
            else
            {
                return _context.SupplierCountryBanks.Where(m => m.CompanyID == CompanyID).Max(p => p.SupplierCountryBankID) + 1;
            }
        }

        public SupplierCountryBank GetSupplierCountryBankByID(int CompanyID, int SupplierCountryBankID)
        {
            return _context.SupplierCountryBanks.FirstOrDefault(m => m.CompanyID == CompanyID && m.SupplierCountryBankID == SupplierCountryBankID);
        }
        public void Update(SupplierCountryBank ObjUpdate)
        {
            var ObjToUpdate = _context.SupplierCountryBanks.FirstOrDefault(m => m.CompanyID == ObjUpdate.CompanyID && m.SupplierCountryBankID == ObjUpdate.SupplierCountryBankID);
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