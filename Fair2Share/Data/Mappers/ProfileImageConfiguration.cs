using Fair2Share.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fair2Share.Data.Mappers {
    public class ProfileImageConfiguration : IEntityTypeConfiguration<ProfileImage> {

        public void Configure(EntityTypeBuilder<ProfileImage> builder) {
            builder.ToTable("ProfileImage");
            builder.HasKey(p => p.ProfileId);
            builder.HasOne(p => p.Profile).WithOne(i => i.ProfileImage);
            builder.Property(p => p.Image).HasColumnType("varbinary(max)");
        }
        
    }
}
