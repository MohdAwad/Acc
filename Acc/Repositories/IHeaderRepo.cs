using Acc.Models;
using Acc.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Repositories
{
    public interface IHeaderRepo
    {
        void Add(Header ObjToSave);
        void Update(Header ObjToSave);
        void Delete(TransActionDeleteVM ObjToSave);
        int GetMaxVHByKind(int CompanyID, int CompanyTransactionKindNo,int TransactionKindNo, int CompanyYear);
        IEnumerable<Header> GetHeaderData(DateTime Fromdate, DateTime ToDate, int CompanyID, int CompanyYear);

        Header GetHeaderDataById(string VoucherNumber, int CompanyID, int CompanyTransactionKindNo, int TransactionKindNo,int CompanyYear);

        Header GetHeaderDataByTransId(string VoucherNumber, int CompanyID, int CompanyTransactionKindNo, int TransactionKindNo, int CompanyYear);
        void UpdateToExportAndUnExport(Header ObjToUpdate);

    }
}