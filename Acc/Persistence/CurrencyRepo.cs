using Acc.Models;
using Acc.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace Acc.Persistence
{
    public class CurrencyRepo : ICurrencyRepo
    {
        private readonly ApplicationDbContext _context;
        public CurrencyRepo(ApplicationDbContext context)
        {
            _context = context;
        }
        public void Add(Currency ObjSave)
        {
            _context.Currencys.Add(ObjSave);
        }

        public void Delete(Currency ObjDelete)
        {
            var ObjToDelete = _context.Currencys.SingleOrDefault(m => m.CompanyID == ObjDelete.CompanyID && m.CurrencyID == ObjDelete.CurrencyID);
            if (ObjToDelete != null)
            {
                _context.Currencys.Remove(ObjToDelete);
            }
        }
        public IEnumerable<Currency> GetAllCurrency(int CompanyID)
        {
            return _context.Currencys.Where(m => m.CompanyID == CompanyID).ToList();
        }

        public Currency GetCurrencyByID(int CompanyID, int CurrencyId)
        {
            return _context.Currencys.FirstOrDefault(m => m.CompanyID == CompanyID && m.CurrencyID == CurrencyId);
        }

        public int GetMaxSerial(int CompanyID)
        {
            var Obj = _context.Currencys.FirstOrDefault(m => m.CompanyID == CompanyID);
            if (Obj == null)
            {
                return 1;
            }
            else
            {
                return _context.Currencys.Where(m => m.CompanyID == CompanyID).Max(p => p.CurrencyID) + 1;
            }
        }
        public void Update(Currency ObjUpdate)
        {
            var ObjToUpdate = _context.Currencys.SingleOrDefault(m => m.CompanyID == ObjUpdate.CompanyID && m.CurrencyID == ObjUpdate.CurrencyID);
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