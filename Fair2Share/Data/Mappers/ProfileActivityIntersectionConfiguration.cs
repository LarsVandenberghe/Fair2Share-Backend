using Fair2Share.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fair2Share.Data.Mappers {
    public class ProfileActivityIntersectionConfiguration : IEntityTypeConfiguration<ProfileActivityIntersection> {
        public void Configure(EntityTypeBuilder<ProfileActivityIntersection> builder) {
            builder.ToTable("ProfileActivity");
            builder.HasKey(b => new { b.ProfileId, b.ActivityId });
            //check cascading
            builder.HasOne(b => b.Profile).WithMany(b => b.Activities).HasForeignKey(b => b.ProfileId);
            builder.HasOne(b => b.Activity).WithMany(b => b.Participants).HasForeignKey(b => b.ActivityId);
        }
    }
}
