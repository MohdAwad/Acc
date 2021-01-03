using Acc.Models;
using Acc.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Persistence
{
    public class St_OtherAccountRepo : ISt_OtherAccountRepo
    {
        private readonly ApplicationDbContext _context;
        public St_OtherAccountRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(St_OtherAccount ObjSave)
        {
            _context.St_OtherAccounts.Add(ObjSave);
        }
        public St_OtherAccount GetSt_OtherAccountByID(int CompanyID)
        {
            return _context.St_OtherAccounts.FirstOrDefault(m => m.CompanyID == CompanyID);
        }
        public void Delete(St_OtherAccount ObjDelete)
        {
            var ObjToDelete = _context.St_OtherAccounts.SingleOrDefault(m => m.CompanyID == ObjDelete.CompanyID);
            if (ObjToDelete != null)
            {
                _context.St_OtherAccounts.Remove(ObjToDelete);
            }
        }
    }
}