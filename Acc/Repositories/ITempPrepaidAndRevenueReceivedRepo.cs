using Acc.Models;
using Acc.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Repositories
{
    public interface ITempPrepaidAndRevenueReceivedRepo
    {
        int GetMaxVHByKind(int CompanyID, int CompanyTransactionKindNo, int TransactionKindNo , int CompanyYear);
        void Add(TempPrepaidAndRevenueReceived ObjToSave);
        TempPrepaidAndRevenueReceived GetTempPrepaidAndRevenueReceivedData(string VoucherNumber, int CompanyID, int CompanyTransactionKindNo, int TransactionKindNo , int CompanyYear);
        void Delete(TempPrepaidAndRevenueReceivedVM ObjToDelete);
        void Update(TempPrepaidAndRevenueReceived ObjToUpdate);
    }
}