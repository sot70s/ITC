using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace ITC.Models
{
    public class ParameterSymtom
    {
        public int Id { get; set; }
        public string SymptomName { get; set; }
        public string SymptomName_Th { get; set; }
        public string Score { get; set; }
        public string DecisionType { get; set; }
        public string SectionType { get; set; }
        public string StandardDate { get; set; }
        public string CriticalDate { get; set; }
        public string CriticalPercent { get; set; }
    }

    [Table("Symptom")]
    public class Symptom
    {
        [Key]
        public int Id { get; set; }
        public string SymptomName { get; set; }
        public string SymptomName_Th { get; set; }
        public int Score { get; set; }
        public string DecisionType { get; set; }
        public string SectionType { get; set; }
        public int StandardDate { get; set; }
        public int CriticalDate { get; set; }
        public int CriticalPercent { get; set; }
    }

    public class SymptomStore
    {
        public int Id { get; set; }
        public string SymptomName { get; set; }
        public string SymptomName_Th { get; set; }
        public int Score { get; set; }
        public string Decision { get; set; }
        public string DecisionType { get; set; }
        public string SectionType { get; set; }
        public int StandardDate { get; set; }
        public int CriticalDate { get; set; }
        public int CriticalPercent { get; set; }
    }

    public class TableSymptom
    {
        public List<SymptomStore> data { get; set; }
    }

    public class QuerySymptom
    {
        public static List<SymptomStore> ListSymptom()
        {
            ITCContext _dbITC = new ITCContext();
            List<SymptomStore> query = _dbITC.Symptom
                .Select(s => new SymptomStore
                {
                    Id = s.Id,
                    SymptomName = s.SymptomName,
                    SymptomName_Th = s.SymptomName_Th,
                    Score = s.Score,
                    Decision = s.DecisionType,
                    DecisionType = (s.DecisionType == "A") ? "A (Need user manager approval)" : (s.DecisionType == "N") ? "N (Pass through to MIS)" : "T (Ticket of helpdesk service)",
                    SectionType = s.SectionType,
                    StandardDate = s.StandardDate,
                    CriticalDate = s.CriticalDate,
                    CriticalPercent = s.CriticalPercent
                }).ToList();
            return query;
        }
    }
}