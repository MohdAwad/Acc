using Acc.Models;
using Acc.Repositories;
using Acc.ViewModels;
using DevExpress.DataProcessing.InMemoryDataProcessor;
using DevExpress.Web.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Acc.Persistence
{
    public class TransActionFileRepo : ITransActionFileRepo
    {
        private readonly ApplicationDbContext _context;

        public TransActionFileRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(TransActionFile ObjToSave)
        {
            _context.TransActionFiles.Add(ObjToSave);
        }

        public void Delete(int id)
        {
            try
            {
                var D = _context.TransActionFiles.FirstOrDefault(m => m.Id == id);
                if (D != null)
                {
                    _context.TransActionFiles.Remove(D);
                }
            }
            catch (Exception ex)
            {
                string er = ex.Message;
            }

        }

       
        public IEnumerable<TransActionFile> GetAllTransactionFile(int year , string VoucherNumber, string TransactionKindNo  ,string CompanyTransactionKindNo)
        {
            return _context.TransActionFiles.Where(m => m.Year == year && m.VoucherNumber == VoucherNumber && m.TransactionKindNo == TransactionKindNo
            && m.CompanyTransactionKindNo == CompanyTransactionKindNo).ToList();
        }

        public IEnumerable<TransActionFile> GetAllTransactionFileBank(int year, string VoucherNumber, string TransactionKindNo, string CompanyTransactionKindNo, int CompanyId)
        {
            return _context.TransActionFiles.Where(m => m.Year == year && m.VoucherNumber == VoucherNumber && m.TransactionKindNo == TransactionKindNo
            && m.CompanyTransactionKindNo == CompanyTransactionKindNo && m.CompanyId == CompanyId).ToList();
        }

        public int GetMaxSerial(int CompanyID)
        {
            var Obj = _context.TransActionFiles.FirstOrDefault(m => m.CompanyId == CompanyID);
            if (Obj == null)
            {
                return 1;
            }
            else
            {
                return _context.TransActionFiles.Where(m => m.CompanyId == CompanyID).Max(p => p.Id) + 1;
            }
        }

        public TransActionFile GetTransActionFilebyId(int id)
        {
            return _context.TransActionFiles.FirstOrDefault(m => m.Id == id);
        }

        public IEnumerable<TransActionFile> GetType(int CompanyID)
        {
              try
                {
                return _context.TransActionFiles.Where(m => m.CompanyId == CompanyID).ToList();
            }
            catch (Exception ex)
                {
                    return new List<TransActionFile>();
                }
            
        }

        public void Update(TransActionFile ObjToUpdate)
        {

             try
            {
                var D = _context.TransActionFiles.FirstOrDefault(m => m.Id == ObjToUpdate.Id && m.CompanyTransactionKindNo == ObjToUpdate.CompanyTransactionKindNo);
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