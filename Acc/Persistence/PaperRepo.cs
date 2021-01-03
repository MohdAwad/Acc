using Acc.Models;
using Acc.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Persistence
{
    public class PaperRepo : IPaperRepo
    {
        private readonly ApplicationDbContext _context;

        public PaperRepo(ApplicationDbContext context)
        {
            _context = context;
        }
        public void Add(Paper ObjToSave)
        {
            _context.Papers.Add(ObjToSave);
        }
        public IEnumerable<Paper> GetPaperDetail(string VoucherNumber, int CompanyTransactionKindNo, int CompanyID, int TransactionKindNo,int CompanyYear)
        {

            return _context.Papers.Where(m => m.CompanyID == CompanyID &&
            m.VoucherNumber == VoucherNumber && m.CompanyTransactionKindNo == CompanyTransactionKindNo && m.TransactionKindNo == TransactionKindNo && m.CompanyYear == CompanyYear).ToList();

        }


    }
}