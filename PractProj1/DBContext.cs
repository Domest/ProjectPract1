using System.Data.Entity;
using PractProj1.Models;

namespace PractProj1
{
    public class DBContext : DbContext
    {
        public DBContext() : base("DbConnectionString") { }
        public DbSet<SendModel> ParsData { get; set; }
    }
}
