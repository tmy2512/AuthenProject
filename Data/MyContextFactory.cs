using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class MyContextFactory : IDesignTimeDbContextFactory<MyContext>
    {
        public MyContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<MyContext>();
            optionsBuilder.UseSqlServer("Server=Admin-PC;Initial Catalog=AUTHEN_DB;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true");

            return new MyContext(optionsBuilder.Options);
        }
    }
}
