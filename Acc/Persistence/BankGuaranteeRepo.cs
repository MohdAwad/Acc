using Acc.Models;
using Acc.Repositories;
using Acc.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Persistence
{
    public class BankGuaranteeRepo : IBankGuaranteeRepo
    {
        private readonly ApplicationDbContext _context;

        public BankGuaranteeRepo(ApplicationDbContext context)
        {
            _context = context;
        }
        public void Add(BankGuarantee ObjToSave)
        {
            _context.BankGuarantees.Add(ObjToSave);
        }

        public void Delete(TransActionDeleteVM ObjToSave)
        {
            var ObjToUpdate = _context.BankGuarantees.FirstOrDefault
              (m => m.CompanyID == ObjToSave.CompanyID
            && m.CompanyTransactionKindNo == ObjToSave.CompanyTransactionKindNo
            && m.CompanyYear == ObjToSave.CompanyYear
            && m.VoucherNumber == ObjToSave.VoucherNumber);

            if (ObjToUpdate != null)
            {
                _context.BankGuarantees.Remove(ObjToUpdate);



            }
        }

        public BankGuarantee GetBankGuarantee(BankGuarantee bankGuarantee)
        {
            return _context.BankGuarantees.FirstOrDefault(m => m.CompanyYear == bankGuarantee.CompanyYear
            && m.VoucherNumber == bankGuarantee.VoucherNumber
            && m.CompanyID == bankGuarantee.CompanyID
            );
        }

        public void Update(BankGuarantee ObjToSave)
        {
            var ObjToUpdate = _context.BankGuarantees.FirstOrDefault
                (m => m.CompanyID == ObjToSave.CompanyID
              && m.CompanyTransactionKindNo == ObjToSave.CompanyTransactionKindNo
              && m.CompanyYear == ObjToSave.CompanyYear
              && m.VoucherNumber == ObjToSave.VoucherNumber);

            if (ObjToUpdate != null)
            {
                ObjToUpdate.DepositValue = ObjToSave.DepositValue;
                ObjToUpdate.DueDate = ObjToSave.DueDate;
                ObjToUpdate.ExpensesAmount = ObjToSave.ExpensesAmount;
                ObjToUpdate.OrderNo = ObjToSave.OrderNo;
                ObjToUpdate.TheBeneficiary = ObjToSave.TheBeneficiary;
                ObjToUpdate.WarrantyAmount = ObjToSave.WarrantyAmount;
                ObjToUpdate.WarrantyNumber = ObjToSave.WarrantyNumber;



            }
        }
    }
}