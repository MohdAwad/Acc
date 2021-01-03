using Acc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Repositories
{
    public interface ICompanyTransactionKindMonthlySerialRepo
    {
        void Add(CompanyTransactionKindMonthlySerial ObjSave);
        int GetMaxSerial(int CompanyID,int CompanyTransactionKindID, int MonthID);
        void Update(int CompanyID, int CompanyTransactionKindID,int MonthID);
        void Delete(int CompanyID, int CompanyTransactionKindID);
        CompanyTransactionKindMonthlySerial CheckIfSave(int CompanyID, int CompanyTransactionKindID);
    }
}