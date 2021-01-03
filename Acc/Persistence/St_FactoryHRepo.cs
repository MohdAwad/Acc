using Acc.Models;
using Acc.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Persistence
{
    public class St_FactoryHRepo : ISt_FactoryHRepo
    {
        private readonly ApplicationDbContext _context;
        public St_FactoryHRepo(ApplicationDbContext context)
        {
            _context = context;
        }
        public void Add(St_FactoryH ObjSave)
        {
            _context.St_FactoryHs.Add(ObjSave);
        }
        public void Delete(St_FactoryH ObjDelete)
        {
            var ObjToDelete = _context.St_FactoryHs.SingleOrDefault(m => m.CompanyID == ObjDelete.CompanyID && m.FactoryID == ObjDelete.FactoryID);
            if (ObjToDelete != null)
            {
                _context.St_FactoryHs.Remove(ObjToDelete);
            }
        }
        public IEnumerable<St_FactoryH> GetAllSt_FactoryH(int CompanyID)
        {
            return _context.St_FactoryHs.Where(m => m.CompanyID == CompanyID).ToList();
        }
        public int GetMaxSerial(int CompanyID)
        {
            var Obj = _context.St_FactoryHs.FirstOrDefault(m => m.CompanyID == CompanyID);
            if (Obj == null)
            {
                return 1;
            }
            else
            {
                return _context.St_FactoryHs.Where(m => m.CompanyID == CompanyID).Max(p => p.FactoryID) + 1;
            }
        }
        public St_FactoryH GetSt_FactoryHByID(int CompanyID, int FactoryID)
        {
            return _context.St_FactoryHs.FirstOrDefault(m => m.CompanyID == CompanyID && m.FactoryID == FactoryID);
        }
        public void Update(St_FactoryH ObjUpdate)
        {
            var ObjToUpdate = _context.St_FactoryHs.FirstOrDefault(m => m.CompanyID == ObjUpdate.CompanyID && m.FactoryID == ObjUpdate.FactoryID);
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