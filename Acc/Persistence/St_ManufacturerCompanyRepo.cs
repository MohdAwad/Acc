using Acc.Models;
using Acc.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Persistence
{
    public class St_ManufacturerCompanyRepo : ISt_ManufacturerCompanyRepo
    {
        private readonly ApplicationDbContext _context;

        public St_ManufacturerCompanyRepo(ApplicationDbContext context)
        {
            _context = context;
        }


        public void Add(St_ManufacturerCompany ObjSave)
        {
            _context.St_ManufacturerCompanies.Add(ObjSave);
        }

        public void Delete(St_ManufacturerCompany ObjDelete)
        {
            var ObjToDelete = _context.St_ManufacturerCompanies.SingleOrDefault(m => m.CompanyID == ObjDelete.CompanyID && m.ManufacturerCompanyID == ObjDelete.ManufacturerCompanyID);
            if (ObjToDelete != null)
            {
                _context.St_ManufacturerCompanies.Remove(ObjToDelete);
            }
        }

        public IEnumerable<St_ManufacturerCompany> GetAllSt_ManufacturerCompany(int CompanyID)
        {
            return _context.St_ManufacturerCompanies.Where(m => m.CompanyID == CompanyID).ToList();
        }

        public int GetMaxSerial(int CompanyID)
        {
            var Obj = _context.St_ManufacturerCompanies.FirstOrDefault(m => m.CompanyID == CompanyID);
            if (Obj == null)
            {
                return 1;
            }
            else
            {
                return _context.St_ManufacturerCompanies.Where(m => m.CompanyID == CompanyID).Max(p => p.ManufacturerCompanyID) + 1;
            }
        }

        public St_ManufacturerCompany GetSt_ManufacturerCompanyByID(int CompanyID, int ManufacturerCompanyID)
        {
            return _context.St_ManufacturerCompanies.FirstOrDefault(m => m.CompanyID == CompanyID && m.ManufacturerCompanyID == ManufacturerCompanyID);
        }

        public void Update(St_ManufacturerCompany ObjUpdate)
        {
            var ObjToUpdate = _context.St_ManufacturerCompanies.FirstOrDefault(m => m.CompanyID == ObjUpdate.CompanyID && m.ManufacturerCompanyID == ObjUpdate.ManufacturerCompanyID);
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