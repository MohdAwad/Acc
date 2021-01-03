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
    public class TransactionKindController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public TransactionKindController()
        {
            _unitOfWork = new UnitOfWork(new ApplicationDbContext());
        }
        [HttpGet]
        public JsonResult GetAllTransactionKind()
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var R = _unitOfWork.TransactionKind.GetAllTransactionKind();
                return Json(R, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                return Json(new List<TransactionKind>(), JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult Index()
        {
            return View();
        }
    }
}