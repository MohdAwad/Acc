using Acc.Models;
using Acc.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Persistence
{
    public class St_ItemUnitRepo : ISt_ItemUnitRepo
    {

        private readonly ApplicationDbContext _context;

        public St_ItemUnitRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(St_ItemUnit ObjSave)
        {
            _context.St_ItemUnits.Add(ObjSave);
        }

        public void Delete(St_ItemUnit ObjDelete)
        {
            var ObjToDelete = _context.St_ItemUnits.SingleOrDefault(m => m.CompanyID == ObjDelete.CompanyID && m.ItemUnitCode == ObjDelete.ItemUnitCode);
            if (ObjToDelete != null)
            {
                _context.St_ItemUnits.Remove(ObjToDelete);
            }
        }

        public IEnumerable<St_ItemUnit> GetAllItemUnit(int CompanyID)
        {
            return _context.St_ItemUnits.Where(m => m.CompanyID == CompanyID).ToList();
        }

        public int GetMaxSerial(int CompanyID)
        {
            var Obj = _context.St_ItemUnits.FirstOrDefault(m => m.CompanyID == CompanyID);
            if (Obj == null)
            {
                return 1;
            }
            else
            {
                return _context.St_ItemUnits.Where(m => m.CompanyID == CompanyID).Max(p => p.ItemUnitCode) + 1;
            }
        }

        public St_ItemUnit GetItemUnitByID(int CompanyID, int ItemUnitCode)
        {
            return _context.St_ItemUnits.FirstOrDefault(m => m.CompanyID == CompanyID && m.ItemUnitCode == ItemUnitCode);
        }

        public void Update(St_ItemUnit ObjUpdate)
        {
            var ObjToUpdate = _context.St_ItemUnits.FirstOrDefault(m => m.CompanyID == ObjUpdate.CompanyID && m.ItemUnitCode == ObjUpdate.ItemUnitCode);
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