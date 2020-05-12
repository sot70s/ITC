using ITC.Models;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace ITC.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return RedirectToAction("../Job/Index");
        }

        [HttpGet]
        public JsonResult GetFullName()
        {
            HRMContext _db = new HRMContext();
            ClaimsPrincipal identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            string emp_no = identity.Claims.Where(c => c.Type == "employee_no").Select(c => c.Value).SingleOrDefault();
            AccountJoinEmployee query = QueryAccount.ListAllRole().Where(w => w.EmployeeNo == emp_no).FirstOrDefault();

            return Json(new {
                Name = query.EMPLOYEE_NAME,
                Permission = query.Permission,
                PageStaff = query.PageStaff,
                PagePlanner = query.PagePlanner,
                PageUserManager = query.PageUserManager,
                PageMisManager = query.PageMisManager
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SignOut()
        {
            Request.GetOwinContext().Authentication.SignOut("Cookies");
            Request.GetOwinContext().Authentication.SignOut("oidc");
            return View();
        }
    }
}