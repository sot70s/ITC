using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace ITC.Models
{
    public class ParameterGrade
    {
        public int Id { get; set; }
        public string GradeCode { get; set; }
        public string Score { get; set; }
    }

    [Table("Grade")]
    public class Grade
    {
        [Key]
        public int Id { get; set; }
        public string GradeCode { get; set; }
        public int Score { get; set; }
    }

    public class GradeStore
    {
        public int Id { get; set; }
        public string GradeCode { get; set; }
        public int Score { get; set; }
    }

    public class TableGrade
    {
        public List<GradeStore> data { get; set; }
    }

    public class QueryGrade
    {
        public static List<GradeStore> ListGrade()
        {
            ITCContext _dbITC = new ITCContext();
            List<GradeStore> query = _dbITC.Grade
                .Select(s => new GradeStore
                {
                    Id = s.Id,
                    GradeCode = s.GradeCode,
                    Score = s.Score,
                }).ToList();

            return query;
        }
    }
}