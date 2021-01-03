using Acc.Models;
using Acc.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Repositories
{
    public interface ISt_FundingAgencyHRepo
    {
        void Add(St_FundingAgencyH ObjSave);

        void Update(St_FundingAgencyHVM ObjUpdate);
        St_FundingAgencyH GetSt_FundingAgencyHByID(int CompanyID, int FundingAgencyID);
        IEnumerable<St_FundingAgencyH> GetAllSt_FundingAgencyH(int CompanyID);

        void Delete(St_FundingAgencyHVM ObjDelete);

        int GetMaxSerial(int CompanyID);

    }
}