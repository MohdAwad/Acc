using Acc.Models;
using Acc.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Persistence
{
    public class ServiceGroupRepo : IServiceGroupRepo
    {
        private readonly ApplicationDbContext _context;
        public ServiceGroupRepo(ApplicationDbContext context)
        {
            _context = context;
        }
        public void Add(ServiceGroup ObjSave)
        {
            _context.ServiceGroups.Add(ObjSave);
        }

        public void Delete(ServiceGroup ObjDelete)
        {
            var ObjToDelete = _context.ServiceGroups.SingleOrDefault(m => m.CompanyID == ObjDelete.CompanyID && m.ServiceGroupID == ObjDelete.ServiceGroupID);
            if (ObjToDelete != null)
            {
                _context.ServiceGroups.Remove(ObjToDelete);
            }
        }

        public IEnumerable<ServiceGroup> GetAllServiceGroup(int CompanyID)
        {
            return _context.ServiceGroups.Where(m => m.CompanyID == CompanyID).ToList();
        }

        public int GetMaxSerial(int CompanyID)
        {
            var Obj = _context.ServiceGroups.FirstOrDefault(m => m.CompanyID == CompanyID);
            if (Obj == null)
            {
                return 1;
            }
            else
            {
                return _context.ServiceGroups.Where(m => m.CompanyID == CompanyID).Max(p => p.ServiceGroupID) + 1;
            }
        }

        public ServiceGroup GetServiceGroupByID(int CompanyID, int ServiceGroupID)
        {
            return _context.ServiceGroups.FirstOrDefault(m => m.CompanyID == CompanyID && m.ServiceGroupID == ServiceGroupID);
        }

        public void Update(ServiceGroup ObjUpdate)
        {
            var ObjToUpdate = _context.ServiceGroups.SingleOrDefault(m => m.CompanyID == ObjUpdate.CompanyID && m.ServiceGroupID == ObjUpdate.ServiceGroupID);
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