using Acc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Repositories
{
    public interface ISt_DelegateReceivingHRepo
    {

        IEnumerable<St_DelegateReceivingH> GetAllDelegateReceivingH(int CompanyID);
        St_DelegateReceivingH GetSt_DelegateReceivingHByID(int CompanyID, int DelegateReceivingID);
        void Add(St_DelegateReceivingH ObjSave);
        void Update(St_DelegateReceivingH ObjUpdate);
        void Delete(St_DelegateReceivingH ObjDelete);
        int GetMaxSerial(int CompanyID);
    }
}