using Acc.Models;
using Acc.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Persistence
{
    public class St_TransactionKindRepo : ISt_TransactionKindRepo
    {
        private readonly ApplicationDbContext _context;
        public St_TransactionKindRepo(ApplicationDbContext context)
        {
            _context = context;
        }
        public IEnumerable<St_TransactionKind> GetAllSt_TransactionKind()
        {
            return _context.St_TransactionKinds.Where(m => m.St_TransactionKindID > 0).ToList();
        }

        public St_TransactionKind GetSt_TransactionKindByID(int St_TransactionKindID)
        {
            return _context.St_TransactionKinds.FirstOrDefault(m => m.St_TransactionKindID == St_TransactionKindID);
        }
    }
}