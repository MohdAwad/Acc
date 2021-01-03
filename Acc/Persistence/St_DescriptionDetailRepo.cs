using Acc.Models;
using Acc.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Persistence
{
    public class St_DescriptionDetailRepo : ISt_DescriptionDetailRepo
    {
        private readonly ApplicationDbContext _context;
        public St_DescriptionDetailRepo(ApplicationDbContext context)
        {
            _context = context;
        }
        public void Add(St_DescriptionDetail ObjSave)
        {
            _context.St_DescriptionDetails.Add(ObjSave);
        }
        public void Delete(St_DescriptionDetail ObjDelete)
        {
            var ObjToDelete = _context.St_DescriptionDetails.SingleOrDefault(m => m.CompanyID == ObjDelete.CompanyID && m.DescriptionDetailID == ObjDelete.DescriptionDetailID && m.DescriptionID == ObjDelete.DescriptionID);
            if (ObjToDelete != null)
            {
                _context.St_DescriptionDetails.Remove(ObjToDelete);
            }
        }
        public int GetMaxSerial(int CompanyID, int DescriptionID)
        {
            var Obj = _context.St_DescriptionDetails.FirstOrDefault(m => m.CompanyID == CompanyID && m.DescriptionID == DescriptionID);
            if (Obj == null)
            {
                return 1;
            }
            else
            {
                return _context.St_DescriptionDetails.Where(m => m.CompanyID == CompanyID && m.DescriptionID == DescriptionID).Max(p => p.DescriptionDetailID) + 1;
            }
        }
        public void Update(St_DescriptionDetail ObjUpdate)
        {
            var ObjToUpdate = _context.St_DescriptionDetails.FirstOrDefault(m => m.CompanyID == ObjUpdate.CompanyID && m.DescriptionDetailID == ObjUpdate.DescriptionDetailID && m.DescriptionID == ObjUpdate.DescriptionID);
            if (ObjToUpdate != null)
            {
                ObjToUpdate.ArabicName = ObjUpdate.ArabicName;
                ObjToUpdate.EnglishName = ObjUpdate.EnglishName;
                ObjToUpdate.InsUserID = ObjUpdate.InsUserID;
                ObjToUpdate.InsDateTime = ObjUpdate.InsDateTime;
            }
        }
        public IEnumerable<St_DescriptionDetail> GetAllSt_DescriptionDetail(int CompanyID)
        {
            return _context.St_DescriptionDetails.Where(m => m.CompanyID == CompanyID).ToList();
        }
        public IEnumerable<St_DescriptionDetail> GetSt_DescriptionDetailBySt_Description(int CompanyID, int DescriptionID)
        {
            return _context.St_DescriptionDetails.Where(m => m.CompanyID == CompanyID && m.DescriptionID == DescriptionID).ToList();
        }
        public St_DescriptionDetail GetSt_DescriptionDetailByID(int CompanyID,int DescriptionDetailID, int DescriptionID)
        {
            return _context.St_DescriptionDetails.FirstOrDefault(m => m.CompanyID == CompanyID && m.DescriptionDetailID == DescriptionDetailID && m.DescriptionID == DescriptionID);
        }
    }
}