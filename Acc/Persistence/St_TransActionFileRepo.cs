using Acc.Models;
using Acc.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Persistence
{

    public class St_TransActionFileRepo : ISt_TransActionFileRepo
    {

        private readonly ApplicationDbContext _context;

        public St_TransActionFileRepo(ApplicationDbContext context)
        {
            _context = context;
        }


        public void Add(St_TransActionFile ObjToSave)
        {
            _context.St_TransActionFiles.Add(ObjToSave);
        }

        public void Delete(int id)
        {
            try
            {
                var D = _context.St_TransActionFiles.FirstOrDefault(m => m.Id == id);
                if (D != null)
                {
                    _context.St_TransActionFiles.Remove(D);
                }
            }
            catch (Exception ex)
            {
                string er = ex.Message;
            }

        }


        public IEnumerable<St_TransActionFile> GetAllTransactionFile(int year, string VoucherNumber, string TransactionKindNo, string CompanyTransactionKindNo)
        {
            return _context.St_TransActionFiles.Where(m => m.Year == year && m.VoucherNumber == VoucherNumber && m.TransactionKindNo == TransactionKindNo
            && m.CompanyTransactionKindNo == CompanyTransactionKindNo).ToList();
        }

        public IEnumerable<St_TransActionFile> GetAllTransactionFileBank(int year, string VoucherNumber, string TransactionKindNo, string CompanyTransactionKindNo, int CompanyId)
        {
            return _context.St_TransActionFiles.Where(m => m.Year == year && m.VoucherNumber == VoucherNumber && m.TransactionKindNo == TransactionKindNo
            && m.CompanyTransactionKindNo == CompanyTransactionKindNo && m.CompanyId == CompanyId).ToList();
        }

        public int GetMaxSerial(int CompanyID)
        {
            var Obj = _context.St_TransActionFiles.FirstOrDefault(m => m.CompanyId == CompanyID);
            if (Obj == null)
            {
                return 1;
            }
            else
            {
                return _context.St_TransActionFiles.Where(m => m.CompanyId == CompanyID).Max(p => p.Id) + 1;
            }
        }

        public St_TransActionFile GetTransActionFilebyId(int id)
        {
            return _context.St_TransActionFiles.FirstOrDefault(m => m.Id == id);
        }

        public IEnumerable<St_TransActionFile> GetType(int CompanyID)
        {
            try
            {
                return _context.St_TransActionFiles.Where(m => m.CompanyId == CompanyID).ToList();
            }
            catch (Exception ex)
            {
                return new List<St_TransActionFile>();
            }

        }

        public void Update(St_TransActionFile ObjToUpdate)
        {

            try
            {
                var D = _context.St_TransActionFiles.FirstOrDefault(m => m.Id == ObjToUpdate.Id && m.CompanyTransactionKindNo == ObjToUpdate.CompanyTransactionKindNo);
                if (D != null)
                {

                    D.VoucherNumber = ObjToUpdate.VoucherNumber;
                    D.TransactionKindNo = ObjToUpdate.TransactionKindNo;
                    D.FileName = ObjToUpdate.FileName;




                }
            }
            catch (Exception ex)
            {
                string er = ex.Message;
            }

        }

    }
}