using Acc.Models;
using Acc.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Repositories
{
    public interface IAccountTypeRepo
    {
        IEnumerable<AccountTypeVM> GetAllAccountTypeVM();
        IEnumerable<AccountType> GetAllAccountType();
        AccountType GetAccountTypeById(int AccountTypeID);
    }
}