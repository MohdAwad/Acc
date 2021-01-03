using Acc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Repositories
{
    public interface ISt_DescriptionDetailRepo
    {
        void Add(St_DescriptionDetail ObjSave);
        void Update(St_DescriptionDetail ObjUpdate);
        void Delete(St_DescriptionDetail ObjDelete);
        int GetMaxSerial(int CompanyID, int DescriptionID);
        IEnumerable<St_DescriptionDetail> GetAllSt_DescriptionDetail(int CompanyID);
        IEnumerable<St_DescriptionDetail> GetSt_DescriptionDetailBySt_Description(int CompanyID, int DescriptionID);
        St_DescriptionDetail GetSt_DescriptionDetailByID(int CompanyID, int DescriptionDetailID, int DescriptionID);
    }
}