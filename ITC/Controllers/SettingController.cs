using ITC.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace ITC.Controllers
{
    [Authorize]
    public class SettingController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult UserMng()
        {
            return PartialView();
        }

        public PartialViewResult UserDataTable()
        {
            return PartialView();
        }

        [HttpGet]
        public JsonResult GetUserDataTable()
        {
            TableAccountsJoinEmployee data = new TableAccountsJoinEmployee()
            {
                data = QueryAccount.ListAccountMeyer().OrderByDescending(o => o.CreatedDate).ToList()
            };
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpDelete]
        public JsonResult DeleteUser(string id)
        {
            MILAuthContext _db = new MILAuthContext();
            Accounts query = _db.Accounts.FirstOrDefault(s => s.Id == id);
            if (query == null)
            {
                return Json(new { success = false, message = "Error while Deleting" });
            }
            _db.Accounts.Remove(query);
            _db.SaveChanges();
            return Json(new { success = true, message = "Delete successful" });
        }

        [HttpGet]
        public JsonResult GetRole(string id)
        {
            List<AccountJoinEmployee> query = QueryAccount.ListRole().Where(w => w.Id == id).ToList();

            return Json(query, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ResetPassword(string id)
        {
            bool status = false;
            var msg = string.Empty;
            sentEmail sm = new sentEmail();
            try
            {
                status = true;
                msg = "Successful";
                AccountJoinEmployee query = QueryAccount.ListAccountMeyer().Where(w => w.Id == id).FirstOrDefault();

                string from = "swadmin@meyer-mil.com";
                string titleEmail = "RESET PASSWORD LOGIN CENTER";
                string subject = "RESET PASSWORD LOGIN CENTER";
                string contents = "<p>Dear Khun. " + query.EMPLOYEE_NAME + "(" + query.EmployeeNo + ")" + "</p>" + "<br/>" +
                                  "<p>Please set your password according to MILAuth policy.</p>" +
                                  "<p>Click on the link below : " + "http://milws/ITC/Account/ChangePassword?id=" + query.Id + "</p>";
                string email_to = query.Email;
                string email_cc = "";
                sm.SendEMailTo(from, titleEmail, email_to, email_cc, "", subject, contents, true, "");
            }
            catch (Exception ex)
            {
                status = false;
                msg = ex.Message;
            }

            return Json(new { success = status, message = msg });
        }

        [HttpPost]
        public JsonResult SetRole(ParameterRole cc)
        {
            bool status = false;
            var msg = string.Empty;
            ITCContext _dbITC = new ITCContext();
            try
            {
                status = true;
                msg = "Successful";
                List<RolePage> queryRolePage = _dbITC.RolePage.Where(w => w.MILAuth_Id == cc.Id).ToList();
                List<RoleUser> queryRoleUser = _dbITC.RoleUser.Where(w => w.MILAuth_Id == cc.Id).ToList();
                List<RoleCoordinator> queryRoleCoordinator = _dbITC.RoleCoordinator.Where(w => w.MILAuth_Id == cc.Id).ToList();
                if (queryRolePage.Count() > 0)
                {
                    queryRolePage[0].PageStaff = cc.PageStaff;
                    queryRolePage[0].PagePlanner = cc.PagePlanner;
                    queryRolePage[0].PageUserManager = cc.PageUserManager;
                    queryRolePage[0].PageMisManager = cc.PageMisManager;
                }
                else
                {
                    _dbITC.RolePage.Add(new RolePage
                    {
                        PageStaff = cc.PageStaff,
                        PagePlanner = cc.PagePlanner,
                        PageUserManager = cc.PageUserManager,
                        PageMisManager = cc.PageMisManager,
                        MILAuth_Id = cc.Id
                    });
                }

                switch (cc.Permission)
                {
                    case 1:
                        if (queryRoleUser.Count() > 0 && queryRoleCoordinator.Count() > 0)
                        {
                            queryRoleUser[0].Permission = false;
                            queryRoleCoordinator[0].Coordinator = false;
                        }
                        else
                        {
                            _dbITC.RoleUser.Add(new RoleUser
                            {
                                Permission = false,
                                MILAuth_Id = cc.Id
                            });
                            _dbITC.RoleCoordinator.Add(new RoleCoordinator
                            {
                                Coordinator = false,
                                MILAuth_Id = cc.Id
                            });
                        }
                        break;
                    case 2:
                        if (queryRoleUser.Count() > 0 && queryRoleCoordinator.Count() > 0)
                        {
                            queryRoleUser[0].Permission = false;
                            queryRoleCoordinator[0].Coordinator = true;
                        }
                        else
                        {
                            _dbITC.RoleUser.Add(new RoleUser
                            {
                                Permission = false,
                                MILAuth_Id = cc.Id
                            });
                            _dbITC.RoleCoordinator.Add(new RoleCoordinator
                            {
                                Coordinator = true,
                                MILAuth_Id = cc.Id
                            });
                        }
                        break;
                    case 3:
                        if (queryRoleUser.Count() > 0 && queryRoleCoordinator.Count() > 0)
                        {
                            queryRoleUser[0].Permission = true;
                            queryRoleCoordinator[0].Coordinator = true;
                        }
                        else
                        {
                            _dbITC.RoleUser.Add(new RoleUser
                            {
                                Permission = true,
                                MILAuth_Id = cc.Id
                            });
                            _dbITC.RoleCoordinator.Add(new RoleCoordinator
                            {
                                Coordinator = true,
                                MILAuth_Id = cc.Id
                            });
                        }
                        break;
                }
                _dbITC.SaveChanges();
            }
            catch (Exception ex)
            {
                status = false;
                msg = ex.Message;
            }

            return Json(new { success = status, message = msg });
        }

        public PartialViewResult CreateUser()
        {
            return PartialView();
        }

        [HttpPost]
        public JsonResult SetUser(ParameterAccount cc)
        {
            bool status = false;
            var msg = string.Empty;

            if (cc.Password != cc.ConfirmPassword)
            {
                status = false;
                msg = "Please check your password";
                if (cc.Email == null || cc.EmployeeNo == null || cc.FullName == null || cc.Username == null || cc.Password == null || cc.ConfirmPassword == null)
                {
                    msg = "One or more fields have _blank";
                }
            }
            else
            {
                var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
                var token = identity.Claims.Where(c => c.Type == "id_token").Select(c => c.Value).SingleOrDefault();
                HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create("https://secure.meyer-mil.com/api/user");
                req.ContentType = "application/json";
                req.Method = "POST";
                req.Headers.Add("Authorization", "Bearer " + token);
                string json = "{\"Email\":\"" + cc.Email + "\"," +
                              "\"Username\":\"" + cc.Username + "\"," +
                              "\"Password\":\"" + cc.Password + "\"," +
                              "\"AccountType\":\"1\"," +
                              "\"EmployeeNo\":\"" + cc.EmployeeNo + "\"," +
                              "\"FullName\":\"" + cc.FullName + "\"," +
                              "\"ProfileImageUri\":\"\"}";
                byte[] postBytes = Encoding.UTF8.GetBytes(json);
                req.ContentLength = postBytes.Length;
                Stream requestStream = req.GetRequestStream();
                requestStream.Write(postBytes, 0, postBytes.Length);
                requestStream.Close();

                try
                {
                    HttpWebResponse res = (HttpWebResponse)req.GetResponse();
                    StreamReader streamReader = new StreamReader(res.GetResponseStream());
                    var result = streamReader.ReadToEnd();
                    if (result != "")
                    {
                        status = false;
                        msg = "Status Code 401";
                    }
                    else
                    {
                        status = true;
                        msg = "Successful";
                    }
                }
                catch (Exception ex)
                {
                    status = false;
                    msg = ex.Message;
                }
            }

            return Json(new { success = status, message = msg });
        }

        public PartialViewResult EquipmentMng()
        {
            return PartialView();
        }

        public PartialViewResult SoftwareAsset()
        {
            return PartialView();
        }

        public PartialViewResult CreateSoftwareAsset()
        {
            return PartialView();
        }

        [HttpGet]
        public JsonResult GetSoftwareAsset()
        {
            TableSoftware data = new TableSoftware()
            {
                data = QuerySoftware.ListSoftware()
            };
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SetSoftwareAsset(ParameterSoftware cc)
        {
            bool status = false;
            var msg = string.Empty;
            var equipment = string.Empty;
            ITCContext _dbITC = new ITCContext();
            if (cc.Company == string.Empty || cc.SoftwareType == string.Empty || cc.Responsible == null || cc.LicenseType == string.Empty || cc.StartDate == null)
            {
                status = false;
                msg = "One or more fields have _blank";
            }
            else
            {
                try
                {
                    status = true;
                    msg = "Successful";
                    List<Software> query = _dbITC.Software.OrderByDescending(o => o.Id).Where(w => w.Company == cc.Company & w.SoftwareType == cc.SoftwareType).Take(1).ToList();
                    if (query.Count() > 0)
                    {
                        switch ((Convert.ToInt32(query[0].Equipment.Substring(6)) + 1).ToString().Length)
                        {
                            case 1:
                                equipment = cc.SoftwareType + "-" + cc.Company + "0000" + (Convert.ToInt32(query[0].Equipment.Substring(6)) + 1).ToString();
                                break;
                            case 2:
                                equipment = cc.SoftwareType + "-" + cc.Company + "000" + (Convert.ToInt32(query[0].Equipment.Substring(6)) + 1).ToString();
                                break;
                            case 3:
                                equipment = cc.SoftwareType + "-" + cc.Company + "00" + (Convert.ToInt32(query[0].Equipment.Substring(6)) + 1).ToString();
                                break;
                            case 4:
                                equipment = cc.SoftwareType + "-" + cc.Company + "0" + (Convert.ToInt32(query[0].Equipment.Substring(6)) + 1).ToString();
                                break;
                            case 5:
                                equipment = cc.SoftwareType + "-" + cc.Company + (Convert.ToInt32(query[0].Equipment.Substring(6)) + 1).ToString();
                                break;
                        }
                    }
                    else
                    {
                        equipment = cc.SoftwareType + "-" + cc.Company + "00001";
                    }

                    _dbITC.Software.Add(new Software
                    {
                        Equipment = equipment,
                        SoftwareType = cc.SoftwareType,
                        SoftwareName = cc.SoftwareName,
                        Responsible = cc.Responsible,
                        Company = cc.Company,
                        CoreFunction = cc.CoreFunction,
                        Description = cc.Description,
                        Remark = cc.Remark,
                        Location = cc.Location,
                        Cost = cc.Cost,
                        LicenseType = cc.LicenseType,
                        LicenseQty = cc.LicenseQty,
                        StartDate = (cc.StartDate == null) ? DateTime.Now : Convert.ToDateTime(cc.StartDate),
                        ObsolateDate = (cc.ObsolateDate == null) ? DateTime.Now.AddYears(10) : Convert.ToDateTime(cc.ObsolateDate),
                        CreateDate = DateTime.Now,
                        UserAccountNeed = cc.UserAccountNeed,
                        TrainingNeed = cc.TrainingNeed,
                        Status = cc.Status
                    });
                    _dbITC.SaveChanges();
                }
                catch (Exception ex)
                {
                    status = false;
                    msg = ex.Message;
                }
            }

            return Json(new { success = status, message = msg, val = equipment });
        }

        [HttpPost]
        public JsonResult SetUploadFilePDF(string equipment)
        {
            bool status = false;
            var msg = string.Empty;
            var dt = DateTime.Now;
            ITCContext _dbITC = new ITCContext();
            try
            {
                status = true;
                msg = "Successful";
                HttpPostedFileBase file = Request.Files[0];
                string _type = Path.GetExtension(file.FileName).ToLower();
                if (_type == ".pdf")
                {
                    var querySF = _dbITC.SoftwareFile.Where(s => s.Equip_Id == equipment).ToList();
                    string path = Server.MapPath("\\ITC\\Download\\pdf\\SW" + dt.Year.ToString() + dt.Month.ToString() + dt.Day.ToString() + dt.Hour.ToString() + dt.Minute.ToString() + dt.Second.ToString() + ".pdf");
                    if (querySF.Count() > 0)
                    {
                        FileInfo dfile = new FileInfo(querySF[0].SoftwareFilePath);
                        dfile.Delete();
                        _dbITC.SoftwareFile.Remove(querySF[0]);

                        switch (_type)
                        {
                            case ".pdf":
                                file.SaveAs(path);
                                break;
                        }

                        _dbITC.SoftwareFile.Add(new SoftwareFile
                        {
                            SoftwareFilePath = path,
                            Equip_Id = equipment
                        });
                        _dbITC.SaveChanges();
                    }
                    else
                    {
                        switch (_type)
                        {
                            case ".pdf":
                                file.SaveAs(path);
                                break;
                        }

                        _dbITC.SoftwareFile.Add(new SoftwareFile
                        {
                            SoftwareFilePath = path,
                            Equip_Id = equipment
                        });
                        _dbITC.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                status = false;
                msg = ex.Message;
            }

            return this.Json(new { success = status, message = msg }, JsonRequestBehavior.AllowGet);
        }

        [HttpDelete]
        public JsonResult DeleteSoftware(string equipment)
        {
            ITCContext _db = new ITCContext();
            Software querySW = _db.Software.FirstOrDefault(s => s.Equipment == equipment);
            List<SoftwareFile> querySF = _db.SoftwareFile.Where(s => s.Equip_Id == equipment).ToList();
            if (querySW == null)
            {
                return Json(new { success = false, message = "Error while Deleting" });
            }
            if (querySF.Count > 0)
            {
                FileInfo file = new FileInfo(querySF[0].SoftwareFilePath);
                file.Delete();
                _db.SoftwareFile.Remove(querySF[0]);
            }
            _db.Software.Remove(querySW);
            _db.SaveChanges();
            return Json(new { success = true, message = "Delete successful" });
        }

        public ActionResult DownloadSoftwareFile(string equipment)
        {
            ITCContext _dbITC = new ITCContext();
            SoftwareFile querySW = _dbITC.SoftwareFile.FirstOrDefault(s => s.Equip_Id == equipment);
            return File(querySW.SoftwareFilePath, "application/pdf");
        }

        [HttpGet]
        public JsonResult ModifySoftware(string equipment)
        {
            List<SoftwarareStore> query = QuerySoftware.ListSoftware().Where(w => w.Equipment == equipment).ToList();
            return Json(query, JsonRequestBehavior.AllowGet);
        }

        [HttpPut]
        public JsonResult UpdateSoftwareAsset(ParameterSoftware cc)
        {
            bool status = false;
            var msg = string.Empty;
            ITCContext _dbITC = new ITCContext();

            try
            {
                status = true;
                msg = "Successful";
                Software query = _dbITC.Software.Where(w => w.Id == cc.id).FirstOrDefault();
                query.SoftwareName = cc.SoftwareName;
                query.Responsible = cc.Responsible;
                query.Company = cc.Company;
                query.CoreFunction = cc.CoreFunction;
                query.Description = cc.Description;
                query.Remark = cc.Remark;
                query.Location = cc.Location;
                query.Cost = cc.Cost;
                query.LicenseType = cc.LicenseType;
                query.LicenseQty = cc.LicenseQty;
                query.StartDate = Convert.ToDateTime(cc.StartDate);
                query.ObsolateDate = Convert.ToDateTime(cc.ObsolateDate);
                query.UserAccountNeed = cc.UserAccountNeed;
                query.TrainingNeed = cc.TrainingNeed;
                query.Status = cc.Status;
                _dbITC.SaveChanges();
            }
            catch (Exception ex)
            {
                status = false;
                msg = ex.Message;
            }
            return Json(new { success = status, message = msg });
        }

        public PartialViewResult SoftwareHolder()
        {
            ITCContext _dbITC = new ITCContext();
            ViewBag.BindPosition = QueryPersonnel.ListEmployeeMeyer().Where(w => w.EMPLOYEE_STATUS == "A").GroupBy(g => g.POSITION_DESCRIPTION).OrderBy(o => o.Key).Select(s => s.Key).ToList();
            ViewBag.BindEmployeeMeyer = QueryPersonnel.ListEmployeeMeyer().Where(w => w.EMPLOYEE_STATUS == "A").OrderBy(o => o.EMPLOYEE_NO).ToList();
            return PartialView();
        }

        [HttpGet]
        public JsonResult GetSoftwareHolder(string emp_no)
        {
            TableSoftwareHolder data = new TableSoftwareHolder();
            if (emp_no == "0" || emp_no == "undefined")
            {
                data = new TableSoftwareHolder()
                {
                    data = new List<EmployeeJoinSoftware>()
                };
            }
            else
            {
                data = new TableSoftwareHolder()
                {
                    data = QuerySoftware.ListSoftwareHolder(emp_no).OrderByDescending(o => o.Equip_Id).ThenBy(t => t.Equipment).ToList()
                };
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetEmployee(ParameterSoftwareHolder cc)
        {
            EmployeeStore query = QueryPersonnel.ListEmployeeMeyer().FirstOrDefault(w => w.EMPLOYEE_NO == cc.EmployeeNo);
            var position = query.POSITION_DESCRIPTION;
            var section = query.SECTION_DESCRIPTION;
            return Json(new {
                position = position,
                section = section
            });
        }


        [HttpGet]
        public JsonResult GetSoftwareTemplate(ParameterSoftware cc)
        {
            TableSoftwareHolder data = new TableSoftwareHolder();
            if (cc.Position == "0" || cc.Position == "undefined")
            {
                data = new TableSoftwareHolder()
                {
                    data = new List<EmployeeJoinSoftware>()
                };
            }
            else
            {
                data = new TableSoftwareHolder()
                {
                    data = QuerySoftware.ListSoftwareTemplate(cc.Position).OrderByDescending(o => o.Equip_Id).ThenBy(t => t.Equipment).ToList()
                };
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SetSoftwareHolder(string equipment, ParameterSoftwareHolder cc)
        {
            bool status = false;
            var msg = string.Empty;
            ITCContext _dbITC = new ITCContext();
            try
            {
                status = true;
                msg = "Successful";
                if (cc.Checkbox == true)
                {
                    _dbITC.SoftwareHolder.Add(new SoftwareHolder
                    {
                        Equip_Id = equipment,
                        EmployeeNo = cc.EmployeeNo
                    });
                    _dbITC.SaveChanges();
                }
                else
                {
                    SoftwareHolder query = _dbITC.SoftwareHolder.FirstOrDefault(s => s.Equip_Id == equipment && s.EmployeeNo == cc.EmployeeNo);
                    if (query == null)
                    {
                        return Json(new { success = false, message = "Error while Deleting" });
                    }
                    _dbITC.SoftwareHolder.Remove(query);
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

        [HttpPost]
        public JsonResult SetSoftwareTemplate(string equipment, ParameterSoftwareTemplate cc)
        {
            bool status = false;
            var msg = string.Empty;
            ITCContext _dbITC = new ITCContext();
            try
            {
                status = true;
                msg = "Successful";
                if (cc.Checkbox == true)
                {
                    _dbITC.SoftwareTemplate.Add(new SoftwareTemplate
                    {
                        Equip_Id = equipment,
                        Position = cc.Position
                    });
                    _dbITC.SaveChanges();
                }
                else
                {
                    SoftwareTemplate query = _dbITC.SoftwareTemplate.FirstOrDefault(s => s.Equip_Id == equipment && s.Position == cc.Position);
                    if (query == null)
                    {
                        return Json(new { success = false, message = "Error while Deleting" });
                    }
                    _dbITC.SoftwareTemplate.Remove(query);
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

        [HttpPost]
        public JsonResult UpdateTemplate(ParameterSoftwareHolder cc)
        {
            bool status = false;
            var msg = string.Empty;
            ITCContext _dbITC = new ITCContext();
            HRMContext _dbHRM = new HRMContext();
            try
            {
                status = true;
                msg = "Successful";
                EmployeeStore queryEmp = QueryPersonnel.ListEmployeeMeyer().FirstOrDefault(s => s.EMPLOYEE_NO == cc.EmployeeNo);
                var ListSoftwareTemplate = _dbITC.SoftwareTemplate.Where(w => w.Position == queryEmp.POSITION_DESCRIPTION).ToList();
                for (int i = 0; i < ListSoftwareTemplate.Count(); i++)
                {
                    var item = ListSoftwareTemplate[i];
                    SoftwareHolder querySH = _dbITC.SoftwareHolder.FirstOrDefault(s => s.Equip_Id == item.Equip_Id && s.EmployeeNo == cc.EmployeeNo);
                    if (querySH == null)
                    {
                        _dbITC.SoftwareHolder.Add(new SoftwareHolder
                        {
                            Equip_Id = item.Equip_Id,
                            EmployeeNo = cc.EmployeeNo
                        });
                        _dbITC.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                status = false;
                msg = ex.Message;
            }

            return Json(new { success = status, message = msg });
        }

        public PartialViewResult TeamFoundationMng()
        {
            return PartialView();
        }

        public PartialViewResult Approver()
        {
            return PartialView();
        }

        [HttpGet]
        public JsonResult GetApprover()
        {
            TableApproverJoinAccount data = new TableApproverJoinAccount()
            {
                data = QueryApprover.ListApprover()
            };
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpDelete]
        public JsonResult DeleteApprover(int id)
        {
            ITCContext _dbITC = new ITCContext();
            Approver query = _dbITC.Approver.FirstOrDefault(s => s.Id == id);
            if (query == null)
            {
                return Json(new { success = false, message = "Error while Deleting" });
            }
            _dbITC.Approver.Remove(query);
            _dbITC.SaveChanges();

            return Json(new { success = true, message = "Delete successful" });
        }

        public PartialViewResult CreateApprover()
        {
            return PartialView();
        }

        [HttpPost]
        public JsonResult SetApprover(ParameterApprover cc)
        {
            bool status = false;
            var msg = string.Empty;
            ITCContext _dbITC = new ITCContext();
            if (cc.EmployeeNo == null || cc.SectionCode == null)
            {
                status = false;
                msg = "One or more fields have _blank";
            }
            else
            {
                try
                {
                    status = true;
                    msg = "Successful";
                    _dbITC.Approver.Add(new Approver
                    {
                        EmployeeNo = cc.EmployeeNo,
                        SectionCode = cc.SectionCode
                    });
                    _dbITC.SaveChanges();
                }
                catch (Exception ex)
                {
                    status = false;
                    msg = ex.Message;
                }
            }
            return Json(new { success = status, message = msg });
        }

        public PartialViewResult Coordinator()
        {
            ViewBag.BindEmployee = QueryAccount.ListRoleCoordinatorOnly().OrderBy(o => o.DEPARTMENT_CODE).ThenBy(t => t.EmployeeNo);
            return PartialView();
        }

        [HttpGet]
        public JsonResult GetCoordinator(string employee_no)
        {
            HRMContext _dbHRM = new HRMContext();
            TableSectionJoinGroupCoordinator data = new TableSectionJoinGroupCoordinator();
            if (employee_no == "0" || employee_no == "undefined")
            {
                data = new TableSectionJoinGroupCoordinator()
                {
                    data = new List<SectionJoinGroupCoordinator>()
                };
            }
            else
            {
                data = new TableSectionJoinGroupCoordinator()
                {
                    data = QueryGroupCoordinator.ListSection(employee_no).OrderByDescending(o => o.EmployeeNo).ToList()
                };
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SetCoordinator(ParameterGroupCoordinator cc)
        {
            bool status = false;
            var msg = string.Empty;
            ITCContext _dbITC = new ITCContext();
            try
            {
                status = true;
                msg = "Successful";
                if (cc.Checkbox == true)
                {
                    _dbITC.GroupCoordinator.Add(new GroupCoordinator
                    {
                        EmployeeNo = cc.EmployeeNo,
                        SectionCode = cc.SectionCode
                    });
                    _dbITC.SaveChanges();
                }
                else
                {
                    GroupCoordinator query = _dbITC.GroupCoordinator.FirstOrDefault(s => s.EmployeeNo == cc.EmployeeNo && s.SectionCode == cc.SectionCode);
                    if (query == null)
                    {
                        return Json(new { success = false, message = "Error while Deleting" });
                    }
                    _dbITC.GroupCoordinator.Remove(query);
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

        public PartialViewResult Mis()
        {
            ITCContext _dbITC = new ITCContext();
            List<AccountJoinEmployee> queryAccountJoinEmployee = QueryAccount.ListEmployee().Union(QueryAccount.ListManager()).Where(w => w.EMPLOYEE_STATUS == "A" && w.SECTION_CODE == "G4MS").OrderBy(o => o.EmployeeNo).ToList();
            ViewBag.BindDDlEmployeeNo = queryAccountJoinEmployee;

            return PartialView();
        }

        [HttpGet]
        public JsonResult GetMis()
        {
            TableMisFlowJoinEmployee data = new TableMisFlowJoinEmployee()
            {
                data = QueryMisFlow.ListMisFlow().OrderBy(o => o.EmployeeNo).ToList()
            };
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult CreateMis()
        {
            ITCContext _dbITC = new ITCContext();
            List<AccountJoinEmployee> query = QueryAccount.ListAccountMeyer().Where(w => w.EMPLOYEE_STATUS == "A" && w.SECTION_CODE == "G4MS").OrderBy(o => o.EmployeeNo).ToList();
            ViewBag.BindDDlEmployeeNo = query;
            return PartialView();
        }

        [HttpPost]
        public JsonResult SetMis(ParameterMisFlow cc)
        {
            bool status = false;
            var msg = string.Empty;
            ITCContext _dbITC = new ITCContext();
            if (cc.EmployeeNo == "0" || cc.Division == "0" || cc.JobType == "0")
            {
                status = false;
                msg = "One or more fields have _blank";
            }
            else
            {
                try
                {
                    status = true;
                    msg = "Successful";
                    _dbITC.MisFlow.Add(new MisFlow
                    {
                        EmployeeNo = cc.EmployeeNo,
                        Division = cc.Division,
                        JobType = cc.JobType,
                    });
                    _dbITC.SaveChanges();
                }
                catch (Exception ex)
                {
                    status = false;
                    msg = ex.Message;
                }
            }
            return Json(new { success = status, message = msg });
        }

        [HttpDelete]
        public JsonResult DeleteMis(int id)
        {
            ITCContext _dbITC = new ITCContext();
            MisFlow query = _dbITC.MisFlow.FirstOrDefault(s => s.Id == id);
            if (query == null)
            {
                return Json(new { success = false, message = "Error while Deleting" });
            }
            _dbITC.MisFlow.Remove(query);
            _dbITC.SaveChanges();
            return Json(new { success = true, message = "Delete successful" });
        }

        [HttpGet]
        public JsonResult ModifyMis(int id)
        {
            List<MisFlowJoinEmployee> query = QueryMisFlow.ListMisFlow().Where(w => w.Id == id).ToList();
            return Json(query, JsonRequestBehavior.AllowGet);
        }

        [HttpPut]
        public JsonResult UpdateMis(ParameterMisFlow cc)
        {
            bool status = false;
            var msg = string.Empty;
            ITCContext _dbITC = new ITCContext();
            try
            {
                status = true;
                msg = "Successful";
                MisFlow query = _dbITC.MisFlow.Where(w => w.Id == cc.Id).FirstOrDefault();
                query.EmployeeNo = cc.EmployeeNo;
                query.Division = cc.Division;
                query.JobType = cc.JobType;
                _dbITC.SaveChanges();
            }
            catch (Exception ex)
            {
                status = false;
                msg = ex.Message;
            }

            return Json(new { success = status, message = msg });
        }

        public PartialViewResult JobPriorityMng()
        {
            return PartialView();
        }

        public PartialViewResult Symptom()
        {
            return PartialView();
        }

        public PartialViewResult CreateSymptom()
        {
            return PartialView();
        }

        [HttpPost]
        public JsonResult SetSymptom(ParameterSymtom cc)
        {
            bool status = false;
            var msg = string.Empty;
            ITCContext _dbITC = new ITCContext();

            if (cc.SymptomName == null || cc.SymptomName_Th == null || cc.Score == null || cc.SectionType == "0" || cc.DecisionType == "0")
            {
                status = false;
                msg = "One or more fields have _blank";
            }
            else
            {
                try
                {
                    status = true;
                    msg = "Successful";
                    _dbITC.Symptom.Add(new Symptom
                    {
                        SymptomName = cc.SymptomName,
                        SymptomName_Th = cc.SymptomName_Th,
                        Score = Convert.ToInt32(cc.Score),
                        SectionType = cc.SectionType,
                        DecisionType = cc.DecisionType
                    });
                    _dbITC.SaveChanges();
                }
                catch (Exception ex)
                {
                    status = false;
                    msg = ex.Message;
                }
            }
            return Json(new { success = status, message = msg });
        }

        [HttpGet]
        public JsonResult GetSymptom()
        {
            TableSymptom data = new TableSymptom()
            {
                data = QuerySymptom.ListSymptom()
            };
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpDelete]
        public JsonResult DeleteSymptom(int id)
        {
            ITCContext _dbITC = new ITCContext();
            Symptom query = _dbITC.Symptom.FirstOrDefault(s => s.Id == id);
            if (query == null)
            {
                return Json(new { success = false, message = "Error while Deleting" });
            }
            _dbITC.Symptom.Remove(query);
            _dbITC.SaveChanges();
            return Json(new { success = true, message = "Delete successful" });
        }

        [HttpGet]
        public JsonResult ModifySymptom(int id)
        {
            List<SymptomStore> querySymptom = QuerySymptom.ListSymptom().Where(w => w.Id == id).ToList();
            return Json(querySymptom, JsonRequestBehavior.AllowGet);
        }

        [HttpPut]
        public JsonResult UpdateSymptom(ParameterSymtom cc)
        {
            bool status = false;
            var msg = string.Empty;
            ITCContext _dbITC = new ITCContext();
            try
            {
                status = true;
                msg = "Successful";
                Symptom query = _dbITC.Symptom.Where(w => w.Id == cc.Id).FirstOrDefault();
                query.SymptomName = cc.SymptomName;
                query.SymptomName_Th = cc.SymptomName_Th;
                query.Score = Convert.ToInt32(cc.Score);
                query.DecisionType = cc.DecisionType;
                query.SectionType = cc.SectionType;
                _dbITC.SaveChanges();
            }
            catch (Exception ex)
            {
                status = false;
                msg = ex.Message;
            }
            return Json(new { success = status, message = msg });
        }

        public PartialViewResult Grade()
        {
            return PartialView();
        }

        public PartialViewResult CreateGrade()
        {
            return PartialView();
        }

        [HttpPost]
        public JsonResult SetGrade(ParameterGrade cc)
        {
            bool status = false;
            var msg = string.Empty;
            ITCContext _dbITC = new ITCContext();
            if (cc.GradeCode == null || cc.Score == null)
            {
                status = false;
                msg = "One or more fields have _blank";
            }
            else
            {
                try
                {
                    status = true;
                    msg = "Successful";
                    _dbITC.Grade.Add(new Grade
                    {
                        GradeCode = cc.GradeCode,
                        Score = Convert.ToInt32(cc.Score),
                    });
                    _dbITC.SaveChanges();
                }
                catch (Exception ex)
                {
                    status = false;
                    msg = ex.Message;
                }
            }
            return Json(new { success = status, message = msg });
        }

        [HttpGet]
        public JsonResult GetGrade()
        {
            TableGrade data = new TableGrade()
            {
                data = QueryGrade.ListGrade()
            };
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpDelete]
        public JsonResult DeleteGrade(int id)
        {
            ITCContext _dbITC = new ITCContext();
            Grade query = _dbITC.Grade.FirstOrDefault(s => s.Id == id);
            if (query == null)
            {
                return Json(new { success = false, message = "Error while Deleting" });
            }
            _dbITC.Grade.Remove(query);
            _dbITC.SaveChanges();
            return Json(new { success = true, message = "Delete successful" });
        }

        [HttpGet]
        public JsonResult ModifyGrade(int id)
        {
            List<GradeStore> query = QueryGrade.ListGrade().Where(w => w.Id == id).ToList();
            return Json(query, JsonRequestBehavior.AllowGet);
        }

        [HttpPut]
        public JsonResult UpdateGrade(ParameterGrade cc)
        {
            bool status = false;
            var msg = string.Empty;
            ITCContext _dbITC = new ITCContext();
            try
            {
                status = true;
                msg = "Successful";
                Grade query = _dbITC.Grade.Where(w => w.Id == cc.Id).FirstOrDefault();
                query.GradeCode = cc.GradeCode;
                query.Score = Convert.ToInt32(cc.Score);
                _dbITC.SaveChanges();
            }
            catch (Exception ex)
            {
                status = false;
                msg = ex.Message;
            }
            return Json(new { success = status, message = msg });
        }

        public PartialViewResult Priority()
        {
            return PartialView();
        }

        public PartialViewResult CreatePriority()
        {
            return PartialView();
        }

        [HttpPost]
        public JsonResult SetPriority(ParameterPriority cc)
        {
            bool status = false;
            var msg = string.Empty;
            ITCContext _dbITC = new ITCContext();
            if (cc.PriorityName == null || cc.Score == null)
            {
                status = false;
                msg = "One or more fields have _blank";
            }
            else
            {
                try
                {
                    status = true;
                    msg = "Successful";
                    _dbITC.Priority.Add(new Priority
                    {
                        PriorityName = cc.PriorityName,
                        Score = Convert.ToInt32(cc.Score),
                    });
                    _dbITC.SaveChanges();
                }
                catch (Exception ex)
                {
                    status = false;
                    msg = ex.Message;
                }
            }

            return Json(new { success = status, message = msg });
        }

        [HttpGet]
        public JsonResult GetPriority()
        {
            TablePriority data = new TablePriority()
            {
                data = QueryPriority.ListPriority()
            };
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpDelete]
        public JsonResult DeletePriority(int id)
        {
            ITCContext _dbITC = new ITCContext();
            Priority query = _dbITC.Priority.FirstOrDefault(s => s.Id == id);
            if (query == null)
            {
                return Json(new { success = false, message = "Error while Deleting" });
            }
            _dbITC.Priority.Remove(query);
            _dbITC.SaveChanges();
            return Json(new { success = true, message = "Delete successful" });
        }

        [HttpGet]
        public JsonResult ModifyPriority(int id)
        {
            List<PriorityStore> query = QueryPriority.ListPriority().Where(w => w.Id == id).ToList();
            return Json(query, JsonRequestBehavior.AllowGet);
        }

        [HttpPut]
        public JsonResult UpdatePriority(ParameterPriority cc)
        {
            bool status = false;
            var msg = string.Empty;
            ITCContext _dbITC = new ITCContext();
            try
            {
                status = true;
                msg = "Successful";
                Priority query = _dbITC.Priority.Where(w => w.Id == cc.Id).FirstOrDefault();
                query.PriorityName = cc.PriorityName;
                query.Score = Convert.ToInt32(cc.Score);
                _dbITC.SaveChanges();
            }
            catch (Exception ex)
            {
                status = false;
                msg = ex.Message;
            }
            return Json(new { success = status, message = msg });
        }

        public ActionResult ChangePassword()
        {
            HRMContext _db = new HRMContext();
            ClaimsPrincipal identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            string emp_no = identity.Claims.Where(c => c.Type == "employee_no").Select(c => c.Value).SingleOrDefault();
            AccountJoinEmployee query = QueryAccount.ListAccountMeyer().Where(w => w.EmployeeNo == emp_no).FirstOrDefault();

            ViewBag.BindEmployeeName = query.EMPLOYEE_NAME;
            ViewBag.BindId = query.Id;
            return View();
        }

        [HttpPost]
        public JsonResult SetChangePassword(ParameterAccount cc)
        {
            bool status = false;
            var msg = string.Empty;
            PasswordHasher hasher = new PasswordHasher();
            MILAuthContext _dbMILAuth = new MILAuthContext();
            if (cc.OldPassword == null || cc.NewPassword == null || cc.ConfirmPassword == null)
            {
                status = false;
                msg = "One or more fields have _blank";
            }
            else
            {
                if (cc.NewPassword != cc.ConfirmPassword)
                {
                    status = false;
                    msg = "Please check your password";
                }
                else
                {
                    try
                    {
                        Accounts query = _dbMILAuth.Accounts.SingleOrDefault(w => w.Id == cc.Id);
                        var _pwd = hasher.GenerateIdentityV3Hash(cc.ConfirmPassword, KeyDerivationPrf.HMACSHA1, 10000, 16);
                        var _cfrmPwd = hasher.VerifyIdentityV3Hash(cc.OldPassword, query.PasswordHash);
                        if (_cfrmPwd == true)
                        {
                            status = true;
                            msg = "Successful";
                            query.PasswordHash = _pwd;
                            _dbMILAuth.SaveChanges();
                        }
                    }
                    catch (Exception ex)
                    {
                        status = false;
                        msg = ex.Message;
                    }
                }
            }

            return Json(new { success = status, message = msg });
        }

        public PartialViewResult HardwareAsset()
        {
            ITCContext dbcon = new ITCContext();
            ViewBag.BindEquipmentTypeH = QueryEquipmentType.ListEquipmentType().OrderByDescending(o => o.EquipmentType).ToList();
            ViewBag.BindEquipmentTypeM = QueryEquipmentType.ListEquipmentType().OrderByDescending(o => o.EquipmentType).ToList();
            return PartialView();
        }

        public PartialViewResult CreateHardwareAsset()
        {
            ITCContext dbcon = new ITCContext();
            ViewBag.BindEquipmentType = QueryEquipmentType.ListEquipmentType().OrderByDescending(o => o.EquipmentType).ToList();
            return PartialView();
        }

        [HttpGet]
        public JsonResult GetHardwareAsset()
        {

            TableHardwareAsset data = new TableHardwareAsset()
            {
                data = QueryHardwareAsset.ListHardwareAsset().OrderByDescending(o => o.Id).ToList()
            };
            return Json(data, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult InsertHardwareAsset(ParameterHardwareAsset s)
        {
            ITCContext dbcon = new ITCContext();
            var dbinsert = new HardwareAsset();
            var msg = "";
            bool status = false;

            if (s.Equipment == null || s.EquipmentType == "Select" || s.Description == null || s.SerialNumber == null || s.Model == null || s.Owner == null || s.Location == null || s.TelephoneOwner == null || s.Responsible == null || s.Status == '0')
            {
                msg = "Insert error !!";
                status = false;
            }
            else
            {
                dbcon.HardwareAssets.Add(new HardwareAsset
                {
                    Equipment = s.Equipment,
                    EquipmentType = s.EquipmentType,
                    Description = s.Description,
                    SerialNumber = s.SerialNumber,
                    Model = s.Model,
                    Owner = s.Owner,
                    Location = s.Location,
                    TelephoneOwner = s.TelephoneOwner,
                    Responsible = s.Responsible,
                    Status = s.Status,
                    PreviousOwner = s.PreviousOwner,
                    PreviousLocation = s.PreviousLocation,
                    ServiceVendor = s.ServiceVendor,
                    Company = s.Company,
                    EquipmentGroup = s.EquipmentGroup,
                    PurchaseDate = s.PurchaseDate,
                    ReceiveDate = s.ReceiveDate,
                    ServiceDate = s.ServiceDate,
                    WarrantyType = s.WarrantyType,
                    WarrantyNum = s.WarrantyNum,
                    SafetyNote = s.SafetyNote,
                });

                dbcon.SaveChanges();
                msg = "Insert success !!";
                status = true;
            }

            return Json(new { success = status, message = msg });
        }

        [HttpGet]
        public JsonResult InfoHardwareAsset(int Id)
        {
            List<Parameter> query = QueryHardwareAssetList.HardwareAssetList().Where(s => s.Id == Id).ToList();
            return Json(query, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public JsonResult ModifyHardwareAsset(int Id)
        {
            List<Parameter> query = QueryHardwareAssetList.HardwareAssetList().Where(s => s.Id == Id).ToList();
            return Json(query, JsonRequestBehavior.AllowGet);
        }

        [HttpPut]
        public JsonResult UpdateHardwareAsset(Parameter cc)
        {
            ITCContext dbcon = new ITCContext();
            HardwareAsset query = dbcon.HardwareAssets.FirstOrDefault(s => s.Id == cc.Id);

            query.Id = cc.Id;
            query.EquipmentType = cc.EquipmentType;
            query.Description = cc.Description;
            query.SerialNumber = cc.SerialNumber;
            query.Status = cc.Status;
            query.Responsible = cc.Responsible;
            query.Model = cc.Model;
            query.Owner = cc.Owner;
            query.Location = cc.Location;
            query.ServiceVendor = cc.ServiceVendor;
            query.PreviousOwner = cc.PreviousOwner;
            query.PreviousLocation = cc.PreviousLocation;
            query.PurchaseDate = Convert.ToDateTime(cc.PurchaseDate);
            query.ReceiveDate = Convert.ToDateTime(cc.ReceiveDate);
            query.ServiceDate = Convert.ToDateTime(cc.ServiceDate);
            query.WarrantyType = cc.WarrantyType;
            query.WarrantyNum = cc.WarrantyNum;
            query.SafetyNote = cc.SafetyNote;



            dbcon.SaveChanges();

            return Json(new { success = true, message = "Modify success !!" });
        }

        [HttpDelete]
        public JsonResult DeleteHardwareAsset(int Id)
        {
            ITCContext dbcon = new ITCContext();
            HardwareAsset query = dbcon.HardwareAssets.FirstOrDefault(s => s.Id == Id);

            dbcon.HardwareAssets.Remove(query);
            dbcon.SaveChanges();
            return Json(new { message = "Delete success !!" });
        }

        [HttpPost]
        public JsonResult GetEquipmentLest(HardwareAsset cc)
        {

            HardwareAsset query = QueryEquipmentLast.ListEquipmentLast().LastOrDefault(w => w.EquipmentType == cc.EquipmentType);
            string e = "";

            if (query == null)
            {
                e = cc.EquipmentType + "00001";
            }
            if (query != null && query.Equipment == null)
            {
                e = query.EquipmentType + "00001";
            }
            if (query != null)
            {
                e = query.Equipment;
            }


            return Json(new { success = e });
        }

        public PartialViewResult EquipmentType()
        {
            return PartialView();
        }

        public PartialViewResult CreateEquipmentType()
        {
            return PartialView();
        }


        [HttpGet]
        public JsonResult GetEquipmentType(ParameterEquipmentType cc)
        {
            TableEquipmentType data = new TableEquipmentType()
            {
                data = QueryEquipmentType.ListEquipmentType().OrderByDescending(o => o.Id).ToList()
            };
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult InsertEquipmentType(ParameterEquipmentType s)
        {
            ITCContext dbcon = new ITCContext();
            var dbinsert = new Equipment_Type();
            var msg = "";
            bool status = false;
            if (s.EquipmentType == null || s.Description == null || s.Status == '0')
            {
                msg = "Insert error !!";
                status = false;
            }
            else
            {
                dbcon.Equipment_Types.Add(new Equipment_Type
                {

                    EquipmentType = s.EquipmentType,
                    Description = s.Description,
                    Status = s.Status,

                });

                dbcon.SaveChanges();
                msg = "Insert success !!";
                status = true;
            }

            return Json(new { success = status, message = msg });
        }

        [HttpGet]
        public JsonResult ModifyEquipmentType(int Id)
        {
            List<Equipment_Type> query = QueryEquipmentType.ListEquipmentType().Where(s => s.Id == Id).ToList();
            return Json(query, JsonRequestBehavior.AllowGet);
        }

        [HttpPut]
        public JsonResult UpdateEquipmentType(ParameterEQ cc)
        {
            ITCContext dbcon = new ITCContext();
            Equipment_Type query = dbcon.Equipment_Types.FirstOrDefault(s => s.Id == cc.Id);

            query.Id = cc.Id;
            query.Description = cc.Description;
            query.Status = cc.Status;

            dbcon.SaveChanges();

            return Json(new { success = true, message = "Modify success !!" });
        }

        [HttpDelete]
        public JsonResult DeleteEquipmentType(int Id)
        {
            ITCContext dbcon = new ITCContext();
            Equipment_Type query = dbcon.Equipment_Types.FirstOrDefault(s => s.Id == Id);

            dbcon.Equipment_Types.Remove(query);
            dbcon.SaveChanges();
            return Json(new { message = "Delete success !!" });
        }
    }
}