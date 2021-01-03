using Acc.Models;
using Acc.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Acc.Repositories
{
    public interface IUserRepo
    {
        ApplicationUser GetMyInfo(string UserID);
        void UpdateCompanyID(string UserID, int CompanyID);
        void UpdateUserPers(ApplicationUser model);
        Task<bool> UpdateUserPermission(ApplicationUser model);
        Task<bool> RemoveUserPermission(ApplicationUser model);

        ApplicationUser LoginMobile(string UserName, string Password);
        ApplicationUser GetUserByIDAndCo(int CoId, string UId);

        ApplicationUser GetUserByEmpIdAndCo(int CoId, string UId);


        ApplicationUser GetUserByID(string UId);

        ApplicationUser GetUserEmailID(string UId);

        ApplicationUser GetUserEmailIDForEmailValidation(string UId, string Email);

        IEnumerable<ApplicationUser> GetAllUsers(int CoId);
        void AddModify(ApplicationUser ObjToSave);
        void AddModifyFromCreateCompnay(ApplicationUser ObjToSave);
      
        void DeActivate(ApplicationUser ObjToSave);



        void Delete(ApplicationUser ObjToSave);

        ApplicationUser GetUserByEmailAndPassword(string Email, string Passord);
        void ChangePass(ApplicationUser ObjToSave);

        void ChangeCurrActiveYear(string UserName, int Year);

    }
}