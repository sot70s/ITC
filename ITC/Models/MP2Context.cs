using System.Data.Entity;

namespace ITC.Models
{
    public class MP2Context : DbContext
    {
        public DbSet<EQUIP> EQUIP { get; set; }
        public DbSet<EQUIPTYPE> EQTYPE { get; set; }
    }
}