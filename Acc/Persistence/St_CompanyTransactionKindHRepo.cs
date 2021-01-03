using Acc.Models;
using Acc.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Persistence
{
    public class St_CompanyTransactionKindHRepo : ISt_CompanyTransactionKindHRepo
    {
        private readonly ApplicationDbContext _context;

        public St_CompanyTransactionKindHRepo(ApplicationDbContext context)
        {
            _context = context;
        }
        public void Add(St_CompanyTransactionKindH ObjSave)
        {
            _context.St_CompanyTransactionKindHs.Add(ObjSave);
        }
        public IEnumerable<St_CompanyTransactionKindH> GetAllSt_CompanyTransactionKindH(int CompanyID)
        {
            return _context.St_CompanyTransactionKindHs.Where(m => m.CompanyID == CompanyID).ToList();
        }

        public St_CompanyTransactionKindH GetSt_CompanyTransactionKindHByID(int CompanyID, int St_CompanyTransactionKindID)
        {
            return _context.St_CompanyTransactionKindHs.FirstOrDefault(m => m.CompanyID == CompanyID && m.St_CompanyTransactionKindID == St_CompanyTransactionKindID);
        }

        public int GetSt_CompanyTransactionKindHBySt_TransKindNo(int CompanyID, int St_TransactionKindID)
        {
            var Obj = _context.St_CompanyTransactionKindHs.FirstOrDefault(m => m.CompanyID == CompanyID && m.St_TransactionKindID == St_TransactionKindID);
            return Obj.St_CompanyTransactionKindID;
        }

        public int GetMaxSerial(int CompanyID)
        {
            var Obj = _context.St_CompanyTransactionKindHs.FirstOrDefault(m => m.CompanyID == CompanyID);
            if (Obj == null)
            {
                return 1000;
            }
            else
            {
                return _context.St_CompanyTransactionKindHs.Where(m => m.CompanyID == CompanyID).Max(p => p.St_CompanyTransactionKindID) + 1;
            }
        }

        public int GetSt_TransKindHForSt_CompanyTransactionKindH(int CompanyID, int St_CompanyTransactionKindID)
        {
            var Obj = _context.St_CompanyTransactionKindHs.FirstOrDefault(m => m.CompanyID == CompanyID && m.St_CompanyTransactionKindID == St_CompanyTransactionKindID);
            if (Obj != null)
                return Obj.St_TransactionKindID;
            else
                return 0;
        }
        public void Update(St_CompanyTransactionKindH ObjUpdate)
        {
            var ObjToUpdate = _context.St_CompanyTransactionKindHs.FirstOrDefault(m => m.CompanyID == ObjUpdate.CompanyID && m.St_CompanyTransactionKindID == ObjUpdate.St_CompanyTransactionKindID);
            if (ObjToUpdate != null)
            {
                ObjToUpdate.AutoSerial = ObjUpdate.AutoSerial;
                ObjToUpdate.SymbolSerial = ObjUpdate.SymbolSerial;
                ObjToUpdate.Symbol = ObjUpdate.Symbol;
                ObjToUpdate.Serial = ObjUpdate.Serial;
                ObjToUpdate.StockCode = ObjUpdate.StockCode;
                ObjToUpdate.InsDateTime = ObjUpdate.InsDateTime;
                ObjToUpdate.InsUserID = ObjUpdate.InsUserID;
            }
        }
    }
}