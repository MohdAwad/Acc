using Acc.Models;
using Acc.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace Acc.Persistence
{
    public class FreezeTransactionRepo : IFreezeTransactionRepo
    {
        private readonly ApplicationDbContext _context;
        public FreezeTransactionRepo(ApplicationDbContext context)
        {
            _context = context;
        }
        public void Add(FreezeTransaction ObjSave)
        {
            _context.FreezeTransactions.Add(ObjSave);
        }
        public void Delete(FreezeTransaction ObjDelete)
        {
            var ObjToDelete = _context.FreezeTransactions.SingleOrDefault(m => m.CompanyID == ObjDelete.CompanyID && m.MonthID == ObjDelete.MonthID);
            if (ObjToDelete != null)
            {
                _context.FreezeTransactions.Remove(ObjToDelete);
            }
        }
        public FreezeTransaction GetFreezeTransactionByID(int CompanyID, int Id)
        {
            return _context.FreezeTransactions.FirstOrDefault(m => m.CompanyID == CompanyID && m.Id == Id);
        }
    }
}