using Acc.Models;
using System.Collections.Generic;

namespace Acc.Repositories
{
    public interface IDrawnBankRepo
    {
        IEnumerable<DrawnBank> GetAllDrawnBank(int CompanyID);
        DrawnBank GetDrawnBankByID(int CompanyID, int BankId);
        void Add(DrawnBank ObjSave);
        void Update(DrawnBank ObjUpdate);
        int GetMaxSerial(int CompanyID);
    }
}