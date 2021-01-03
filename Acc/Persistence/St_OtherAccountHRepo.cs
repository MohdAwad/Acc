using Acc.Models;
using Acc.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Persistence
{
    public class St_OtherAccountHRepo : ISt_OtherAccountHRepo
    {
        private readonly ApplicationDbContext _context;
        public St_OtherAccountHRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(St_OtherAccountH ObjSave)
        {
            _context.St_OtherAccountHs.Add(ObjSave);
        }
        public St_OtherAccountH GetSt_OtherAccountHByID(int CompanyID)
        {
            return _context.St_OtherAccountHs.FirstOrDefault(m => m.CompanyID == CompanyID);
        }
        public void Delete(St_OtherAccountH ObjDelete)
        {
            var ObjToDelete = _context.St_OtherAccountHs.SingleOrDefault(m => m.CompanyID == ObjDelete.CompanyID);
            if (ObjToDelete != null)
            {
                _context.St_OtherAccountHs.Remove(ObjToDelete);
            }
        }
    }
}