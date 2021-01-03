using Acc.Models;
using Acc.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Persistence
{
    public class SaleRepo : ISaleRepo
    {
        private readonly ApplicationDbContext _context;

        public SaleRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(Sale ObjSave)
        {
            _context.Sales.Add(ObjSave);
        }

        public void Delete(Sale ObjDelete)
        {
            var ObjToDelete = _context.Sales.SingleOrDefault(m => m.CompanyID == ObjDelete.CompanyID && m.SalesID == ObjDelete.SalesID);
            if (ObjToDelete != null)
            {
                _context.Sales.Remove(ObjToDelete);
            }
        }

        public IEnumerable<Sale> GetAllSale(int CompanyID)
        {
            return _context.Sales.Where(m => m.CompanyID == CompanyID).ToList();
        }

        public int GetMaxSerial(int CompanyID)
        {
            var Obj = _context.Sales.FirstOrDefault(m => m.CompanyID == CompanyID);
            if (Obj == null)
            {
                return 1;
            }
            else
            {
                return _context.Sales.Where(m => m.CompanyID == CompanyID).Max(p => p.SalesID) + 1;
            }
        }

        public Sale GetSaleByID(int CompanyID, int SalesID)
        {
            return _context.Sales.FirstOrDefault(m => m.CompanyID == CompanyID && m.SalesID == SalesID);
        }
        public void Update(Sale ObjUpdate)
        {
            var ObjToUpdate = _context.Sales.FirstOrDefault(m => m.CompanyID == ObjUpdate.CompanyID && m.SalesID == ObjUpdate.SalesID);
            if (ObjToUpdate != null)
            {
                ObjToUpdate.SalesName = ObjUpdate.SalesName;
                ObjToUpdate.InsDateTime = ObjUpdate.InsDateTime;
                ObjToUpdate.InsUserID = ObjUpdate.InsUserID;
            }
        }
    }
}