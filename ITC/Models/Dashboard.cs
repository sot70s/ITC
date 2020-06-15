using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ITC.Models
{
    public class ParameterDashboard
    {
        public string Equipment { get; set; }
        public string EquipmentType { get; set; }
        public string Location { get; set; }
        public string Responsible { get; set; }
        public DateTime PurchaseDate { get; set; }
        public string EquipmentAging { get; set; }
        public string EquipmentAgingType { get; set; }
        public int EquipmentAgingFrom { get; set; }
        public int EquipmentAgingTo { get; set; }
        public List<ObjEquipmentType> objEquipmentType { get; set; }
    }

    public class ParameterfilterChart
    {
        public string[] EquipmentType { get; set; }
        public string[] Location { get; set; }
        public string Responsible { get; set; }
        public string EquipmentAgingType { get; set; }
        public int EquipmentAgingFrom { get; set; }
        public int EquipmentAgingTo { get; set; }
    }

    public class ParameterChart
    {
        public List<ObjEquipmentType> objEquipmentType { get; set; }
        public string Location { get; set; }

    }

    public class ObjEquipmentType
    {
        public string EquipmentType { get; set; }
        public int CountType { get; set; }
        public string Responsible { get; set; }
        public string EquipmentAging { get; set; }
        public string EquipmentAgingType { get; set; }
        public int EquipmentAgingFrom { get; set; }
        public int EquipmentAgingTo { get; set; }
    }

    public class TableDashboard
    {
        public List<ParameterDashboard> data { get; set; }
    }

    public class QueryDashboard
    {
        public static List<ParameterHardwareAsset> ListDashboard()
        {
            string getAgeMachineYear(DateTime purchaseDate)
            {
                double ApproxDaysPerYear = 365.25;
                int iDays = (DateTime.Now - purchaseDate).Days;
                int iYear = (int)(iDays / ApproxDaysPerYear);
                return Convert.ToString(iYear);
            }
            string getAgeMachineMonth(DateTime purchaseDate)
            {
                double ApproxDaysPerMonth = 30.4375;
                double ApproxDaysPerYear = 365.25;
                int iDays = (DateTime.Now - purchaseDate).Days;
                int iYear = (int)(iDays / ApproxDaysPerYear);
                int iMonths = (int)(iDays / ApproxDaysPerMonth);
                return Convert.ToString(iMonths);
            }

            ITCContext _dbITC = new ITCContext();
            List<ParameterHardwareAsset> query = _dbITC.HardwareAssets.ToList()
                .Select(s => new ParameterHardwareAsset
                {
                    Equipment = s.Equipment,
                    EquipmentType = s.EquipmentType,
                    Location = s.Location,
                    Responsible = s.Responsible,
                    PurchaseDate = s.PurchaseDate,
                    EquipmentAgingTypeYear = getAgeMachineYear(s.PurchaseDate),
                    EquipmentAgingTypeMonth = getAgeMachineMonth(s.PurchaseDate),
                    EquipmentAgingTypeDay = (DateTime.Now - s.PurchaseDate).Days.ToString(),

                }).ToList();

            return query;
        }
    }
}