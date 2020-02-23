using System;
using System.Collections.Generic;
using System.Text;
using GiveMe.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GiveMe.Data
{
    public class IRepository : IdentityDbContext<ApplicationUser>
    {
        public IRepository(DbContextOptions<IRepository> options)
            : base(options)
        {
        }

        public DbSet<Project> Posts { get; set; }

    }
}
