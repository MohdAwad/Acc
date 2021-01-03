using Acc.Models;
using Acc.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Repositories
{
    public interface IBankGuaranteeRepo
    {
        void Add(BankGuarantee ObjToSave);
        
        void Update(BankGuarantee ObjToSave);

        void Delete(TransActionDeleteVM ObjToSave);
        BankGuarantee GetBankGuarantee(BankGuarantee bankGuarantee);





    }
}