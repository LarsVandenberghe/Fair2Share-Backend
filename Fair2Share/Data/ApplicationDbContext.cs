using Fair2Share.Data.Mappers;
using Fair2Share.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fair2Share.Data.Repositories {
    public class ApplicationDbContext : IdentityDbContext{
        public DbSet<Profile> Profiles { get; set; }
        //public DbSet<Friends> Friends { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options){

        }

        protected override void OnModelCreating(ModelBuilder builder) {
            base.OnModelCreating(builder);
            builder.ApplyConfiguration(new ProfileConfiguration());
          //  builder.ApplyConfiguration(new FriendsConfiguration());
        }
    }
}
