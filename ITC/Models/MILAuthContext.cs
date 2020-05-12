using System.Data.Entity;

namespace ITC.Models
{
    public class MILAuthContext : DbContext
    {
        public DbSet<Accounts> Accounts { get; set; }
    }
}