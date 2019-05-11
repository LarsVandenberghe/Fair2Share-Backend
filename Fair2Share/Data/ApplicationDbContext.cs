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
        public DbSet<Activity> Activities { get; set; }
        //public DbSet<Transaction> Transactions { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options){

        }

        protected override void OnModelCreating(ModelBuilder builder) {
            base.OnModelCreating(builder);
            builder.ApplyConfiguration(new ActivitiesConfiguration());
            builder.ApplyConfiguration(new FriendsConfiguration());
            builder.ApplyConfiguration(new FriendRequestConfiguration());
            builder.ApplyConfiguration(new ProfileActivityIntersectionConfiguration());
            builder.ApplyConfiguration(new ProfileConfiguration());
            builder.ApplyConfiguration(new TransactionConfiguration());
            builder.ApplyConfiguration(new ProfileTransactionIntersectionConfiguration());
            builder.ApplyConfiguration(new ProfileImageConfiguration());
        }
    }
}
