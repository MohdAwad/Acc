using Acc.Models;
using Acc.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Persistence
{
    public class St_TransactionKindHRepo : ISt_TransactionKindHRepo
    {
        private readonly ApplicationDbContext _context;
        public St_TransactionKindHRepo(ApplicationDbContext context)
        {
            _context = context;
        }
        public IEnumerable<St_TransactionKindH> GetAllSt_TransactionKindH()
        {
            return _context.St_TransactionKindHs.Where(m => m.St_TransactionKindID > 0).ToList();
        }

        public St_TransactionKindH GetSt_TransactionKindHByID(int St_TransactionKindID)
        {
            return _context.St_TransactionKindHs.FirstOrDefault(m => m.St_TransactionKindID == St_TransactionKindID);
        }
    }
}