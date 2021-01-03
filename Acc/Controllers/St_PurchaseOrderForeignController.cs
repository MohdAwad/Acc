using Acc.Helpers;
using Acc.Models;
using Acc.Persistence;
using Acc.Repositories;
using Acc.ViewModels;
using DevExpress.DataAccess.Sql.DataApi;
using DevExpress.PivotGrid.Criteria;
using DevExpress.Web.Internal;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace Acc.Controllers
{
    [Authorize]
    public class St_PurchaseOrderForeignController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        public St_PurchaseOrderForeignController()
        {
            _unitOfWork = new UnitOfWork(new ApplicationDbContext());
        }
        public ActionResult Index()
        {
            return View();
        }
    }
}