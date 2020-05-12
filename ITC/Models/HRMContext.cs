using System.Data.Entity;

namespace ITC.Models
{
    public class HRMContext : DbContext
    {
        public DbSet<HRM_Employee> HRM_Employee { get; set; }
        public DbSet<HRM_Employee_Manager> HRM_Employee_Manager { get; set; }
        public DbSet<HRM_Section_Master> HRM_Section_Master { get; set; }
    }
}