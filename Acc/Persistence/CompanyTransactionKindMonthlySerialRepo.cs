using Acc.Models;
using Acc.Repositories;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Acc.Persistence
{
    public class CompanyTransactionKindMonthlySerialRepo : ICompanyTransactionKindMonthlySerialRepo
    {
        private readonly ApplicationDbContext _context;

        public CompanyTransactionKindMonthlySerialRepo(ApplicationDbContext context)
        {
            _context = context;
        }
        public void Add(CompanyTransactionKindMonthlySerial ObjSave)
        {
            _context.CompanyTransactionKindMonthlySerials.Add(ObjSave);
        }
        public void Delete(int CompanyID,  int CompanyTransactionKindID)
        {
            _context.Database.ExecuteSqlCommand(
             " Delete from CompanyTransactionKindMonthlySerials  " +
             " Where " +
             " CompanyID = @CompanyID " +
             " And " +
             " CompanyTransactionKindID = @CompanyTransactionKindID "
             , new SqlParameter("@CompanyID", CompanyID)
             , new SqlParameter("@CompanyTransactionKindID", CompanyTransactionKindID)
             );
        }
        public int GetMaxSerial(int CompanyID, int CompanyTransactionKindID, int MonthID)
        {
            var Obj = _context.CompanyTransactionKindMonthlySerials.FirstOrDefault(m => m.CompanyID == CompanyID && m.CompanyTransactionKindID == CompanyTransactionKindID
            && m.MonthID == MonthID );
            if (Obj != null)
                return Obj.LastSerial + 1;
            else
                return 1;
        }
        public void Update(int CompanyID, int CompanyTransactionKindID, int MonthID)
        {
            _context.Database.ExecuteSqlCommand(
             " Update CompanyTransactionKindMonthlySerials  " +
             " Set " +
             " LastSerial = LastSerial + 1 " +
             " Where " +
             " CompanyID = @CompanyID " +
             " And " +
             " CompanyTransactionKindID = @CompanyTransactionKindID " +
             " And " +
             " MonthID = @MonthID "
             , new SqlParameter("@CompanyID", CompanyID)
             , new SqlParameter("@CompanyTransactionKindID", CompanyTransactionKindID)
             , new SqlParameter("@MonthID", MonthID)
             );
        }
        public CompanyTransactionKindMonthlySerial CheckIfSave(int CompanyID, int CompanyTransactionKindID)
        {
            return _context.CompanyTransactionKindMonthlySerials.FirstOrDefault(m => m.CompanyID == CompanyID && m.CompanyTransactionKindID == CompanyTransactionKindID);
        }
    }
}