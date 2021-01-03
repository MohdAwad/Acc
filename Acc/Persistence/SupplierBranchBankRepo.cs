using Acc.Models;
using Acc.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Persistence
{
    public class SupplierBranchBankRepo : ISupplierBranchBankRepo
    {
        private readonly ApplicationDbContext _context;

        public SupplierBranchBankRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(SupplierBranchBank ObjSave)
        {
            _context.SupplierBranchBanks.Add(ObjSave);
        }

        public void Delete(SupplierBranchBank ObjDelete)
        {
            var ObjToDelete = _context.SupplierBranchBanks.SingleOrDefault(m => m.CompanyID == ObjDelete.CompanyID && m.SupplierBranchBankID == ObjDelete.SupplierBranchBankID);
            if (ObjToDelete != null)
            {
                _context.SupplierBranchBanks.Remove(ObjToDelete);
            }
        }


        public int GetMaxSerial(int CompanyID)
        {
            var Obj = _context.SupplierBranchBanks.FirstOrDefault(m => m.CompanyID == CompanyID);
            if (Obj == null)
            {
                return 1;
            }
            else
            {
                return _context.SupplierBranchBanks.Where(m => m.CompanyID == CompanyID).Max(p => p.SupplierBranchBankID) + 1;
            }
        }
        public void Update(SupplierBranchBank ObjUpdate)
        {
            var ObjToUpdate = _context.SupplierBranchBanks.FirstOrDefault(m => m.CompanyID == ObjUpdate.CompanyID && m.SupplierBranchBankID == ObjUpdate.SupplierBranchBankID);
            if (ObjToUpdate != null)
            {
                ObjToUpdate.ArabicName = ObjUpdate.ArabicName;
                ObjToUpdate.EnglishName = ObjUpdate.EnglishName;
                ObjToUpdate.SupplierBankID = ObjUpdate.SupplierBankID;
                ObjToUpdate.InsDateTime = ObjUpdate.InsDateTime;
                ObjToUpdate.InsUserID = ObjUpdate.InsUserID;
            }
        }
        public IEnumerable<SupplierBranchBank> GetAllSupplierBranchBank(int CompanyID)
        {
            return _context.SupplierBranchBanks.Where(m => m.CompanyID == CompanyID).ToList();
        }
        public IEnumerable<SupplierBranchBank> GetAllSupplierBranchBankByBankID(int CompanyID,int SupplierBankID)
        {
            return _context.SupplierBranchBanks.Where(m => m.CompanyID == CompanyID && m.SupplierBankID == SupplierBankID).ToList();
        }
    }
}