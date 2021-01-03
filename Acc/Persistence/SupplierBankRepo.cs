using Acc.Models;
using Acc.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Persistence
{
    public class SupplierBankRepo : ISupplierBankRepo
    {
        private readonly ApplicationDbContext _context;

        public SupplierBankRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(SupplierBank ObjSave)
        {
            _context.SupplierBanks.Add(ObjSave);
        }

        public void Delete(SupplierBank ObjDelete)
        {
            var ObjToDelete = _context.SupplierBanks.SingleOrDefault(m => m.CompanyID == ObjDelete.CompanyID && m.SupplierBankID == ObjDelete.SupplierBankID);
            if (ObjToDelete != null)
            {
                _context.SupplierBanks.Remove(ObjToDelete);
            }
        }

        public IEnumerable<SupplierBank> GetAllSupplierBank(int CompanyID)
        {
            return _context.SupplierBanks.Where(m => m.CompanyID == CompanyID).ToList();
        }

        public int GetMaxSerial(int CompanyID)
        {
            var Obj = _context.SupplierBanks.FirstOrDefault(m => m.CompanyID == CompanyID);
            if (Obj == null)
            {
                return 1;
            }
            else
            {
                return _context.SupplierBanks.Where(m => m.CompanyID == CompanyID).Max(p => p.SupplierBankID) + 1;
            }
        }

        public SupplierBank GetSupplierBankByID(int CompanyID, int SupplierBankID)
        {
            return _context.SupplierBanks.FirstOrDefault(m => m.CompanyID == CompanyID && m.SupplierBankID == SupplierBankID);
        }
        public void Update(SupplierBank ObjUpdate)
        {
            var ObjToUpdate = _context.SupplierBanks.FirstOrDefault(m => m.CompanyID == ObjUpdate.CompanyID && m.SupplierBankID == ObjUpdate.SupplierBankID);
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