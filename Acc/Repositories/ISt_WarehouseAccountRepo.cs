using Acc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Repositories
{
    public interface ISt_WarehouseAccountRepo
    {
        void Add(St_WarehouseAccount ObjSave);
        St_WarehouseAccount GetSt_WarehouseAccountByID(int CompanyID, string StockCode);
        void Delete(St_WarehouseAccount ObjDelete);
    }
}