using Acc.Models;
using Acc.Repositories;
using Acc.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Persistence
{
    public class AccountTypeRepo : IAccountTypeRepo
    {
        private readonly ApplicationDbContext _context;

        public AccountTypeRepo(ApplicationDbContext context)
        {
            _context = context;
        }
        public AccountType GetAccountTypeById(int AccountTypeID)
        {
          return  _context.AccountTypes.FirstOrDefault(m => m.AccountTypeID == AccountTypeID);
        }
        public IEnumerable<AccountType> GetAllAccountType()
        {
            return _context.AccountTypes.Where(m => m.AccountTypeID > 0);
        }
        public IEnumerable<AccountTypeVM> GetAllAccountTypeVM()
        {
            return _context.Database.SqlQuery<AccountTypeVM>(
                  " Select * from AccountTypes where AccountTypeID  > 0 ").ToList();

        }
    }
}