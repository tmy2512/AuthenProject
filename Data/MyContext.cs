using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class MyContext : DbContext
    {
        
        public MyContext(DbContextOptions<MyContext> options) : base(options)
        {
            
        }
        public virtual DbSet<User> userModel { get; set; }
        public virtual DbSet<User2> userModel2 { get; set; }

    }
}
