using Acc.Models;
using Acc.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Persistence
{
    public class CostCenterRepo : ICostCenterRepo
    {
        private readonly ApplicationDbContext _context;
        public CostCenterRepo(ApplicationDbContext context)
        {
            _context = context;
        }
        public void Add(CostCenter ObjToSave)
        {
            _context.CostCenters.Add(ObjToSave);
        }
        public void Delete(CostCenter ObjToSave)
        {
            var ObjToDelete = _context.CostCenters.FirstOrDefault(m => m.CostNumber == ObjToSave.CostNumber
                            && m.CompanyID == ObjToSave.CompanyID);
            if (ObjToDelete != null)
            {
                _context.CostCenters.Remove(ObjToDelete);
            }
        }
        public IEnumerable<CostCenter> GetAllCostCenter(int CompanyID)
        {
            return _context.CostCenters.Where(m => m.CompanyID == CompanyID).ToList();
        }
        public void Update(CostCenter ObjToSave)
        {
            var ObjToUpdate = _context.CostCenters.FirstOrDefault(m => m.CostNumber == ObjToSave.CostNumber
                && m.CompanyID == ObjToSave.CompanyID);
            if (ObjToUpdate != null)
            {
                ObjToUpdate.ArabicName = ObjToSave.ArabicName;
                ObjToUpdate.EnglishName = ObjToSave.EnglishName; 
                ObjToUpdate.Note = ObjToSave.Note;
                ObjToUpdate.StoppedCost = ObjToSave.StoppedCost;



            }
        }
       public    CostCenter  GetCostCenterByFather(int CompanyID, string AccountFather)
        {
            return _context.CostCenters.FirstOrDefault(m => m.CompanyID == CompanyID && m.CostNumber == AccountFather);
        }
        public CostCenter GetCostCenterById(int CompanyID, string AccountFather)
        {
            return _context.CostCenters.FirstOrDefault(m => m.CompanyID == CompanyID && m.CostNumber == AccountFather);
        }
        public IEnumerable<CostCenter> GetCostCenterByLevel(int AccountLevel)
        {
            throw new NotImplementedException();
        }
        public CostCenter GetCostByID(int CompanyID, string CostCenterID)
        {
            return _context.CostCenters.FirstOrDefault(m => m.CostNumber == CostCenterID && m.CompanyID == CompanyID);
        }

    }
}