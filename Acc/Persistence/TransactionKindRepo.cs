using Acc.Models;
using Acc.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Persistence
{
    public class TransactionKindRepo : ITransactionKindRepo
    {
        private readonly ApplicationDbContext _context;

        public TransactionKindRepo(ApplicationDbContext context)
        {
            _context = context;
        }
        public IEnumerable<TransactionKind> GetAllTransactionKind()
        {
            return _context.TransactionKinds.Where(m => m.TransactionKindID > 0 && m.TransactionKindID != 17 && m.TransactionKindID != 18).ToList();
        }

        public TransactionKind GetTransactionKindByID(int TransactionKindID)
        {
            return _context.TransactionKinds.FirstOrDefault(m => m.TransactionKindID == TransactionKindID);
        }
    }
}