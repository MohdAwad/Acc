using Acc.Models;
using Acc.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Persistence
{
    public class St_CarpenterHRepo : ISt_CarpenterHRepo
    {

        private readonly ApplicationDbContext _context;
        public St_CarpenterHRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(St_CarpenterH ObjSave)
        {
            _context.St_Carpenters.Add(ObjSave);
        }

        public void Delete(St_CarpenterH ObjDelete)
        {
            var ObjToDelete = _context.St_Carpenters.SingleOrDefault(m => m.CompanyID == ObjDelete.CompanyID && m.CarpenterID == ObjDelete.CarpenterID);
            if (ObjToDelete != null)
            {
                _context.St_Carpenters.Remove(ObjToDelete);
            }
        }

        public IEnumerable<St_CarpenterH> GetAllSt_CarpenterH(int CompanyID)
        {
            return _context.St_Carpenters.Where(m => m.CompanyID == CompanyID).ToList();
        }

        public int GetMaxSerial(int CompanyID)
        {
            var Obj = _context.St_Carpenters.FirstOrDefault(m => m.CompanyID == CompanyID);
            if (Obj == null)
            {
                return 1;
            }
            else
            {
                return _context.St_Carpenters.Where(m => m.CompanyID == CompanyID).Max(p => p.CarpenterID) + 1;
            }
        }

        public St_CarpenterH GetSt_CarpenterHByID(int CompanyID, int CarpenterID)
        {
            return _context.St_Carpenters.FirstOrDefault(m => m.CompanyID == CompanyID && m.CarpenterID == CarpenterID);
        }

        public void Update(St_CarpenterH ObjUpdate)
        {

            var ObjToUpdate = _context.St_Carpenters.SingleOrDefault(m => m.CompanyID == ObjUpdate.CompanyID && m.CarpenterID == ObjUpdate.CarpenterID);
            if (ObjToUpdate != null)
            {
                ObjToUpdate.ArabicName = ObjUpdate.ArabicName;
                ObjToUpdate.EnglishName = ObjUpdate.EnglishName;
                ObjToUpdate.HrID = ObjUpdate.HrID;
                ObjToUpdate.InsDateTime = ObjUpdate.InsDateTime;
                ObjToUpdate.InsUserID = ObjUpdate.InsUserID;

            }
        }
    }
}