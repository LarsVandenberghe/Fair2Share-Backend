using Fair2Share.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fair2Share.Data.Mappers {
    public class ActivitiesConfiguration : IEntityTypeConfiguration<Activity> {
        public void Configure(EntityTypeBuilder<Activity> builder) {
            builder.ToTable("Activities");
            builder.HasKey(p => p.ActivityId);
            builder.Property(p => p.Name).IsRequired(true).HasMaxLength(100);
            builder.HasMany(b => b.Transactions).WithOne().IsRequired(true);
        }
    }
}
