using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace PractProj1
{
    public class DBContext : DbContext
    {
        public DBContext() : base("DbConnectionString") { }

        public DbSet<SendModel> ParsData { get; set; }

        //public DBContext(DbContextOptions<DBContext> options)
        //    : base(options)
        //{
        //}
    }
}
