using Acc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Repositories
{
    public interface ISt_AdvertisingRepresentativeRepo
    {

        IEnumerable<St_AdvertisingRepresentative> GetAllAdvertisingRepresentative(int CompanyID);
        St_AdvertisingRepresentative GetSt_AdvertisingRepresentativeByID(int CompanyID, int AdvertisingRepresentativeID);
        void Add(St_AdvertisingRepresentative ObjSave);
        void Update(St_AdvertisingRepresentative ObjUpdate);
        void Delete(St_AdvertisingRepresentative ObjDelete);
        int GetMaxSerial(int CompanyID);
    }
}