using Acc.Models;
using Acc.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Persistence
{
    public class CustomerCityRepo : ICustomerCityRepo
    {
        private readonly ApplicationDbContext _context;

        public CustomerCityRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(CustomerCity ObjSave)
        {
            _context.CustomerCitys.Add(ObjSave);
        }

        public void Delete(CustomerCity ObjDelete)
        {
            var ObjToDelete = _context.CustomerCitys.SingleOrDefault(m => m.CompanyID == ObjDelete.CompanyID && m.CustomerCityID == ObjDelete.CustomerCityID);
            if (ObjToDelete != null)
            {
                _context.CustomerCitys.Remove(ObjToDelete);
            }
        }

        public IEnumerable<CustomerCity> GetAllCustomerCity(int CompanyID)
        {
            return _context.CustomerCitys.Where(m => m.CompanyID == CompanyID).ToList();
        }

        public int GetMaxSerial(int CompanyID)
        {
            var Obj = _context.CustomerCitys.FirstOrDefault(m => m.CompanyID == CompanyID);
            if (Obj == null)
            {
                return 1;
            }
            else
            {
                return _context.CustomerCitys.Where(m => m.CompanyID == CompanyID).Max(p => p.CustomerCityID) + 1;
            }
        }

        public CustomerCity GetCustomerCityByID(int CompanyID, int CustomerCityID)
        {
            return _context.CustomerCitys.FirstOrDefault(m => m.CompanyID == CompanyID && m.CustomerCityID == CustomerCityID);
        }
        public void Update(CustomerCity ObjUpdate)
        {
            var ObjToUpdate = _context.CustomerCitys.FirstOrDefault(m => m.CompanyID == ObjUpdate.CompanyID && m.CustomerCityID == ObjUpdate.CustomerCityID);
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