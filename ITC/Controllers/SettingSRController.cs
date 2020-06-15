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
    public class SettingSRController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult SOPMng()
        {
            return PartialView();
        }

        public PartialViewResult SOPMaster()
        {
            ClaimsPrincipal identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            string emp_no = identity.Claims.Where(c => c.Type == "employee_no").Select(c => c.Value).SingleOrDefault();
            ITCContext _dbITC = new ITCContext();
            ViewBag.BindEmployeeName = QueryPersonnel.ListEmployeeMeyer().FirstOrDefault(w => w.EMPLOYEE_NO == emp_no).EMPLOYEE_NAME;

            return PartialView();
        }

        [HttpGet]
        public JsonResult GetSOP()
        {
            TableSOP data = new TableSOP()
            {
                data = SOPInfo.ListSOP().OrderByDescending(o => o.Id).ToList()
            };
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetDDLSOP()
        {
            ITCContext _dbITC = new ITCContext();
            List<SOPTemplate> _SOPTemplate = _dbITC.SOPTemplate.ToList();

            return Json(_SOPTemplate, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CreateSOP(ParameterSOP cc)
        {
            bool status = false;
            var msg = string.Empty;
            ClaimsPrincipal identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            string emp_no = identity.Claims.Where(c => c.Type == "employee_no").Select(c => c.Value).SingleOrDefault();
            ITCContext _dbITC = new ITCContext();

            if (cc.Procedure == null)
            {
                msg = "One or more fields have _blank";
            }
            else
            {
                try
                {
                    status = true;
                    msg = "Successful";
                    _dbITC.SOP.Add(new SOP
                    {
                        Procedure = cc.Procedure,
                        Type = cc.Type,
                        Description = cc.Description,
                        CreateDate = Convert.ToDateTime(cc.CreateDate),
                        CreateBy = emp_no
                    });
                    _dbITC.SaveChanges();
                }
                catch (Exception ex)
                {
                    status = false;
                    msg = ex.Message;
                }
            }

            return this.Json(new { success = status, message = msg, date = String.Format("{0:dd-MMM-yyyy hh:mm tt}", DateTime.Now) }, JsonRequestBehavior.AllowGet);
        }

        [HttpDelete]
        public JsonResult DeleteProcedure(int id)
        {
            ITCContext _dbITC = new ITCContext();
            SOP _SOP = _dbITC.SOP.FirstOrDefault(s => s.Id == id);
            if (_SOP == null)
            {
                return Json(new { success = false, message = "Error while Deleting" });
            }
            _dbITC.SOP.Remove(_SOP);

            List<SOPAsset> _SOPAsset = _dbITC.SOPAsset.Where(w => w.SOP_Id == id).ToList();
            for (int i = 0; i < _SOPAsset.Count(); i++)
            {
                _dbITC.SOPAsset.Remove(_SOPAsset[i]);
            }

            _dbITC.SaveChanges();
            return Json(new { success = true, message = "Delete successful", date = String.Format("{0:dd-MMM-yyyy hh:mm tt}", DateTime.Now) });
        }

        public PartialViewResult SOPAsset()
        {
            ClaimsPrincipal identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            string emp_no = identity.Claims.Where(c => c.Type == "employee_no").Select(c => c.Value).SingleOrDefault();
            ITCContext _dbITC = new ITCContext();
            ViewBag.BindEmployeeName = QueryPersonnel.ListEmployeeMeyer().FirstOrDefault(w => w.EMPLOYEE_NO == emp_no).EMPLOYEE_NAME;
            ViewBag.BindSOP = _dbITC.SOPTemplate.ToList();
            return PartialView();
        }

        [HttpGet]
        public JsonResult GetSOPTemplate()
        {
            TableSOPTemplate data = new TableSOPTemplate()
            {
                data = SOPTemplateInfo.ListSOPTemplate().OrderByDescending(o => o.Id).ToList()
            };
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CreateSOPTemplate(ParameterSOPTemplate cc)
        {
            bool status = false;
            var msg = string.Empty;
            ClaimsPrincipal identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            string emp_no = identity.Claims.Where(c => c.Type == "employee_no").Select(c => c.Value).SingleOrDefault();

            ITCContext _dbITC = new ITCContext();

            if (cc.SOP == null)
            {
                msg = "One or more fields have _blank";
            }
            else
            {
                try
                {
                    status = true;
                    msg = "Successful";
                    _dbITC.SOPTemplate.Add(new SOPTemplate
                    {
                        SOP = cc.SOP,
                        Type = cc.Type,
                        CreateDate = Convert.ToDateTime(cc.CreateDate),
                        CreateBy = emp_no
                    });
                    _dbITC.SaveChanges();
                }
                catch (Exception ex)
                {
                    status = false;
                    msg = ex.Message;
                }
            }

            return this.Json(new { success = status, message = msg, date = String.Format("{0:dd-MMM-yyyy hh:mm tt}", DateTime.Now) }, JsonRequestBehavior.AllowGet);
        }

        [HttpDelete]
        public JsonResult DeleteSOP(int id)
        {
            ITCContext _dbITC = new ITCContext();
            SOPTemplate _SOPTemplate = _dbITC.SOPTemplate.FirstOrDefault(s => s.Id == id);
            if (_SOPTemplate == null)
            {
                return Json(new { success = false, message = "Error while Deleting" });
            }
            _dbITC.SOPTemplate.Remove(_SOPTemplate);

            List<SOPAsset> _SOPAsset = _dbITC.SOPAsset.Where(w => w.SOPTemplate_Id == id).ToList();
            for (int i = 0; i < _SOPAsset.Count(); i++)
            {
                _dbITC.SOPAsset.Remove(_SOPAsset[i]);
            }

            _dbITC.SaveChanges();
            return Json(new { success = true, message = "Delete successful", date = String.Format("{0:dd-MMM-yyyy hh:mm tt}", DateTime.Now) });
        }

        [HttpGet]
        public JsonResult GetSOPAsset(int id)
        {
            List<SOPAssets> _SOPAssetInfo = new List<SOPAssets>();
            TableSOPAssetInfo data = new TableSOPAssetInfo();
            if (id == 0)
            {
                data = new TableSOPAssetInfo()
                {
                    data = new List<SOPAssets>()
                };
            }
            else
            {
                data = new TableSOPAssetInfo()
                {
                    data = SOPAssetInfo.ListSOPAsset(id).ToList()
                };
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SetSOPAsset(int id, ParameterSOPAsset cc)
        {
            bool status = false;
            var msg = string.Empty;
            ClaimsPrincipal identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            string emp_no = identity.Claims.Where(c => c.Type == "employee_no").Select(c => c.Value).SingleOrDefault();
            ITCContext _dbITC = new ITCContext();
            try
            {
                status = true;
                msg = "Successful";
                if (cc.Checkbox == true)
                {
                    _dbITC.SOPAsset.Add(new SOPAsset
                    {
                        SOP_Id = id,
                        SOPTemplate_Id = cc.SOPTemplate_Id,
                        CreateDate = DateTime.Now,
                        CreateBy = emp_no
                    });
                    _dbITC.SaveChanges();
                }
                else
                {
                    SOPAsset query = _dbITC.SOPAsset.FirstOrDefault(s => s.SOP_Id == id && s.SOPTemplate_Id == cc.SOPTemplate_Id);
                    if (query == null)
                    {
                        return Json(new { success = false, message = "Error while Deleting" });
                    }
                    _dbITC.SOPAsset.Remove(query);
                    _dbITC.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                status = false;
                msg = ex.Message;
            }

            return Json(new { success = status, message = msg });
        }

        public PartialViewResult PermissionMng()
        {
            return PartialView();
        }

        public PartialViewResult Role()
        {
            ViewBag.BindEmployee = QueryAccount.ListAccountMeyer().Where(w => w.EMPLOYEE_STATUS == "A" && (w.SECTION_CODE == "G4MS" || w.SECTION_CODE == "G1EO")).OrderBy(o => o.EmployeeNo).ToList();
            return PartialView();
        }


        [HttpGet]
        public JsonResult GetRole()
        {
            TableRoleSR data = new TableRoleSR()
            {
                data = RoleSRInfo.ListRoleSR().OrderByDescending(o => o.Id).ToList()
            };
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CreateRole(ParameterRoleSR cc)
        {
            bool status = false;
            var msg = string.Empty;
            ClaimsPrincipal identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            string emp_no = identity.Claims.Where(c => c.Type == "employee_no").Select(c => c.Value).SingleOrDefault();
            ITCContext _dbITC = new ITCContext();

            if (cc.EmployeeNo == "0")
            {
                msg = "One or more fields have _blank";
            }
            else
            {
                try
                {
                    status = true;
                    msg = "Successful";
                    _dbITC.RoleSR.Add(new RoleSR
                    {
                        EmployeeNo = cc.EmployeeNo,
                        SectionType = cc.SectionType,
                        JobType = cc.JobType,
                        CreateDate = DateTime.Now,
                        CreateBy = emp_no
                    });
                    _dbITC.SaveChanges();
                }
                catch (Exception ex)
                {
                    status = false;
                    msg = ex.Message;
                }
            }

            return this.Json(new { success = status, message = msg }, JsonRequestBehavior.AllowGet);
        }

        [HttpDelete]
        public JsonResult DeleteRole(int id)
        {
            ITCContext _dbITC = new ITCContext();
            RoleSR _RoleSR = _dbITC.RoleSR.FirstOrDefault(s => s.Id == id);
            if (_RoleSR == null)
            {
                return Json(new { success = false, message = "Error while Deleting" });
            }
            _dbITC.RoleSR.Remove(_RoleSR);
            _dbITC.SaveChanges();

            return Json(new { success = true, message = "Delete successful" });
        }
    }
}