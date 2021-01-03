using Acc.Models;
using Acc.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Persistence
{
    public class ServiceRepo : IServiceRepo
    {
        private readonly ApplicationDbContext _context;

        public ServiceRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public ApplicationDbContext Context => _context;

        public void Add(Service ObjSave)
        {
            Context.Services.Add(ObjSave);
        }

        public void Delete(Service ObjDelete)
        {
            var ObjToDelete = Context.Services.SingleOrDefault(m => m.CompanyID == ObjDelete.CompanyID && m.ServiceID == ObjDelete.ServiceID);
            if (ObjToDelete != null)
            {
                Context.Services.Remove(ObjToDelete);
            }
        }

        public IEnumerable<Service> GetAllService(int CompanyID)
        {
            return Context.Services.Where(m => m.CompanyID == CompanyID).ToList();
        }

        public int GetMaxSerial(int CompanyID)
        {
            var Obj = _context.Services.FirstOrDefault(m => m.CompanyID == CompanyID);
            if (Obj == null)
            {
                return 1;
            }
            else
            {
                return _context.Services.Where(m => m.CompanyID == CompanyID).Max(p => p.ServiceID) + 1;
            }
        }

        public Service GetServiceByID(int CompanyID, int ServiceID)
        {
            return Context.Services.FirstOrDefault(m => m.CompanyID == CompanyID && m.ServiceID == ServiceID);
        }

        public void Update(Service ObjUpdate)
        {
            var ObjToUpdate = Context.Services.FirstOrDefault(m => m.CompanyID == ObjUpdate.CompanyID && m.ServiceID == ObjUpdate.ServiceID);
            if (ObjToUpdate != null)
            {
                ObjToUpdate.ArabicName = ObjUpdate.ArabicName;
                ObjToUpdate.EnglishName = ObjUpdate.EnglishName;
                ObjToUpdate.ServiceGroupID = ObjUpdate.ServiceGroupID;
                ObjToUpdate.Note = ObjUpdate.Note;
                ObjToUpdate.CostPrice = ObjUpdate.CostPrice;
                ObjToUpdate.SalePrice = ObjUpdate.SalePrice;
                ObjToUpdate.TaxPercentage = ObjUpdate.TaxPercentage;
                ObjToUpdate.InsDateTime = ObjUpdate.InsDateTime;
                ObjToUpdate.InsUserID = ObjUpdate.InsUserID;
            }
        }
    }
}