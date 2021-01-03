using Acc.Models;
using System.Collections.Generic;

namespace Acc.Repositories
{
    public interface ICurrencyRepo
    {
        IEnumerable<Currency> GetAllCurrency(int CompanyID);
        Currency GetCurrencyByID(int CompanyID, int CurrencyId);
        void Add(Currency ObjSave);
        void Update(Currency ObjUpdate);
        void Delete(Currency ObjDelete);
        int GetMaxSerial(int CompanyID);
    }
}