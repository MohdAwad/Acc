using Acc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Repositories
{
    public interface IDefinitionBankRepo
    {
        void Add(DefinitionBank ObjSave);
        IEnumerable<DefinitionBank> GetAllDefinitionBank(int CompanyID);
        void Update(DefinitionBank ObjUpdate);
        void Delete(DefinitionBank ObjDelete);

        void UpdateDeleteFlag(DefinitionBank ObjUpdate);
        DefinitionBank GetDefinitionBankByID(int CompanyID, int BankID);
        int GetMaxSerial(int CompanyID);
    }
}