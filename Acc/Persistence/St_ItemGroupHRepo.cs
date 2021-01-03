using Acc.Models;
using Acc.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Persistence
{
    public class St_ItemGroupHRepo : ISt_ItemGroupHRepo
    {
        private readonly ApplicationDbContext _context;
        public St_ItemGroupHRepo(ApplicationDbContext context)
        {
            _context = context;
        }
        public void Add(St_ItemGroupH ObjSave)
        {
            _context.St_ItemGroupHs.Add(ObjSave);
        }
        public void Delete(St_ItemGroupH ObjDelete)
        {
            var ObjToDelete = _context.St_ItemGroupHs.SingleOrDefault(m => m.CompanyID == ObjDelete.CompanyID && m.GroupCode == ObjDelete.GroupCode);
            if (ObjToDelete != null)
            {
                _context.St_ItemGroupHs.Remove(ObjToDelete);
            }
        }
        public IEnumerable<St_ItemGroupH> GetAllSt_ItemGroupH(int CompanyID)
        {
            return _context.St_ItemGroupHs.Where(m => m.CompanyID == CompanyID).ToList();
        }
        public St_ItemGroupH GetSt_ItemGroupHByID(int CompanyID, string GroupCode)
        {
            return _context.St_ItemGroupHs.FirstOrDefault(m => m.CompanyID == CompanyID && m.GroupCode == GroupCode);
        }
        public void Update(St_ItemGroupH ObjUpdate)
        {
            var ObjToUpdate = _context.St_ItemGroupHs.FirstOrDefault(m => m.CompanyID == ObjUpdate.CompanyID && m.GroupCode == ObjUpdate.GroupCode);
            if (ObjToUpdate != null)
            {
                ObjToUpdate.ArabicName = ObjUpdate.ArabicName;
                ObjToUpdate.EnglishName = ObjUpdate.EnglishName;
                ObjToUpdate.InsDateTime = ObjUpdate.InsDateTime;
                ObjToUpdate.InsUserID = ObjUpdate.InsUserID;
            }
        }
        public string CheckIfGroupCodeExisting(int CompanyID, string GroupCode)
        {
            var Obj = _context.St_ItemGroupHs.FirstOrDefault(m => m.CompanyID == CompanyID && m.GroupCode == GroupCode);
            if (Obj != null)
                return Obj.GroupCode;
            else
                return "";

        }
    }
}