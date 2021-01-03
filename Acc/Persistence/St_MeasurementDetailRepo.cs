using Acc.Models;
using Acc.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace Acc.Persistence
{
    public class St_MeasurementDetailRepo : ISt_MeasurementDetailRepo
    {
        private readonly ApplicationDbContext _context;
        public St_MeasurementDetailRepo(ApplicationDbContext context)
        {
            _context = context;
        }
        public void Add(St_MeasurementDetail ObjSave)
        {
            _context.St_MeasurementDetails.Add(ObjSave);
        }
        public void Delete(St_MeasurementDetail ObjDelete)
        {
            var ObjToDelete = _context.St_MeasurementDetails.SingleOrDefault(m => m.CompanyID == ObjDelete.CompanyID && m.MeasurementDetailID == ObjDelete.MeasurementDetailID && m.MeasurementID == ObjDelete.MeasurementID);
            if (ObjToDelete != null)
            {
                _context.St_MeasurementDetails.Remove(ObjToDelete);
            }
        }
        public int GetMaxSerial(int CompanyID, int MeasurementID)
        {
            var Obj = _context.St_MeasurementDetails.FirstOrDefault(m => m.CompanyID == CompanyID && m.MeasurementID == MeasurementID);
            if (Obj == null)
            {
                return 1;
            }
            else
            {
                return _context.St_MeasurementDetails.Where(m => m.CompanyID == CompanyID && m.MeasurementID == MeasurementID).Max(p => p.MeasurementDetailID) + 1;
            }
        }
        public void Update(St_MeasurementDetail ObjUpdate)
        {
            var ObjToUpdate = _context.St_MeasurementDetails.FirstOrDefault(m => m.CompanyID == ObjUpdate.CompanyID && m.MeasurementDetailID == ObjUpdate.MeasurementDetailID && m.MeasurementID == ObjUpdate.MeasurementID);
            if (ObjToUpdate != null)
            {
                ObjToUpdate.ArabicName = ObjUpdate.ArabicName;
                ObjToUpdate.EnglishName = ObjUpdate.EnglishName;
                ObjToUpdate.InsUserID = ObjUpdate.InsUserID;
                ObjToUpdate.InsDateTime = ObjUpdate.InsDateTime;
            }
        }
        public IEnumerable<St_MeasurementDetail> GetAllSt_MeasurementDetail(int CompanyID)
        {
            return _context.St_MeasurementDetails.Where(m => m.CompanyID == CompanyID).ToList();
        }
        public IEnumerable<St_MeasurementDetail> GetSt_MeasurementDetailBySt_Measurement(int CompanyID, int MeasurementID)
        {
            return _context.St_MeasurementDetails.Where(m => m.CompanyID == CompanyID && m.MeasurementID == MeasurementID).ToList();
        }
        public St_MeasurementDetail GetSt_MeasurementDetailByID(int CompanyID, int MeasurementDetailID, int MeasurementID)
        {
            return _context.St_MeasurementDetails.FirstOrDefault(m => m.CompanyID == CompanyID && m.MeasurementDetailID == MeasurementDetailID && m.MeasurementID == MeasurementID);
        }
    }
}