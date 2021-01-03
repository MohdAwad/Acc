using Acc.Models;
using Acc.Repositories;
using Acc.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Acc.Persistence
{
    public class AssetTransActionRepo : IAssetTransActionRepo
    {
        private readonly ApplicationDbContext _context;

        public AssetTransActionRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(AssetTransAction ObjToSave)
        {
            _context.AssetTransActions.Add(ObjToSave);
        }

        public void ExportAssetTransByVouchrNo(int CompanyID, int VouchrNo)
        {

            _context.Database.ExecuteSqlCommand(
                "  Update   AssetTransActions " +
                " set Exported=1   " +

                " where CompanyID=@CompanyID and VouchrNo=@VouchrNo   "


                , new SqlParameter("@CompanyID", CompanyID)
                , new SqlParameter("@VouchrNo", VouchrNo)



                );
        }

        public IEnumerable<AssetTransToPostVM> GetAssetCreditToPost(int CompanyID, int HeaderID)
        {
            try

            {
                return _context.Database.SqlQuery<AssetTransToPostVM>(
                 "   select D.*,A.ArabicName  as AccountName from ChartOfAccounts A, " +
                 "( " +
                 " select CompanyID,CreditAccountNo as AccountNo ,CreditCostNo as CostCenterNo, sum(ValueofConsumption) as Value from " +
                 " ( " +
                "  Select AST.ValueofConsumption, " +
                "  A.DebitAccountNo, A.CreditAccountNo, A.DebitCostNo, A.CreditCostNo,A.CompanyID " +
                "  From AssetTransActions AST ,Assets A " +
                "  Where A.CompanyID=AST.CompanyID and A.AssetID=AST.AssetID and AST.Exported=0  " +
                "  and AST.CompanyID=@CompanyID and AST.VouchrNo=@HeaderID  " +
                " ) S " +
                "  group by  CompanyID,CreditAccountNo,CreditCostNo   " +
                " ) D " +
                "  where  D.AccountNo=A.AccountNumber and A.CompanyID=D.CompanyID "
                , new SqlParameter("@CompanyID", CompanyID)
                , new SqlParameter("@HeaderID", HeaderID)

            ).ToList();
            }
            catch(Exception ex)
            {
                return new List<AssetTransToPostVM>();
            }
        }
        public IEnumerable<AssetTransToPostVM> GetAssetDebitToPost(int CompanyID, int HeaderID)
        {
            try

            {
                return _context.Database.SqlQuery<AssetTransToPostVM>(
                 "   select D.*,A.ArabicName as AccountName from ChartOfAccounts A, " +
                 "( " +
                 " select CompanyID,DebitAccountNo as AccountNo ,DebitCostNo as CostCenterNo, sum(ValueofConsumption) as Value from " +
                 " ( " +
                "  Select AST.ValueofConsumption, " +
                "  A.DebitAccountNo, A.CreditAccountNo, A.DebitCostNo, A.CreditCostNo,A.CompanyID " +
                "  From AssetTransActions AST ,Assets A " +
                "  Where A.CompanyID=AST.CompanyID and A.AssetID=AST.AssetID and AST.Exported=0  " +
                "  and AST.CompanyID=@CompanyID and AST.VouchrNo=@HeaderID  " +
                " ) S " +
                "  group by  CompanyID,DebitAccountNo,DebitCostNo   " +
                " ) D " +
                "  where  D.AccountNo=A.AccountNumber and A.CompanyID=D.CompanyID "
                , new SqlParameter("@CompanyID", CompanyID)
                , new SqlParameter("@HeaderID", HeaderID)

            ).ToList();
            }
            catch (Exception ex)
            {
                return new List<AssetTransToPostVM>();
            }
        }
    }
}