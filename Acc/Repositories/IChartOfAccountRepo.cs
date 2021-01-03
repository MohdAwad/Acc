using Acc.Models;
using System.Collections.Generic;

namespace Acc.Repositories
{
    public interface IChartOfAccountRepo
    {
        void Add(ChartOfAccount ObjToSave);
        void Update(ChartOfAccount ObjToSave);

        void Delete(ChartOfAccount ObjToSave);

        IEnumerable<ChartOfAccount> GetAllChartOfAccount(int CompanyID);

        ChartOfAccount GetChartOfAccountById(int CompanyID, string AccountFather);

        ChartOfAccount GetAccountByID(int CompanyID, string AccountID);

        IEnumerable<ChartOfAccount> GetChartOfAccountByLevel(int AccountLevel);

    }
}