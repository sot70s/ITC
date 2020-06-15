using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;

namespace ITC.Models
{
    public class ParameterHardwareAsset
    {
        public int Id { get; set; }
        public string Equipment { get; set; }
        public string EquipmentType { get; set; }
        public string Description { get; set; }
        public string SerialNumber { get; set; }
        public string Model { get; set; }
        public string Owner { get; set; }
        public string Location { get; set; }
        public string TelephoneOwner { get; set; }
        public string Responsible { get; set; }
        public int Status { get; set; }
        public string PreviousOwner { get; set; }
        public string PreviousLocation { get; set; }
        public string ServiceVendor { get; set; }
        public string Company { get; set; }
        public string EquipmentGroup { get; set; }
        public DateTime PurchaseDate { get; set; }
        public DateTime ReceiveDate { get; set; }
        public DateTime ServiceDate { get; set; }
        public string WarrantyType { get; set; }
        public int WarrantyNum { get; set; }
        public string SafetyNote { get; set; }
        public int CountEquipmentType { get; set; }
        public string EquipmentAgingType { get; set; }
        public string EquipmentAgingTypeYear { get; set; }
        public string EquipmentAgingTypeMonth { get; set; }
        public string EquipmentAgingTypeDay { get; set; }
        public int EquipmentAgingNum { get; set; }
        public int EquipmentAgingFrom { get; set; }
        public int EquipmentAgingTo { get; set; }
        public string SupportBy { get; set; }
    }

    public class ParameterPersonal
    {
        public int Id { get; set; }
        public string Equipment { get; set; }
        public string EquipmentType { get; set; }
        public string Description { get; set; }
        public string SerialNumber { get; set; }
        public string Model { get; set; }
        public string Owner { get; set; }
        public string Location { get; set; }
        public string Section { get; set; }
        public string Department { get; set; }
        public string TelephoneOwner { get; set; }
        public string Responsible { get; set; }
        public int Status { get; set; }
        public string PreviousOwner { get; set; }
        public string PreviousLocation { get; set; }
        public string ServiceVendor { get; set; }
        public string ContactVendor { get; set; }
        public string Company { get; set; }
        public string EquipmentGroup { get; set; }
        public string ReceiveDate { get; set; }
        public string PurchaseDate { get; set; }
        public string ServiceDate { get; set; }
        public string WarrantyDate { get; set; }
        public string WarrantyType { get; set; }
        public int WarrantyNum { get; set; }
        public string StopPurchaseDate { get; set; }
        public string StopServiceDate { get; set; }
        public string StopWarrantyDate { get; set; }
        public string EquipmentAging { get; set; }
        public string EquipmentAgingType { get; set; }
        public int EquipmentAgingNum { get; set; }
        public string LastCalibateDate { get; set; }
        public string SafetyNote { get; set; }
        public string SupportBy { get; set; }
    }

    public class HRM_Master
    {
        public string SECTION_CODE { get; set; }
        public string SECTION_DESCRIPTION { get; set; }
        public string DIVISION_CODE { get; set; }
        public string DIVISION_DESCRIPTION { get; set; }
        public string DEPARTMENT_CODE { get; set; }
        public string DEPARTMENT_DESCRIPTION { get; set; }
        public int COUNTDEPARTMENT { get; set; }
    }


    [Table("HardwareAsset")]
    public class HardwareAsset
    {
        [Key]
        public int Id { get; set; }
        public string Equipment { get; set; }
        public string EquipmentType { get; set; }
        public string Description { get; set; }
        public string SerialNumber { get; set; }
        public string Model { get; set; }
        public string Owner { get; set; }
        public string Location { get; set; }
        public string TelephoneOwner { get; set; }
        public string Responsible { get; set; }
        public int Status { get; set; }
        public string PreviousOwner { get; set; }
        public string PreviousLocation { get; set; }
        public string ServiceVendor { get; set; }
        public string Company { get; set; }
        public string EquipmentGroup { get; set; }
        public DateTime PurchaseDate { get; set; }
        public DateTime ReceiveDate { get; set; }
        public DateTime ServiceDate { get; set; }
        public string WarrantyType { get; set; }
        public int WarrantyNum { get; set; }
        public string SafetyNote { get; set; }
        public string SupportBy { get; set; }

    }

    public class TableHardwareAsset
    {
        public List<HardwareAsset> data { get; set; }
    }

    public class TablePersonal
    {
        public List<ParameterPersonal> data { get; set; }
    }

    public class QueryHardwareAsset
    {
        public static List<HardwareAsset> ListHardwareAsset()
        {
            ITCContext _dbITC = new ITCContext();
            List<HardwareAsset> query = _dbITC.HardwareAssets.ToList();
            return query;
        }
    }

    public class QueryHardwareAssetList
    {
        public static List<ParameterPersonal> HardwareAssetList()
        {
            ITCContext _dbITC = new ITCContext();
            List<ParameterPersonal> query = _dbITC.HardwareAssets.ToList()
                .Select(s => new ParameterPersonal
                {
                    Id = s.Id,
                    Equipment = (s.Equipment == null) ? "N/A" : s.Equipment,
                    EquipmentType = (s.EquipmentType == null) ? "N/A" : s.EquipmentType,
                    Description = (s.Description == null) ? "N/A" : s.Description,
                    SerialNumber = (s.SerialNumber == null) ? "N/A" : s.SerialNumber,
                    Model = (s.Model == null) ? "N/A" : s.Model,
                    Owner = (s.Owner == null) ? "N/A" : s.Owner,
                    Location = (s.Location == null) ? "N/A" : s.Location,
                    TelephoneOwner = (s.TelephoneOwner == null) ? "N/A" : s.TelephoneOwner,
                    Responsible = (s.Responsible == null) ? "N/A" : s.Responsible,
                    Status = s.Status,
                    PreviousOwner = (s.PreviousOwner == null) ? "N/A" : s.PreviousOwner,
                    PreviousLocation = (s.PreviousLocation == null) ? "N/A" : s.PreviousLocation,
                    ServiceVendor = (s.ServiceVendor == null) ? "N/A" : s.ServiceVendor,
                    Company = (s.Company == null) ? "N/A" : s.Company,
                    EquipmentGroup = (s.EquipmentGroup == null) ? "N/A" : s.EquipmentGroup,
                    PurchaseDate = String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(s.PurchaseDate)),
                    ReceiveDate = String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(s.ReceiveDate)),
                    ServiceDate = String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(s.ServiceDate)),
                    //PurchaseDate = (String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(s.PurchaseDate)) == "01-Jan-0001") ? "N/A" : String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(s.PurchaseDate)),
                    //ReceiveDate = (String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(s.ReceiveDate)) == "01-Jan-0001") ? "N/A" : String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(s.ReceiveDate)),
                    //ServiceDate = (String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(s.ServiceDate)) == "01-Jan-0001") ? "N/A" : String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(s.ServiceDate)),
                    WarrantyType = (s.WarrantyType == null) ? "N/A" : s.WarrantyType,
                    WarrantyNum = s.WarrantyNum,
                    SafetyNote = (s.SafetyNote == null) ? "N/A" : s.SafetyNote,
                    SupportBy = (s.SupportBy == null) ? "N/A" : s.SupportBy

                }).ToList();
            return query;
        }

        public static List<ParameterPersonal> PersonalList(int id)
        {
            string GetAgeMachine(DateTime purchaseDate)
            {
                double ApproxDaysPerMonth = 30.4375;
                double ApproxDaysPerYear = 365.25;
                var formatStr = string.Empty;
                int iDays = (DateTime.Now - purchaseDate).Days;

                int iYear = (int)(iDays / ApproxDaysPerYear);
                iDays -= (int)(iYear * ApproxDaysPerYear);

                int iMonths = (int)(iDays / ApproxDaysPerMonth);
                iDays -= (int)(iMonths * ApproxDaysPerMonth);

                if (iYear > 0)
                    formatStr += iYear + " Year ";
                if (iMonths > 0)
                    formatStr += iMonths + " Month ";
                if (iDays > 0)
                    formatStr += iDays + " Day";
                if (iYear > 1000)
                    formatStr = "N/A";
                return formatStr;
            }

            string GetwarrantyDate(DateTime warrantyDate, string AgeType, int AgeNum)
            {
                var formatStr = string.Empty;
                var dd = string.Empty;
                int iday = 0;
                if (AgeType == "Year")
                    iday = AgeNum * 365;
                formatStr = warrantyDate.AddDays(iday).ToString();
                if (AgeType == "Month")
                    dd = String.Format("{0:MMM}", Convert.ToDateTime(warrantyDate));
                iday = (AgeNum * 30);
                formatStr = warrantyDate.AddDays(iday).ToString();
                if (AgeType == "Day")
                    iday = AgeNum;
                formatStr = warrantyDate.AddDays(iday).ToString();

                return formatStr;
            }

            ITCContext _dbITC = new ITCContext();
            List<ParameterPersonal> query = (from EQT in _dbITC.HardwareAssets.Where(w => w.Id == id).ToList()
                                             join eq in QueryPersonnel.ListEmployeeMeyer().ToList()
                                             on EQT.Owner equals eq.EMPLOYEE_NO

                                             join RES in QueryPersonnel.ListEmployeeMeyer().ToList()
                                             on EQT.Responsible equals RES.EMPLOYEE_NO

                                             join res in QueryPersonnel.ListEmployeeMeyer().ToList()
                                             on EQT.PreviousOwner equals res.EMPLOYEE_NO
                                             select new ParameterPersonal
                                             {
                                                 Id = EQT.Id,
                                                 Equipment = EQT.Equipment,
                                                 EquipmentType = (EQT.EquipmentType == null) ? "N/A" : EQT.EquipmentType,
                                                 Description = (EQT.Description == null) ? "N/A" : EQT.Description,
                                                 SerialNumber = (EQT.SerialNumber == null) ? "N/A" : EQT.SerialNumber,
                                                 Model = (EQT.Model == null) ? "N/A" : EQT.Model,
                                                 Owner = (eq.EMPLOYEE_NAME == null) ? "N/A" : eq.EMPLOYEE_NAME,
                                                 Location = (EQT.Location == null) ? "N/A" : EQT.Location,
                                                 Section = (eq.SECTION_CODE == null) ? "N/A" : eq.SECTION_CODE,
                                                 Department = (eq.DEPARTMENT_DESCRIPTION == null) ? "N/A" : eq.DEPARTMENT_DESCRIPTION,
                                                 TelephoneOwner = (EQT.TelephoneOwner == null) ? "N/A" : EQT.TelephoneOwner,
                                                 PreviousOwner = (res.EMPLOYEE_NAME == null) ? "N/A" : res.EMPLOYEE_NAME,
                                                 PreviousLocation = (EQT.PreviousLocation == null) ? "N/A" : EQT.PreviousLocation,
                                                 Responsible = (RES.EMPLOYEE_NAME == null) ? "N/A" : RES.EMPLOYEE_NAME,
                                                 ServiceVendor = (EQT.ServiceVendor == null) ? "N/A" : EQT.ServiceVendor,
                                                 //ContactVendor = (EQT.ContactVendor == null) ? "N/A" : EQT.ContactVendor,
                                                 Company = (EQT.Company == null) ? "N/A" : EQT.Company,
                                                 EquipmentGroup = (EQT.EquipmentGroup == null) ? "N/A" : EQT.EquipmentGroup,
                                                 PurchaseDate = (String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(EQT.PurchaseDate)) == "01-Jan-0001") ? "N/A" : String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(EQT.PurchaseDate)),
                                                 ReceiveDate = (String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(EQT.ReceiveDate)) == "01-Jan-0001") ? "N/A" : String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(EQT.ReceiveDate)),
                                                 ServiceDate = (String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(EQT.ServiceDate)) == "01-Jan-0001") ? "N/A" : String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(EQT.ServiceDate)),
                                                 WarrantyDate = (String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(GetwarrantyDate(EQT.ReceiveDate, EQT.WarrantyType, EQT.WarrantyNum))) == "01-Jan-0001") ? "N/A" : String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(GetwarrantyDate(EQT.ReceiveDate, EQT.WarrantyType, EQT.WarrantyNum))),
                                                 //LastCalibateDate = (EQT.LastCalibateDate == null) ? "N/A" : EQT.LastCalibateDate,
                                                 SafetyNote = (EQT.SafetyNote == null) ? "N/A" : EQT.SafetyNote,
                                                 EquipmentAging = GetAgeMachine(EQT.PurchaseDate),
                                                 SupportBy = (EQT.SupportBy == null) ? "N/A" : EQT.SupportBy

                                             }).OrderByDescending(o => o.Id).ToList();
            return query;
        }

        public static List<ParameterPersonal> PersonelList()
        {
            string GetAging(DateTime purchaseDate)
            {
                double ApproxDaysPerMonth = 30.4375;
                double ApproxDaysPerYear = 365.25;
                int formatStr = 0;
                int iDays = (DateTime.Now - purchaseDate).Days;
                int iYear = (int)(iDays / ApproxDaysPerYear);
                iDays -= (int)(iYear * ApproxDaysPerYear);
                int iMonths = (int)(iDays / ApproxDaysPerMonth);
                iDays -= (int)(iMonths * ApproxDaysPerMonth);
                if (iYear > 0 && (iMonths > 0 || iMonths == 0) && (iDays > 0 || iDays == 0))
                    formatStr = iYear;
                if (iMonths > 0 && (iYear == 0) && (iDays > 0 || iDays == 0))
                    formatStr = iMonths;
                if (iDays > 0 && iMonths == 0 && iYear == 0)
                    formatStr = iDays;

                return Convert.ToString(formatStr);
            }

            string GetAgingType(DateTime purchaseDate)
            {
                double ApproxDaysPerMonth = 30.4375;
                double ApproxDaysPerYear = 365.25;
                var formatStr = string.Empty;
                int iDays = (DateTime.Now - purchaseDate).Days;
                int iYear = (int)(iDays / ApproxDaysPerYear);
                iDays -= (int)(iYear * ApproxDaysPerYear);
                int iMonths = (int)(iDays / ApproxDaysPerMonth);
                iDays -= (int)(iMonths * ApproxDaysPerMonth);
                if (iYear > 0 && (iMonths > 0 || iMonths == 0) && (iDays > 0 || iDays == 0))
                    formatStr = "Year";
                if (iMonths > 0 && (iYear == 0) && (iDays > 0 || iDays == 0))
                    formatStr = "Month";
                if (iDays > 0 && iMonths == 0 && iYear == 0)
                    formatStr = "Day";
                return formatStr;
            }

            string GetwarrantyDate(DateTime warrantyDate, string AgeType, int AgeNum)
            {
                var formatStr = string.Empty;
                var dd = string.Empty;
                int iday = 0;
                if (AgeType == "Year")
                    iday = AgeNum * 365;
                formatStr = warrantyDate.AddDays(iday).ToString();
                if (AgeType == "Month")
                    dd = String.Format("{0:MMM}", Convert.ToDateTime(warrantyDate));
                iday = (AgeNum * 30);
                formatStr = warrantyDate.AddDays(iday).ToString();
                if (AgeType == "Day")
                    iday = AgeNum;
                formatStr = warrantyDate.AddDays(iday).ToString();

                return formatStr;
            }

            ITCContext _dbITC = new ITCContext();
            List<ParameterPersonal> query = (from EQT in _dbITC.HardwareAssets.ToList()
                                             join eq in QueryPersonnel.ListEmployeeMeyer().ToList()
                                             on EQT.Owner equals eq.EMPLOYEE_NO

                                             join RES in QueryPersonnel.ListEmployeeMeyer().ToList()
                                             on EQT.Responsible equals RES.EMPLOYEE_NO

                                             join res in QueryPersonnel.ListEmployeeMeyer().ToList()
                                             on EQT.PreviousOwner equals res.EMPLOYEE_NO
                                             select new ParameterPersonal
                                             {
                                                 Id = EQT.Id,
                                                 Equipment = EQT.Equipment,
                                                 EquipmentType = (EQT.EquipmentType == null) ? "N/A" : EQT.EquipmentType,
                                                 Description = (EQT.Description == null) ? "N/A" : EQT.Description,
                                                 SerialNumber = (EQT.SerialNumber == null) ? "N/A" : EQT.SerialNumber,
                                                 Model = (EQT.Model == null) ? "N/A" : EQT.Model,
                                                 Status = EQT.Status,
                                                 Owner = (eq.EMPLOYEE_NAME == null) ? "N/A" : eq.EMPLOYEE_NAME,
                                                 Location = (EQT.Location == null) ? "N/A" : EQT.Location,
                                                 Section = (eq.SECTION_CODE == null) ? "N/A" : eq.SECTION_CODE,
                                                 Department = (eq.DEPARTMENT_DESCRIPTION == null) ? "N/A" : eq.DEPARTMENT_DESCRIPTION,
                                                 TelephoneOwner = (EQT.TelephoneOwner == null) ? "N/A" : EQT.TelephoneOwner,
                                                 PreviousOwner = (res.EMPLOYEE_NAME == null) ? "N/A" : res.EMPLOYEE_NAME,
                                                 PreviousLocation = (EQT.PreviousLocation == null) ? "N/A" : EQT.PreviousLocation,
                                                 Responsible = (RES.EMPLOYEE_NAME == null) ? "N/A" : RES.EMPLOYEE_NAME,
                                                 ServiceVendor = (EQT.ServiceVendor == null) ? "N/A" : EQT.ServiceVendor,
                                                 //ContactVendor = (EQT.ContactVendor == null) ? "N/A" : EQT.ContactVendor,
                                                 Company = (EQT.Company == null) ? "N/A" : EQT.Company,
                                                 EquipmentGroup = (EQT.EquipmentGroup == null) ? "N/A" : EQT.EquipmentGroup,
                                                 //PurchaseDate = (String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(EQT.PurchaseDate)) == "01-Jan-0001") ? "N/A" : String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(EQT.PurchaseDate)),
                                                 //ReceiveDate = (String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(EQT.ReceiveDate)) == "01-Jan-0001") ? "N/A" : String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(EQT.ReceiveDate)),
                                                 //ServiceDate = (String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(EQT.ServiceDate)) == "01-Jan-0001") ? "N/A" : String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(EQT.ServiceDate)),
                                                 WarrantyDate = String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(GetwarrantyDate(EQT.ReceiveDate, EQT.WarrantyType, EQT.WarrantyNum))),
                                                 PurchaseDate = String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(EQT.PurchaseDate)),
                                                 ReceiveDate = String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(EQT.ReceiveDate)),
                                                 ServiceDate = String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(EQT.ServiceDate)),

                                                 EquipmentAgingType = GetAgingType(EQT.PurchaseDate),
                                                 EquipmentAgingNum = Convert.ToInt32(GetAging(EQT.PurchaseDate)),
                                                 //LastCalibateDate = (EQT.LastCalibateDate == null) ? "N/A" : EQT.LastCalibateDate,
                                                 SafetyNote = (EQT.SafetyNote == null) ? "N/A" : EQT.SafetyNote,
                                                 //EquipmentAging = getAgeMachine(EQT.PurchaseDate),
                                                 SupportBy = (EQT.SupportBy == null) ? "N/A" : EQT.SupportBy
                                             }).OrderByDescending(o => o.Id).ToList();
            return query;
        }
    }

    public class QueryEquipmentLast
    {
        public static List<HardwareAsset> ListEquipmentLast()
        {
            ITCContext _dbITC = new ITCContext();
            List<HardwareAsset> query = _dbITC.HardwareAssets.OrderByDescending(o => o.Id).ToList()
                .Select(s => new HardwareAsset
                {
                    Id = s.Id,
                    Equipment = s.Equipment,
                    EquipmentType = s.EquipmentType,

                }).ToList();

            return query;
        }

    }
}