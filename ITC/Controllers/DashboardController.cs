using ITC.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Collections;
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
    public class DashBoardController : Controller
    {
        // GET: DashBoard
        public ActionResult Index()
        {
            ITCContext _dbITC = new ITCContext();
            ViewBag.BindEQUIPTYPE = QueryEquipmentType.ListEquipmentType().OrderByDescending(o => o.EquipmentType).ToList();
            //ViewBag.BindSupportBy = QueryMisFlow.ListMisFlow().Where(w => w.JobType == "Staff" && (w.Division == "IT")).OrderBy(o => o.EmployeeNo).ToList();
            ViewBag.BindSupportBy = (from EQT in _dbITC.HardwareAssets.ToList()
                                     join eq in QueryPersonnel.ListEmployeeMeyer().ToList()
                                     on EQT.Responsible equals eq.EMPLOYEE_NO
                                     select new ParameterPersonal
                                     {
                                         Responsible = eq.EMPLOYEE_NAME,
                                         ResponsibleNo = eq.EMPLOYEE_NO,
                                     })
                .GroupBy(g => new
                {
                    g.Responsible,
                    g.ResponsibleNo
                }, (key, group) => new ParameterHardwareAsset
                {
                    Responsible = key.Responsible,
                    ResponsibleNo = key.ResponsibleNo
                }
                ).ToList();
            return View();
        }

        //have filter
        [HttpPost]
        public JsonResult GetChartDashboard(ParameterfilterChart cc)
        {
            ITCContext _dbITC = new ITCContext();
            ClaimsPrincipal identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            string emp_no = identity.Claims.Where(c => c.Type == "employee_no").Select(c => c.Value).SingleOrDefault();
            List<ParameterChart> objChart = new List<ParameterChart>();
            List<ObjEquipmentType> objCharts = new List<ObjEquipmentType>();
            List<ParameterHardwareAsset> queryEQT = new List<ParameterHardwareAsset>();
            List<ParameterHardwareAsset> queryLO = new List<ParameterHardwareAsset>();
            ParameterChart item;
            ObjEquipmentType item1;


            if (cc.Location == null || cc.Location[0] == "Select All")
            {
                queryLO = QueryDashboard.ListDashboard()
                      .GroupBy(g => new
                      {
                          g.Location
                      }, (key, group) => new ParameterHardwareAsset
                      {
                          Location = key.Location,
                      }).Where(w => (w.Location == "L1" || w.Location == "L2" || w.Location == "L3" || w.Location == "L4" || w.Location == "L5" || w.Location == "L6")).OrderBy(o => o.Location).ToList();
            }
            else
            {

                switch (cc.Location.Count())
                {
                    case 1:
                        queryLO = QueryDashboard.ListDashboard()
                     .GroupBy(g => new
                     {
                         g.Location
                     }, (key, group) => new ParameterHardwareAsset
                     {
                         Location = key.Location,
                     }).Where(w => (w.Location == cc.Location[0])).OrderBy(o => o.Location).ToList();
                        break;

                    case 2:
                        queryLO = QueryDashboard.ListDashboard()
                     .GroupBy(g => new
                     {
                         g.Location
                     }, (key, group) => new ParameterHardwareAsset
                     {
                         Location = key.Location,
                     }).Where(w => (w.Location == cc.Location[0] || w.Location == cc.Location[1])).OrderBy(o => o.Location).ToList();
                        break;

                    case 3:
                        queryLO = QueryDashboard.ListDashboard()
                     .GroupBy(g => new
                     {
                         g.Location
                     }, (key, group) => new ParameterHardwareAsset
                     {
                         Location = key.Location,
                     }).Where(w => (w.Location == cc.Location[0] || w.Location == cc.Location[1] || w.Location == cc.Location[2])).OrderBy(o => o.Location).ToList();
                        break;

                    case 4:
                        queryLO = QueryDashboard.ListDashboard()
                     .GroupBy(g => new
                     {
                         g.Location
                     }, (key, group) => new ParameterHardwareAsset
                     {
                         Location = key.Location,
                     }).Where(w => (w.Location == cc.Location[0] || w.Location == cc.Location[1] || w.Location == cc.Location[2] || w.Location == cc.Location[3])).OrderBy(o => o.Location).ToList();
                        break;

                    case 5:
                        queryLO = QueryDashboard.ListDashboard()
                    .GroupBy(g => new
                    {
                        g.Location,
                    }, (key, group) => new ParameterHardwareAsset
                    {
                        Location = key.Location,
                    }).Where(w => (w.Location == cc.Location[0] || w.Location == cc.Location[1] || w.Location == cc.Location[2] || w.Location == cc.Location[3] || w.Location == cc.Location[4])).OrderBy(o => o.Location).ToList();
                        break;
                }
            }

            if (cc.EquipmentType == null)
            {
                queryEQT = QueryDashboard.ListDashboard().Where(w => (cc.Responsible == "Select" ? w.Responsible == emp_no : w.Responsible == cc.Responsible))
                   .GroupBy(g => new
                   {
                       g.EquipmentType,
                   }, (key, group) => new ParameterHardwareAsset
                   {
                       EquipmentType = key.EquipmentType,
                       CountEquipmentType = group.Count()
                   }).Where(w => (w.EquipmentType == "0ECP" || w.EquipmentType == "0ECN" || w.EquipmentType == "0EPR" || w.EquipmentType == "SUPPORT" || w.EquipmentType == "8-HP" || w.EquipmentType == "SCRUBBER" || w.EquipmentType == "9TY" || w.EquipmentType == "9PT" || w.EquipmentType == "9SS" || w.EquipmentType == "C-MHPLZ")).OrderBy(o => o.EquipmentType).ToList();
            }
            else
            {

                switch (cc.EquipmentType.Count())
                {
                    case 1:
                        queryEQT = QueryDashboard.ListDashboard().Where(w => (cc.Responsible == "Select" ? w.Responsible == emp_no : w.Responsible == cc.Responsible))
                       .GroupBy(g => new
                       {
                           g.EquipmentType
                       }, (key, group) => new ParameterHardwareAsset
                       {
                           EquipmentType = key.EquipmentType,
                           CountEquipmentType = group.Count()
                       }).Where(w => (w.EquipmentType == cc.EquipmentType[0])).OrderBy(o => o.EquipmentType).ToList();
                        break;

                    case 2:
                        queryEQT = QueryDashboard.ListDashboard().Where(w => (cc.Responsible == "Select" ? w.Responsible == emp_no : w.Responsible == cc.Responsible))
                      .GroupBy(g => new
                      {
                          g.EquipmentType
                      }, (key, group) => new ParameterHardwareAsset
                      {
                          EquipmentType = key.EquipmentType,
                          CountEquipmentType = group.Count()
                      }).Where(w => (w.EquipmentType == cc.EquipmentType[0] || w.EquipmentType == cc.EquipmentType[1])).OrderBy(o => o.EquipmentType).ToList();
                        break;
                    case 3:
                        queryEQT = QueryDashboard.ListDashboard().Where(w => (cc.Responsible == "Select" ? w.Responsible == emp_no : w.Responsible == cc.Responsible))
                      .GroupBy(g => new
                      {
                          g.EquipmentType
                      }, (key, group) => new ParameterHardwareAsset
                      {
                          EquipmentType = key.EquipmentType,
                          CountEquipmentType = group.Count()
                      }).Where(w => (w.EquipmentType == cc.EquipmentType[0] || w.EquipmentType == cc.EquipmentType[1] || w.EquipmentType == cc.EquipmentType[2])).OrderBy(o => o.EquipmentType).ToList();
                        break;
                    case 4:
                        queryEQT = QueryDashboard.ListDashboard().Where(w => (cc.Responsible == "Select" ? w.Responsible == emp_no : w.Responsible == cc.Responsible))
                      .GroupBy(g => new
                      {
                          g.EquipmentType
                      }, (key, group) => new ParameterHardwareAsset
                      {
                          EquipmentType = key.EquipmentType,
                          CountEquipmentType = group.Count()
                      }).Where(w => (w.EquipmentType == cc.EquipmentType[0] || w.EquipmentType == cc.EquipmentType[1] || w.EquipmentType == cc.EquipmentType[2] || w.EquipmentType == cc.EquipmentType[3])).OrderBy(o => o.EquipmentType).ToList();
                        break;
                    case 5:
                        queryEQT = QueryDashboard.ListDashboard().Where(w => (cc.Responsible == "Select" ? w.Responsible == emp_no : w.Responsible == cc.Responsible))
                      .GroupBy(g => new
                      {
                          g.EquipmentType
                      }, (key, group) => new ParameterHardwareAsset
                      {
                          EquipmentType = key.EquipmentType,
                          CountEquipmentType = group.Count()
                      }).Where(w => (w.EquipmentType == cc.EquipmentType[0] || w.EquipmentType == cc.EquipmentType[1] || w.EquipmentType == cc.EquipmentType[2] || w.EquipmentType == cc.EquipmentType[3] || w.EquipmentType == cc.EquipmentType[4])).OrderBy(o => o.EquipmentType).ToList();
                        break;
                    case 6:
                        queryEQT = QueryDashboard.ListDashboard().Where(w => (cc.Responsible == "Select" ? w.Responsible == emp_no : w.Responsible == cc.Responsible))
                      .GroupBy(g => new
                      {
                          g.EquipmentType
                      }, (key, group) => new ParameterHardwareAsset
                      {
                          EquipmentType = key.EquipmentType,
                          CountEquipmentType = group.Count()
                      }).Where(w => (w.EquipmentType == cc.EquipmentType[0] || w.EquipmentType == cc.EquipmentType[1] || w.EquipmentType == cc.EquipmentType[2] || w.EquipmentType == cc.EquipmentType[3] || w.EquipmentType == cc.EquipmentType[4] || w.EquipmentType == cc.EquipmentType[5])).OrderBy(o => o.EquipmentType).ToList();
                        break;
                    case 7:
                        queryEQT = QueryDashboard.ListDashboard().Where(w => (cc.Responsible == "Select" ? w.Responsible == emp_no : w.Responsible == cc.Responsible))
                      .GroupBy(g => new
                      {
                          g.EquipmentType
                      }, (key, group) => new ParameterHardwareAsset
                      {
                          EquipmentType = key.EquipmentType,
                          CountEquipmentType = group.Count()
                      }).Where(w => (w.EquipmentType == cc.EquipmentType[0] || w.EquipmentType == cc.EquipmentType[1] || w.EquipmentType == cc.EquipmentType[2] || w.EquipmentType == cc.EquipmentType[3] || w.EquipmentType == cc.EquipmentType[4] || w.EquipmentType == cc.EquipmentType[5] || w.EquipmentType == cc.EquipmentType[6])).OrderBy(o => o.EquipmentType).ToList();
                        break;
                    case 8:
                        queryEQT = QueryDashboard.ListDashboard().Where(w => (cc.Responsible == "Select" ? w.Responsible == emp_no : w.Responsible == cc.Responsible))
                      .GroupBy(g => new
                      {
                          g.EquipmentType
                      }, (key, group) => new ParameterHardwareAsset
                      {
                          EquipmentType = key.EquipmentType,
                          CountEquipmentType = group.Count()
                      }).Where(w => (w.EquipmentType == cc.EquipmentType[0] || w.EquipmentType == cc.EquipmentType[1] || w.EquipmentType == cc.EquipmentType[2] || w.EquipmentType == cc.EquipmentType[3] || w.EquipmentType == cc.EquipmentType[4] || w.EquipmentType == cc.EquipmentType[5] || w.EquipmentType == cc.EquipmentType[6] || w.EquipmentType == cc.EquipmentType[7])).OrderBy(o => o.EquipmentType).ToList();
                        break;
                    case 9:
                        queryEQT = QueryDashboard.ListDashboard().Where(w => (cc.Responsible == "Select" ? w.Responsible == emp_no : w.Responsible == cc.Responsible))
                      .GroupBy(g => new
                      {
                          g.EquipmentType
                      }, (key, group) => new ParameterHardwareAsset
                      {
                          EquipmentType = key.EquipmentType,
                          CountEquipmentType = group.Count()
                      }).Where(w => (w.EquipmentType == cc.EquipmentType[0] || w.EquipmentType == cc.EquipmentType[1] || w.EquipmentType == cc.EquipmentType[2] || w.EquipmentType == cc.EquipmentType[3] || w.EquipmentType == cc.EquipmentType[4] || w.EquipmentType == cc.EquipmentType[5] || w.EquipmentType == cc.EquipmentType[6] || w.EquipmentType == cc.EquipmentType[7] || w.EquipmentType == cc.EquipmentType[8])).OrderBy(o => o.EquipmentType).ToList();
                        break;
                    case 10:
                        queryEQT = QueryDashboard.ListDashboard().Where(w => (cc.Responsible == "Select" ? w.Responsible == emp_no : w.Responsible == cc.Responsible))
                      .GroupBy(g => new
                      {
                          g.EquipmentType
                      }, (key, group) => new ParameterHardwareAsset
                      {
                          EquipmentType = key.EquipmentType,
                          CountEquipmentType = group.Count()
                      }).Where(w => (w.EquipmentType == cc.EquipmentType[0] || w.EquipmentType == cc.EquipmentType[1] || w.EquipmentType == cc.EquipmentType[2] || w.EquipmentType == cc.EquipmentType[3] || w.EquipmentType == cc.EquipmentType[4] || w.EquipmentType == cc.EquipmentType[5] || w.EquipmentType == cc.EquipmentType[6] || w.EquipmentType == cc.EquipmentType[7] || w.EquipmentType == cc.EquipmentType[8] || w.EquipmentType == cc.EquipmentType[9])).OrderBy(o => o.EquipmentType).ToList();
                        break;
                }
            }

            for (int i = 0; i < queryLO.Count(); i++)
            {
                objCharts = new List<ObjEquipmentType>();
                item = new ParameterChart();

                List<ParameterHardwareAsset> query1 = QueryDashboard.ListDashboard()
                .Where(w => (w.Location == queryLO[i].Location) &&
                            (cc.Responsible == "Select" ? w.Responsible == emp_no : w.Responsible == cc.Responsible) &&

                 (((cc.EquipmentAgingType == "Select" || cc.EquipmentAgingType == "Year" || cc.EquipmentAgingType == "Month" || cc.EquipmentAgingType == "Day") && (cc.EquipmentAgingFrom == 0 && cc.EquipmentAgingTo == 0) ? (Convert.ToInt32(w.EquipmentAgingTypeYear) >= 0 && Convert.ToInt32(w.EquipmentAgingTypeYear) <= 3)
                        : (cc.EquipmentAgingType == "Year" ? (Convert.ToInt32(w.EquipmentAgingTypeYear) >= cc.EquipmentAgingFrom && Convert.ToInt32(w.EquipmentAgingTypeYear) <= cc.EquipmentAgingTo)
                        : (cc.EquipmentAgingType == "Month" ? (Convert.ToInt32(w.EquipmentAgingTypeMonth) >= cc.EquipmentAgingFrom && Convert.ToInt32(w.EquipmentAgingTypeMonth) <= cc.EquipmentAgingTo)
                        : (cc.EquipmentAgingType == "Day" ? (Convert.ToInt32(w.EquipmentAgingTypeDay) >= cc.EquipmentAgingFrom && Convert.ToInt32(w.EquipmentAgingTypeDay) <= cc.EquipmentAgingTo)
                        : (Convert.ToInt32(w.EquipmentAgingTypeYear) >= 0 && Convert.ToInt32(w.EquipmentAgingTypeYear) <= 3)))))))
                .GroupBy(g => new
                {
                    g.Location,
                    g.EquipmentType,
                }, (key, group) => new ParameterHardwareAsset
                {
                    Location = key.Location,
                    EquipmentType = key.EquipmentType,
                    CountEquipmentType = group.Count()

                }).ToList();

                for (int j = 0; j < query1.Count(); j++)
                {
                    item1 = new ObjEquipmentType();
                    item1.EquipmentType = query1[j].EquipmentType;
                    item1.CountType = query1[j].CountEquipmentType;
                    objCharts.Add(item1);
                }

                item.Location = queryLO[i].Location;
                item.objEquipmentType = objCharts;
                objChart.Add(item);

            }

            return Json(new { obj = objChart.ToList(), objEQT = queryEQT.ToList() });
        }


    }
}
