using Acc.Models;
using Acc.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Persistence
{
    public class St_BranchHRepo : ISt_BranchHRepo
    {
        private readonly ApplicationDbContext _context;

        public St_BranchHRepo(ApplicationDbContext context)
        {
            _context = context;
        }
        public ApplicationDbContext Context => _context;
        public void Add(St_BranchH ObjSave)
        {
            Context.St_BrancheHs.Add(ObjSave);
        }
        public void Delete(St_BranchH ObjDelete)
        {
            var ObjToDelete = Context.St_BrancheHs.SingleOrDefault(m => m.CompanyID == ObjDelete.CompanyID && m.BranchCode == ObjDelete.BranchCode);
            if (ObjToDelete != null)
            {
                Context.St_BrancheHs.Remove(ObjToDelete);
            }
        }
        public St_BranchH GetSt_BranchrByID(int CompanyID, string BranchCode)
        {
            return Context.St_BrancheHs.FirstOrDefault(m => m.CompanyID == CompanyID && m.BranchCode == BranchCode);
        }
        public void Update(St_BranchH ObjUpdate)
        {
            var ObjToUpdate = Context.St_BrancheHs.FirstOrDefault(m => m.CompanyID == ObjUpdate.CompanyID && m.BranchCode == ObjUpdate.BranchCode);
            if (ObjToUpdate != null)
            {
                ObjToUpdate.ArabicName = ObjUpdate.ArabicName;
                ObjToUpdate.EnglishName = ObjUpdate.EnglishName;
                ObjToUpdate.MainStockCode = ObjUpdate.MainStockCode;
                ObjToUpdate.BranchStockCode = ObjUpdate.BranchStockCode;
                ObjToUpdate.Address = ObjUpdate.Address;
                ObjToUpdate.Telephone = ObjUpdate.Telephone;
                ObjToUpdate.InsDateTime = ObjUpdate.InsDateTime;
                ObjToUpdate.InsUserID = ObjUpdate.InsUserID;
            }
        }
        public string CheckIfBranchCodeExisting(int CompanyID, string BranchCode)
        {
            var Obj = _context.St_BrancheHs.FirstOrDefault(m => m.CompanyID == CompanyID && m.BranchCode == BranchCode);
            if (Obj != null)
                return Obj.BranchCode;
            else
                return "";

        }
        public IEnumerable<St_BranchH> GetAllSt_BranchH(int CompanyID)
        {
            return _context.St_BrancheHs.Where(m => m.CompanyID == CompanyID).ToList();
        }

    }
}