using Acc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web;

namespace Acc.Repositories
{
    public interface IServiceBillHeaderRepo
    {

        void Add(HeaderServiceBill ObjToSave);
        void Update(HeaderServiceBill ObjToSave);
        void Delete( int CompanyID, int VoucherNumber, int CompanyTransactionKindNo, int TransactionKindNo, int CompanyYear);
        int GetMaxVHByKind(int CompanyID,  int TransactionKindNo, int CompanyYear);
        IEnumerable<HeaderServiceBill> GetHeaderData(DateTime Fromdate, DateTime ToDate, int CompanyID, int ServiceType, int CompanyYear);

        HeaderServiceBill GetHeaderDataById(int CompanyID, int VoucherNumber,  int CompanyTransactionKindNo, int TransactionKindNo, int CompanyYear);

        
        void UpdateToExportAndUnExport(HeaderServiceBill ObjToUpdate);

    }
}