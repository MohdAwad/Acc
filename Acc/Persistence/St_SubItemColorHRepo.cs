using Acc.Models;
using Acc.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace Acc.Persistence
{
    public class St_SubItemColorHRepo : ISt_SubItemColorHRepo
    {
        private readonly ApplicationDbContext _context;
        public St_SubItemColorHRepo(ApplicationDbContext context)
        {
            _context = context;
        }
        public void Add(St_SubItemColorH ObjSave)
        {
            _context.St_SubItemColorHs.Add(ObjSave);
        }
        public void Delete(St_SubItemColorH ObjDelete)
        {
            var ObjToDelete = _context.St_SubItemColorHs.SingleOrDefault(m => m.CompanyID == ObjDelete.CompanyID && m.SubItemColorCode == ObjDelete.SubItemColorCode);
            if (ObjToDelete != null)
            {
                _context.St_SubItemColorHs.Remove(ObjToDelete);
            }
        }
        public int GetMaxSerial(int CompanyID)
        {
            var Obj = _context.St_SubItemColorHs.FirstOrDefault(m => m.CompanyID == CompanyID);
            if (Obj == null)
            {
                return 1;
            }
            else
            {
                return _context.St_SubItemColorHs.Where(m => m.CompanyID == CompanyID).Max(p => p.SubItemColorCode) + 1;
            }
        }
        public St_SubItemColorH GetSt_SubItemColorHByID(int CompanyID, int SubItemColorCode)
        {
            return _context.St_SubItemColorHs.FirstOrDefault(m => m.CompanyID == CompanyID && m.SubItemColorCode == SubItemColorCode);
        }
        public void Update(St_SubItemColorH ObjUpdate)
        {
            var ObjToUpdate = _context.St_SubItemColorHs.FirstOrDefault(m => m.CompanyID == ObjUpdate.CompanyID && m.SubItemColorCode == ObjUpdate.SubItemColorCode);
            if (ObjToUpdate != null)
            {
                ObjToUpdate.ArabicName = ObjUpdate.ArabicName;
                ObjToUpdate.EnglishName = ObjUpdate.EnglishName;
                ObjToUpdate.InsDateTime = ObjUpdate.InsDateTime;
                ObjToUpdate.InsUserID = ObjUpdate.InsUserID;
            }
        }
    }
}