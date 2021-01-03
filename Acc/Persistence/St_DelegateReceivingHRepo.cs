using Acc.Models;
using Acc.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Persistence
{
    public class St_DelegateReceivingHRepo : ISt_DelegateReceivingHRepo
    {
        private readonly ApplicationDbContext _context;

        public St_DelegateReceivingHRepo(ApplicationDbContext context)
        {
            _context = context;
        }


        public void Add(St_DelegateReceivingH ObjSave)
        {
            _context.St_DelegateReceivingHs.Add(ObjSave);
        }

        public void Delete(St_DelegateReceivingH ObjDelete)
        {
            var ObjToDelete = _context.St_DelegateReceivingHs.SingleOrDefault(m => m.CompanyID == ObjDelete.CompanyID && m.DelegateReceivingID == ObjDelete.DelegateReceivingID);
            if (ObjToDelete != null)
            {
                _context.St_DelegateReceivingHs.Remove(ObjToDelete);
            }
        }

        public IEnumerable<St_DelegateReceivingH> GetAllDelegateReceivingH(int CompanyID)
        {
            return _context.St_DelegateReceivingHs.Where(m => m.CompanyID == CompanyID).ToList();
        }

        public int GetMaxSerial(int CompanyID)
        {
            var Obj = _context.St_DelegateReceivingHs.FirstOrDefault(m => m.CompanyID == CompanyID);
            if (Obj == null)
            {
                return 1;
            }
            else
            {
                return _context.St_DelegateReceivingHs.Where(m => m.CompanyID == CompanyID).Max(p => p.DelegateReceivingID) + 1;
            }
        }

        public St_DelegateReceivingH GetSt_DelegateReceivingHByID(int CompanyID, int DelegateReceivingID)
        {
            return _context.St_DelegateReceivingHs.FirstOrDefault(m => m.CompanyID == CompanyID && m.DelegateReceivingID == DelegateReceivingID);
        }

        public void Update(St_DelegateReceivingH ObjUpdate)
        {
            var ObjToUpdate = _context.St_DelegateReceivingHs.FirstOrDefault(m => m.CompanyID == ObjUpdate.CompanyID && m.DelegateReceivingID == ObjUpdate.DelegateReceivingID);
            if (ObjToUpdate != null)
            {
                ObjToUpdate.ArabicName = ObjUpdate.ArabicName;
                ObjToUpdate.EnglishName = ObjUpdate.EnglishName;
                ObjToUpdate.InsDateTime = ObjUpdate.InsDateTime;
                ObjToUpdate.InsUserID = ObjUpdate.InsUserID;
                ObjToUpdate.Telephone = ObjUpdate.Telephone;
                ObjToUpdate.HrID = ObjUpdate.HrID;

            }
        }
    }
}