using Acc.Models;
using Acc.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Persistence
{
    public class CompanyTransactionKindRepo : ICompanyTransactionKindRepo
    {
        private readonly ApplicationDbContext _context;

        public CompanyTransactionKindRepo(ApplicationDbContext context)
        {
            _context = context;
        }
        public void Add(CompanyTransactionKind ObjSave)
        {
            _context.CompanyTransactionKinds.Add(ObjSave);
        }

        public void Delete(CompanyTransactionKind ObjDelete)
        {
            var ObjToDelete = _context.CompanyTransactionKinds.SingleOrDefault(m => m.CompanyID == ObjDelete.CompanyID && m.CompanyTransactionKindID == ObjDelete.CompanyTransactionKindID);
            if (ObjToDelete != null)
            {
                _context.CompanyTransactionKinds.Remove(ObjToDelete);
            }
        }

        public IEnumerable<CompanyTransactionKind> GetAllCompanyTransactionKind(int CompanyID)
        {
            return _context.CompanyTransactionKinds.Where(m => m.CompanyID == CompanyID).ToList();
        }

        public CompanyTransactionKind GetCompanyTransactionKindByID(int CompanyID, int CompanyTransactionKindID )
        {
            return _context.CompanyTransactionKinds.FirstOrDefault(m => m.CompanyID == CompanyID && m.CompanyTransactionKindID == CompanyTransactionKindID);
        }

        public int GetCompanyTransactionKindByTransKindNo(int CompanyID, int TransactionKindID)
        {
            var Obj = _context.CompanyTransactionKinds.FirstOrDefault(m => m.CompanyID == CompanyID && m.TransactionKindID == TransactionKindID);
            return Obj.CompanyTransactionKindID;
        }

        public int GetMaxSerial(int CompanyID)
        {
            var Obj = _context.CompanyTransactionKinds.FirstOrDefault(m => m.CompanyID == CompanyID);
            if (Obj == null)
            {
                return 1;
            }
            else
            {
                return _context.CompanyTransactionKinds.Where(m => m.CompanyID == CompanyID).Max(p => p.CompanyTransactionKindID) + 1;
            }
        }

        public int GetTransKindForCompanyTransactionKind(int CompanyID,int CompanyTransactionKindID)
        {
            var Obj = _context.CompanyTransactionKinds.FirstOrDefault(m => m.CompanyID == CompanyID && m.CompanyTransactionKindID == CompanyTransactionKindID);
            if (Obj != null)
                return Obj.TransactionKindID;
            else
                return 0;

           
           
        }

        public void Update(CompanyTransactionKind ObjUpdate)
        {
            var ObjToUpdate = _context.CompanyTransactionKinds.FirstOrDefault(m => m.CompanyID == ObjUpdate.CompanyID && m.CompanyTransactionKindID == ObjUpdate.CompanyTransactionKindID);
            if (ObjToUpdate != null)
            {
                ObjToUpdate.ArabicName = ObjUpdate.ArabicName;
                ObjToUpdate.EnglishName = ObjUpdate.EnglishName;
                ObjToUpdate.TransactionKind = ObjUpdate.TransactionKind;
                ObjToUpdate.AutoSerial = ObjUpdate.AutoSerial;
                ObjToUpdate.MonthlySerial = ObjUpdate.MonthlySerial;
                ObjToUpdate.Symbol = ObjUpdate.Symbol;
                ObjToUpdate.Year = ObjUpdate.Year;
                ObjToUpdate.Month = ObjUpdate.Month;
                ObjToUpdate.Serial = ObjUpdate.Serial;
                ObjToUpdate.InsDateTime = ObjUpdate.InsDateTime;
                ObjToUpdate.InsUserID = ObjUpdate.InsUserID;
            }
        }
    }
}