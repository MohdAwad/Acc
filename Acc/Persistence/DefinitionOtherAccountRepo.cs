using Acc.Models;
using Acc.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Persistence
{
    public class DefinitionOtherAccountRepo : IDefinitionOtherAccountRepo
    {
        private readonly ApplicationDbContext _context;
        public DefinitionOtherAccountRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(OtherAccount ObjSave)
        {
            _context.OtherAccounts.Add(ObjSave);
        }
        public OtherAccount GetDefinitionOtherAccountByID(int CompanyID)
        {
            return _context.OtherAccounts.FirstOrDefault(m => m.CompanyID == CompanyID);
        }
        public void Delete(OtherAccount ObjDelete)
        {
            var ObjToDelete = _context.OtherAccounts.SingleOrDefault(m => m.CompanyID == ObjDelete.CompanyID);
            if (ObjToDelete != null)
            {
                _context.OtherAccounts.Remove(ObjToDelete);
            }
        }
    }
}