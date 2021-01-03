using Acc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Repositories
{
    public interface ICurrencyValueRepo
    {
        void Add(CurrencyValue ObjSave);
    }
}