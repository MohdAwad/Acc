using Acc.Models;
using Acc.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Persistence
{
    public class St_SectionsOfFactoryHRepo : ISt_SectionsOfFactoryHRepo
    {
        private readonly ApplicationDbContext _context;

        public St_SectionsOfFactoryHRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(St_SectionsOfFactoryH ObjSave)
        {
            _context.St_SectionsOfFactoryHs.Add(ObjSave);
        }

        public void Delete(St_SectionsOfFactoryH ObjDelete)
        {
            var ObjToDelete = _context.St_SectionsOfFactoryHs.SingleOrDefault(m => m.CompanyID == ObjDelete.CompanyID && m.SectionsOfFactoryID == ObjDelete.SectionsOfFactoryID);
            if (ObjToDelete != null)
            {
                _context.St_SectionsOfFactoryHs.Remove(ObjToDelete);
            }
        }


        public int GetMaxSerial(int CompanyID)
        {
            var Obj = _context.St_SectionsOfFactoryHs.FirstOrDefault(m => m.CompanyID == CompanyID);
            if (Obj == null)
            {
                return 1;
            }
            else
            {
                return _context.St_SectionsOfFactoryHs.Where(m => m.CompanyID == CompanyID).Max(p => p.SectionsOfFactoryID) + 1;
            }
        }
        public void Update(St_SectionsOfFactoryH ObjUpdate)
        {
            var ObjToUpdate = _context.St_SectionsOfFactoryHs.FirstOrDefault(m => m.CompanyID == ObjUpdate.CompanyID && m.SectionsOfFactoryID == ObjUpdate.SectionsOfFactoryID);
            if (ObjToUpdate != null)
            {
                ObjToUpdate.ArabicName = ObjUpdate.ArabicName;
                ObjToUpdate.EnglishName = ObjUpdate.EnglishName;
                ObjToUpdate.FactoryID = ObjUpdate.FactoryID;
            }
        }
        public IEnumerable<St_SectionsOfFactoryH> GetAllSt_SectionsOfFactory(int CompanyID)
        {
            return _context.St_SectionsOfFactoryHs.Where(m => m.CompanyID == CompanyID).ToList();
        }
        public IEnumerable<St_SectionsOfFactoryH> GetAllSt_SectionsOfFactoryBySt_FactoryID(int CompanyID, int FactoryID)
        {
            return _context.St_SectionsOfFactoryHs.Where(m => m.CompanyID == CompanyID && m.FactoryID == FactoryID).ToList();
        }
    }
}