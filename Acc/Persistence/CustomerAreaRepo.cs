using Acc.Models;
using Acc.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Persistence
{
    public class CustomerAreaRepo : ICustomerAreaRepo
    {
        private readonly ApplicationDbContext _context;
        public CustomerAreaRepo(ApplicationDbContext context)
        {
            _context = context;
        }
        public void Add(CustomerArea ObjSave)
        {
            _context.CustomerAreas.Add(ObjSave);
        }
        public void Delete(CustomerArea ObjDelete)
        {
            var ObjToDelete = _context.CustomerAreas.SingleOrDefault(m => m.CompanyID == ObjDelete.CompanyID && m.CustomerAreaID == ObjDelete.CustomerAreaID);
            if (ObjToDelete != null)
            {
                _context.CustomerAreas.Remove(ObjToDelete);
            }
        }
        public int GetMaxSerial(int CompanyID)
        {
            var Obj = _context.CustomerAreas.FirstOrDefault(m => m.CompanyID == CompanyID);
            if (Obj == null)
            {
                return 1;
            }
            else
            {
                return _context.CustomerAreas.Where(m => m.CompanyID == CompanyID).Max(p => p.CustomerAreaID) + 1;
            }
        }
        public void Update(CustomerArea ObjUpdate)
        {
            var ObjToUpdate = _context.CustomerAreas.FirstOrDefault(m => m.CompanyID == ObjUpdate.CompanyID && m.CustomerAreaID == ObjUpdate.CustomerAreaID);
            if (ObjToUpdate != null)
            {
                ObjToUpdate.ArabicName = ObjUpdate.ArabicName;
                ObjToUpdate.EnglishName = ObjUpdate.EnglishName;
                ObjToUpdate.CustomerCityID = ObjUpdate.CustomerCityID;
                ObjToUpdate.InsDateTime = ObjUpdate.InsDateTime;
                ObjToUpdate.InsUserID = ObjUpdate.InsUserID;
            }
        }
        public IEnumerable<CustomerArea> GetAllCustomerArea(int CompanyID)
        {
            return _context.CustomerAreas.Where(m => m.CompanyID == CompanyID).ToList();
        }
        public IEnumerable<CustomerArea> GetAllCustomerAreaByCityID(int CompanyID,int CustomerCityID)
        {
            return _context.CustomerAreas.Where(m => m.CompanyID == CompanyID && m.CustomerCityID == CustomerCityID).ToList();
        }
    }
}