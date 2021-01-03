using Acc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Repositories
{
    public interface ISt_WarehouseHRepo
    {
        void Add(St_WarehouseH ObjSave);
        void Update(St_WarehouseH ObjUpdate);
        void Delete(St_WarehouseH ObjDelete);
        St_WarehouseH GetSt_WarehouseHByID(int CompanyID, string StockCode);
        string CheckIfStockCodeExisting(int CompanyID, string StockCode);
        IEnumerable<St_WarehouseH> GetAllSt_WarehouseH(int CompanyID);
    }
}