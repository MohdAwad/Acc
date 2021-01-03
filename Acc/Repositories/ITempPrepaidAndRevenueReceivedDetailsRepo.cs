using Acc.Models;
using Acc.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Repositories
{
    public interface ITempPrepaidAndRevenueReceivedDetailsRepo
    {
        void Add(TempPrepaidAndRevenueReceivedDetail ObjToSave);
        void UpdateToExport(TempPrepaidAndRevenueReceivedDetail ObjToUpdate);
        TempPrepaidAndRevenueReceivedDetail GetTempPrepaidAndRevenueReceivedDetailData(string VoucherNumber, int CompanyID, int CompanyTransactionKindNo, int TransactionKindNo , int CompanyYear);
    }
}