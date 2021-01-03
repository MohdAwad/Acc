using Acc.Models;
using Acc.Persistence;
using Acc.Repositories;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Acc.Controllers
{
    public class AccountTypeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public AccountTypeController()
        {
            _unitOfWork = new UnitOfWork(new ApplicationDbContext());
        }
        [HttpGet]
        public JsonResult GetAllAccountType()
        {
            try
            {


                var userId = User.Identity.GetUserId();

                
               
                   // return Json(new List<AccountType>(), JsonRequestBehavior.AllowGet);
                



                var R = _unitOfWork.AccountType.GetAllAccountType( );
                return Json(R, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return Json(new List<AccountType>(), JsonRequestBehavior.AllowGet);
            }

        }
        // GET: AccountType
        public ActionResult Index()
        {
            return View();
        }
    }
}