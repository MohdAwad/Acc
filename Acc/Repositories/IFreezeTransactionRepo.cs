using Acc.Models;
using Acc.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace Acc.Repositories
{
    public interface IFreezeTransactionRepo
    {
        void Add(FreezeTransaction ObjSave);
        void Delete(FreezeTransaction ObjDelete);
        FreezeTransaction GetFreezeTransactionByID(int CompanyID, int Id);
    }
}