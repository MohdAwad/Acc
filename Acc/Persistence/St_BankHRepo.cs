using Acc.Models;
using Acc.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Persistence
{
    public class St_BankHRepo : ISt_BankHRepo
    {

        private readonly ApplicationDbContext _context;
        public St_BankHRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(St_BankH ObjSave)
        {
            _context.St_BankHs.Add(ObjSave);
        }

        public void Delete(St_BankH ObjDelete)
        {
            var ObjToDelete = _context.St_BankHs.SingleOrDefault(m => m.CompanyID == ObjDelete.CompanyID && m.BankID == ObjDelete.BankID);
            if (ObjToDelete != null)
            {
                _context.St_BankHs.Remove(ObjToDelete);
            }
        }

        public IEnumerable<St_BankH> GetAllSt_BankH(int CompanyID)
        {
            return _context.St_BankHs.Where(m => m.CompanyID == CompanyID).ToList();
        }

        public St_BankH GetAllSt_BankHByID(int CompanyID, int BankID)
        {
            return _context.St_BankHs.FirstOrDefault(m => m.CompanyID == CompanyID  && m.BankID == BankID);
        }

        public int GetMaxSerial(int CompanyID)
        {
            var Obj = _context.St_BankHs.FirstOrDefault(m => m.CompanyID == CompanyID);
            if (Obj == null)
            {
                return 1;
            }
            else
            {
                return _context.St_BankHs.Where(m => m.CompanyID == CompanyID).Max(p => p.BankID) + 1;
            }
        }

        public void Update(St_BankH ObjUpdate)
        {
            var ObjToUpdate = _context.St_BankHs.FirstOrDefault(m => m.CompanyID == ObjUpdate.CompanyID && m.BankID == ObjUpdate.BankID);
            if (ObjToUpdate != null)
            {
                ObjToUpdate.ArabicName = ObjUpdate.ArabicName;
                ObjToUpdate.EnglishName = ObjUpdate.EnglishName;
                ObjToUpdate.FundingAgencyID = ObjUpdate.FundingAgencyID;
                ObjToUpdate.InsUserID = ObjUpdate.InsUserID;
                ObjToUpdate.InsDateTime = ObjUpdate.InsDateTime;
                ObjToUpdate.BankAccountNumber = ObjUpdate.BankAccountNumber;
            }
        }
    }
}