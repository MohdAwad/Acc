using Acc.Models;
using Acc.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Persistence
{
    public class St_SalesManHRepo : ISt_SalesManHRepo
    {


        private readonly ApplicationDbContext _context;
        public St_SalesManHRepo(ApplicationDbContext context)
        {
            _context = context;
        }
        public void Add(St_SalesManH ObjSave)
        {
            _context.St_SalesManHs.Add(ObjSave);
        }

        public void Delete(St_SalesManH ObjDelete)
        {
            var ObjToDelete = _context.St_SalesManHs.SingleOrDefault(m => m.CompanyID == ObjDelete.CompanyID && m.SalesManID == ObjDelete.SalesManID);
            if (ObjToDelete != null)
            {
                _context.St_SalesManHs.Remove(ObjToDelete);
            }
        }

        public IEnumerable<St_SalesManH> GetAllSt_SalesManH(int CompanyID)
        {
            return _context.St_SalesManHs.Where(m => m.CompanyID == CompanyID).ToList();
        }

        public int GetMaxSerial(int CompanyID)
        {
            var Obj = _context.St_SalesManHs.FirstOrDefault(m => m.CompanyID == CompanyID);
            if (Obj == null)
            {
                return 1;
            }
            else
            {
                return _context.St_SalesManHs.Where(m => m.CompanyID == CompanyID).Max(p => p.SalesManID) + 1;
            }
        }

        public St_SalesManH GetSt_SalesManHByID(int CompanyID, int SalesManID)
        {
            return _context.St_SalesManHs.FirstOrDefault(m => m.CompanyID == CompanyID && m.SalesManID == SalesManID);
        }

        public void Update(St_SalesManH ObjUpdate)
        {
            var ObjToUpdate = _context.St_SalesManHs.SingleOrDefault(m => m.CompanyID == ObjUpdate.CompanyID && m.SalesManID == ObjUpdate.SalesManID);
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