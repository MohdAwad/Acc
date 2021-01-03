using Acc.Models;
using Acc.Repositories;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Acc.Persistence
{
    public class St_CompanyTransactionKindSymbolSerialHRepo : ISt_CompanyTransactionKindSymbolSerialHRepo
    {
        private readonly ApplicationDbContext _context;

        public St_CompanyTransactionKindSymbolSerialHRepo(ApplicationDbContext context)
        {
            _context = context;
        }
        public void Add(St_CompanyTransactionKindSymbolSerialH ObjSave)
        {
            _context.St_CompanyTransactionKindSymbolSerialHs.Add(ObjSave);
        }
        public void Delete(int CompanyID, int St_CompanyTransactionKindID)
        {
            _context.Database.ExecuteSqlCommand(
             " Delete from St_CompanyTransactionKindSymbolSerialH  " +
             " Where " +
             " CompanyID = @CompanyID " +
             " And " +
             " St_CompanyTransactionKindID = @St_CompanyTransactionKindID "
             , new SqlParameter("@CompanyID", CompanyID)
             , new SqlParameter("@St_CompanyTransactionKindID", St_CompanyTransactionKindID)
             );
        }
        public int GetMaxSerial(int CompanyID, int St_CompanyTransactionKindID, string StockCode)
        {
            var Obj = _context.St_CompanyTransactionKindSymbolSerialHs.FirstOrDefault(m => m.CompanyID == CompanyID && m.St_CompanyTransactionKindID == St_CompanyTransactionKindID
            && m.StockCode == StockCode);
            if (Obj != null)
                return Obj.LastSerial + 1;
            else
                return 1;
        }
        public void Update(int CompanyID, int St_CompanyTransactionKindID, string StockCode)
        {
            _context.Database.ExecuteSqlCommand(
             " Update St_CompanyTransactionKindSymbolSerialH  " +
             " Set " +
             " LastSerial = LastSerial + 1 " +
             " Where " +
             " CompanyID = @CompanyID " +
             " And " +
             " St_CompanyTransactionKindID = @St_CompanyTransactionKindID " +
             " And " +
             " StockCode = @StockCode "
             , new SqlParameter("@CompanyID", CompanyID)
             , new SqlParameter("@St_CompanyTransactionKindID", St_CompanyTransactionKindID)
             , new SqlParameter("@StockCode", StockCode)
             );
        }
        public St_CompanyTransactionKindSymbolSerialH CheckIfSave(int CompanyID, int St_CompanyTransactionKindID)
        {
            return _context.St_CompanyTransactionKindSymbolSerialHs.FirstOrDefault(m => m.CompanyID == CompanyID && m.St_CompanyTransactionKindID == St_CompanyTransactionKindID);
        }
    }
}