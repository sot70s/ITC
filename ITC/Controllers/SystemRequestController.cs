using ITC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace ITC.Controllers
{
    [Authorize]
    public class SystemRequestController : Controller
    {
        public ActionResult Index()
        {
            ClaimsPrincipal identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            string emp_no = identity.Claims.Where(c => c.Type == "employee_no").Select(c => c.Value).SingleOrDefault();
            ITCContext _dbITC = new ITCContext();
            ViewBag.BindEmployeeName = QueryPersonnel.ListEmployeeMeyer().FirstOrDefault(w => w.EMPLOYEE_NO == emp_no).EMPLOYEE_NAME;
            return View();
        }
    }
}