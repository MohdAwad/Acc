using Acc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Repositories
{
    public interface IAccreditationInformationRepo
    {
        void Add(AccreditationInformation ObjToSave);

        void Update(AccreditationInformation ObjToSave);

        AccreditationInformation GetAccreditationInformation(int CompanyID, string AccountNo);

    }
}