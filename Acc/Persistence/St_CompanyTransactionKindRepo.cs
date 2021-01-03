using Acc.Models;
using Acc.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace Acc.Persistence
{
    public class St_CompanyTransactionKindRepo : ISt_CompanyTransactionKindRepo
    {
        private readonly ApplicationDbContext _context;

        public St_CompanyTransactionKindRepo(ApplicationDbContext context)
        {
            _context = context;
        }
        public void Add(St_CompanyTransactionKind ObjSave)
        {
            _context.St_CompanyTransactionKinds.Add(ObjSave);
        }
        public IEnumerable<St_CompanyTransactionKind> GetAllSt_CompanyTransactionKind(int CompanyID)
        {
            return _context.St_CompanyTransactionKinds.Where(m => m.CompanyID == CompanyID).ToList();
        }

        public St_CompanyTransactionKind GetSt_CompanyTransactionKindByID(int CompanyID, int St_CompanyTransactionKindID)
        {
            return _context.St_CompanyTransactionKinds.FirstOrDefault(m => m.CompanyID == CompanyID && m.St_CompanyTransactionKindID == St_CompanyTransactionKindID);
        }

        public int GetSt_CompanyTransactionKindBySt_TransKindNo(int CompanyID, int St_TransactionKindID)
        {
            var Obj = _context.St_CompanyTransactionKinds.FirstOrDefault(m => m.CompanyID == CompanyID && m.St_TransactionKindID == St_TransactionKindID);
            return Obj.St_CompanyTransactionKindID;
        }

        public int GetMaxSerial(int CompanyID)
        {
            var Obj = _context.St_CompanyTransactionKinds.FirstOrDefault(m => m.CompanyID == CompanyID);
            if (Obj == null)
            {
                return 1000;
            }
            else
            {
                return _context.St_CompanyTransactionKinds.Where(m => m.CompanyID == CompanyID).Max(p => p.St_CompanyTransactionKindID) + 1;
            }
        }

        public int GetSt_TransKindForSt_CompanyTransactionKind(int CompanyID, int St_CompanyTransactionKindID)
        {
            var Obj = _context.St_CompanyTransactionKinds.FirstOrDefault(m => m.CompanyID == CompanyID && m.St_CompanyTransactionKindID == St_CompanyTransactionKindID);
            if (Obj != null)
                return Obj.St_TransactionKindID;
            else
                return 0;



        }

        public void Update(St_CompanyTransactionKind ObjUpdate)
        {
            var ObjToUpdate = _context.St_CompanyTransactionKinds.FirstOrDefault(m => m.CompanyID == ObjUpdate.CompanyID && m.St_CompanyTransactionKindID == ObjUpdate.St_CompanyTransactionKindID);
            if (ObjToUpdate != null)
            {
                ObjToUpdate.AutoSerial = ObjUpdate.AutoSerial;
                ObjToUpdate.SymbolSerial = ObjUpdate.SymbolSerial;
                ObjToUpdate.Symbol = ObjUpdate.Symbol;
                ObjToUpdate.Serial = ObjUpdate.Serial;
                ObjToUpdate.InsDateTime = ObjUpdate.InsDateTime;
                ObjToUpdate.InsUserID = ObjUpdate.InsUserID;
            }
        }
        public int GetSt_CompanyTransactionKind(int CompanyID, int St_TransactionKindID, string StockCode)
        {
            var Obj = _context.St_CompanyTransactionKinds.FirstOrDefault(m => m.CompanyID == CompanyID && m.St_TransactionKindID == St_TransactionKindID && m.StockCode == StockCode);
            return Obj.St_CompanyTransactionKindID;
        }
        public St_CompanyTransactionKind GetSt_CompanyTransactionKindByIDAndStockCode(int CompanyID, int St_CompanyTransactionKindID,string StockCode)
        {
            return _context.St_CompanyTransactionKinds.FirstOrDefault(m => m.CompanyID == CompanyID && m.St_CompanyTransactionKindID == St_CompanyTransactionKindID && m.StockCode == StockCode);
        }
    }
}