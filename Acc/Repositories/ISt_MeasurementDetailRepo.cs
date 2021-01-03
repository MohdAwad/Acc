using Acc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Repositories
{
    public interface ISt_MeasurementDetailRepo
    {
        void Add(St_MeasurementDetail ObjSave);
        void Update(St_MeasurementDetail ObjUpdate);
        void Delete(St_MeasurementDetail ObjDelete);
        int GetMaxSerial(int CompanyID, int MeasurementID);
        IEnumerable<St_MeasurementDetail> GetAllSt_MeasurementDetail(int CompanyID);
        IEnumerable<St_MeasurementDetail> GetSt_MeasurementDetailBySt_Measurement(int CompanyID, int MeasurementID);
        St_MeasurementDetail GetSt_MeasurementDetailByID(int CompanyID, int MeasurementDetailID, int MeasurementID);
    }
}