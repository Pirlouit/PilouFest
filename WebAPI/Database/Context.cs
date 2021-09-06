using Microsoft.EntityFrameworkCore;
using SharedLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Database
{
    public class Context : DbContext
    {
        public DbSet<PhotoItem> Photo { get; set; }

        public Context(DbContextOptions<Context> contextOptions) : base(contextOptions) { }


    }
}
