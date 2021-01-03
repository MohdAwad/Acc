using Acc.Models;
using Acc.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Persistence
{
    public class St_BranchAccountHRepo : ISt_BranchAccountHRepo
    {
        private readonly ApplicationDbContext _context;
        public St_BranchAccountHRepo(ApplicationDbContext context)
        {
            _context = context;
        }
        public void Add(St_BranchAccountH ObjSave)
        {
            _context.St_BranchAccountHs.Add(ObjSave);
        }
        public void AddBranchExpenseAccountH(St_BranchExpenseAccountH ObjSave)
        {
            _context.St_BranchExpenseAccountHs.Add(ObjSave);
        }
        public void AddBranchOtherExpenseAccountH(St_BranchOtherExpenseAccountH ObjSave)
        {
            _context.St_BranchOtherExpenseAccountHs.Add(ObjSave);
        }
        public void AddSt_BranchExpenseAccount(St_BranchExpenseAccountH ObjSave)
        {
            _context.St_BranchExpenseAccountHs.Add(ObjSave);
        }
        public St_BranchAccountH GetSt_BranchAccountHByID(int CompanyID, string BranchCode)
        {
            return _context.St_BranchAccountHs.FirstOrDefault(m => m.CompanyID == CompanyID && m.BranchCode == BranchCode);
        }
        public void Delete(St_BranchAccountH ObjDelete)
        {
            var ObjToDelete = _context.St_BranchAccountHs.SingleOrDefault(m => m.CompanyID == ObjDelete.CompanyID && m.BranchCode == ObjDelete.BranchCode);
            if (ObjToDelete != null)
            {
                _context.St_BranchAccountHs.Remove(ObjToDelete);
            }
        }
    }
}