using Acc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Repositories
{
    public interface ISt_CategoryPriceRepo
    {
        void Update(St_CategoryPrice ObjUpdate);
        St_CategoryPrice GetSt_CategoryPriceByID(int CompanyID, int CategoryPriceID);
        IEnumerable<St_CategoryPrice> GetAllSt_CategoryPrice(int CompanyID);
        string GetSt_CategoryPriceNameByID(int CompanyID, int CategoryPriceID);
    }
}