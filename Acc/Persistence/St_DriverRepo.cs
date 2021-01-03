using Acc.Models;
using Acc.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Persistence
{
    public class St_DriverRepo : ISt_DriverRepo
    {
        private readonly ApplicationDbContext _context;

        public St_DriverRepo(ApplicationDbContext context)
        {
            _context = context;
        }


        public void Add(St_Driver ObjSave)
        {
            _context.St_Drivers.Add(ObjSave);
        }

        public void Delete(St_Driver ObjDelete)
        {
            var ObjToDelete = _context.St_Drivers.SingleOrDefault(m => m.CompanyID == ObjDelete.CompanyID && m.DriverID == ObjDelete.DriverID);
            if (ObjToDelete != null)
            {
                _context.St_Drivers.Remove(ObjToDelete);
            }
        }

        public IEnumerable<St_Driver> GetAllDriver(int CompanyID)
        {
            return _context.St_Drivers.Where(m => m.CompanyID == CompanyID).ToList();
        }

        public int GetMaxSerial(int CompanyID)
        {
            var Obj = _context.St_Drivers.FirstOrDefault(m => m.CompanyID == CompanyID);
            if (Obj == null)
            {
                return 1;
            }
            else
            {
                return _context.St_Drivers.Where(m => m.CompanyID == CompanyID).Max(p => p.DriverID) + 1;
            }
        }

        public St_Driver GetSt_DriverByID(int CompanyID, int DriverID)
        {
            return _context.St_Drivers.FirstOrDefault(m => m.CompanyID == CompanyID && m.DriverID == DriverID);
        }

        public void Update(St_Driver ObjUpdate)
        {
            var ObjToUpdate = _context.St_Drivers.FirstOrDefault(m => m.CompanyID == ObjUpdate.CompanyID && m.DriverID == ObjUpdate.DriverID);
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