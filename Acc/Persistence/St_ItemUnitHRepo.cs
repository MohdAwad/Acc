using Acc.Models;
using Acc.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Persistence
{
    public class St_ItemUnitHRepo : ISt_ItemUnitHRepo
    {
        private readonly ApplicationDbContext _context;
        public St_ItemUnitHRepo(ApplicationDbContext context)
        {
            _context = context;
        }
        public void Add(St_ItemUnitH ObjSave)
        {
            _context.St_ItemUnitHs.Add(ObjSave);
        }
        public void Delete(St_ItemUnitH ObjDelete)
        {
            var ObjToDelete = _context.St_ItemUnitHs.SingleOrDefault(m => m.CompanyID == ObjDelete.CompanyID && m.ItemUnitCode == ObjDelete.ItemUnitCode);
            if (ObjToDelete != null)
            {
                _context.St_ItemUnitHs.Remove(ObjToDelete);
            }
        }
        public IEnumerable<St_ItemUnitH> GetAllSt_ItemUnitH(int CompanyID)
        {
            return _context.St_ItemUnitHs.Where(m => m.CompanyID == CompanyID).ToList();
        }
        public int GetMaxSerial(int CompanyID)
        {
            var Obj = _context.St_ItemUnitHs.FirstOrDefault(m => m.CompanyID == CompanyID);
            if (Obj == null)
            {
                return 1;
            }
            else
            {
                return _context.St_ItemUnitHs.Where(m => m.CompanyID == CompanyID).Max(p => p.ItemUnitCode) + 1;
            }
        }
        public St_ItemUnitH GetSt_ItemUnitHByID(int CompanyID, int ItemUnitCode)
        {
            return _context.St_ItemUnitHs.FirstOrDefault(m => m.CompanyID == CompanyID && m.ItemUnitCode == ItemUnitCode);
        }
        public void Update(St_ItemUnitH ObjUpdate)
        {
            var ObjToUpdate = _context.St_ItemUnitHs.FirstOrDefault(m => m.CompanyID == ObjUpdate.CompanyID && m.ItemUnitCode == ObjUpdate.ItemUnitCode);
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