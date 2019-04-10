using Fair2Share.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fair2Share.Data.Mappers {
    public class ProfileConfiguration : IEntityTypeConfiguration<Profile> {
        public void Configure(EntityTypeBuilder<Profile> builder) {
            builder.ToTable("Profile");
            builder.HasKey(p => p.ProfileId);
            builder.Property(p => p.Firstname).IsRequired().HasMaxLength(100);
            builder.Property(p => p.Lastname).IsRequired().HasMaxLength(100);
            builder.Property(p => p.Email).IsRequired().HasMaxLength(100);
            builder.Property(p => p.PathToImage).IsRequired(false).HasMaxLength(200);
            
        }
    }
}
