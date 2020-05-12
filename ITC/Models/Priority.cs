using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace ITC.Models
{
    public class ParameterPriority
    {
        public int Id { get; set; }
        public string PriorityName { get; set; }
        public string Score { get; set; }
    }

    [Table("Priority")]
    public class Priority
    {
        [Key]
        public int Id { get; set; }
        public string PriorityName { get; set; }
        public int Score { get; set; }
    }

    public class PriorityStore
    {
        public int Id { get; set; }
        public string PriorityName { get; set; }
        public int Score { get; set; }
    }

    public class TablePriority
    {
        public List<PriorityStore> data { get; set; }
    }

    public class QueryPriority
    {
        public static List<PriorityStore> ListPriority()
        {
            ITCContext _dbITC = new ITCContext();
            List<PriorityStore> query = _dbITC.Priority
                .Select(s => new PriorityStore
                {
                    Id = s.Id,
                    PriorityName = s.PriorityName,
                    Score = s.Score,
                }).ToList();

            return query;
        }
    }
}