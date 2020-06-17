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

            List<SystemRequestHeader> _SystemRequestHeader = _dbITC.SystemRequestHeader.Where(w => w.Status == false && w.CreateBy == emp_no).ToList();
            if (_SystemRequestHeader.Count() > 0)
            {
                ViewBag.BindSR = _SystemRequestHeader.FirstOrDefault().SystemRequest;
            }
            else
            {
                ViewBag.BindSR = "";
            }
            return View();
        }

        [HttpPost]
        public JsonResult CreateSystemRequest()
        {
            var sr = string.Empty;
            bool status = false;
            var msg = string.Empty;
            ClaimsPrincipal identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            string emp_no = identity.Claims.Where(c => c.Type == "employee_no").Select(c => c.Value).SingleOrDefault();

            ITCContext _db = new ITCContext();
            List<SystemRequestHeader> query = _db.SystemRequestHeader.OrderByDescending(o => o.SystemRequest).Take(1).ToList();
            List<SystemRequestHeader> _SystemRequestHeader = _db.SystemRequestHeader.Where(w => w.Status == false && w.CreateBy == emp_no).ToList();
            string year = DateTime.Now.ToString("yyyy") + "-";
            if (_SystemRequestHeader.Count() == 0)
            {
                status = true;
                msg = "Successful";
                if (query.Count() > 0)
                {
                    switch ((Convert.ToInt32(query[0].SystemRequest.Substring(7)) + 1).ToString().Length)
                    {
                        case 1:
                            sr = "SR" + year + "0000" + (Convert.ToInt32(query[0].SystemRequest.ToString().Substring(7)) + 1).ToString();
                            break;
                        case 2:
                            sr = "SR" + year + "000" + (Convert.ToInt32(query[0].SystemRequest.ToString().Substring(7)) + 1).ToString();
                            break;
                        case 3:
                            sr = "SR" + year + "00" + (Convert.ToInt32(query[0].SystemRequest.ToString().Substring(7)) + 1).ToString();
                            break;
                        case 4:
                            sr = "SR" + year + "0" + (Convert.ToInt32(query[0].SystemRequest.ToString().Substring(7)) + 1).ToString();
                            break;
                        case 5:
                            sr = "SR" + year + (Convert.ToInt32(query[0].SystemRequest.Substring(7)) + 1).ToString();
                            break;

                    }
                }
                else
                {
                    sr = "SR" + year + "00001";
                }

                _db.SystemRequestHeader.Add(new SystemRequestHeader
                {
                    SystemRequest = sr,
                    CreateDate = DateTime.Now,
                    CreateBy = emp_no,
                    Status = false
                });

                _db.SaveChanges();
            }
            else
            {
                status = false;
                msg = "Has been created";
            }

            return Json(new
            {
                success = status,
                message = msg,
                system_request = sr
            });
        }

        [HttpPost]
        public JsonResult GetDurationDate(ParameterDurationh cc)
        {
            string strFormat = getDuration(Convert.ToDateTime(cc.StartDate), Convert.ToDateTime(cc.FinishDate));
            return Json(new
            {
                duration = strFormat
            });
        }

        public string getDuration(DateTime StartDate, DateTime FinishDate)
        {
            string strFormat = "";
            double ApproxDaysPerMonth = 30.4375;
            double ApproxDaysPerYear = 365.25;

            int iDays = (FinishDate - StartDate).Days;

            int iYear = (int)(iDays / ApproxDaysPerYear);
            iDays -= (int)(iYear * ApproxDaysPerYear);

            int iMonth = (int)(iDays / ApproxDaysPerMonth);
            iDays -= (int)(iMonth * ApproxDaysPerMonth);

            if (iYear > 0)
            {
                strFormat += iYear + " Year ";
            }
            if (iMonth > 0)
            {
                strFormat += iMonth + " Month ";
            }
            if (iDays > 0)
            {
                strFormat += iDays + " Day";
            }

            return strFormat;
        }
    }
}