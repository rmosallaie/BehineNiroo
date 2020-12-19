using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BehineNiroo.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BehineNiroo.Data
{
    public class BehineNirooContext : IdentityDbContext<IdentityUser>
    {
        public BehineNirooContext(DbContextOptions<BehineNirooContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            //builder.Entity<Event>().HasNoKey();
        }

        //public DbSet<Event> Event { get; set; }
    }
}
