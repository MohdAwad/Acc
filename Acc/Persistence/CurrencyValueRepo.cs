using Acc.Models;
using Acc.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Persistence
{
    public class CurrencyValueRepo : ICurrencyValueRepo
    {
        private readonly ApplicationDbContext _context;
        public CurrencyValueRepo(ApplicationDbContext context)
        {
            _context = context;
        }
        public void Add(CurrencyValue ObjSave)
        {
            _context.CurrencyValues.Add(ObjSave);
        }
    }
}