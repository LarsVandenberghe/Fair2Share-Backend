using Fair2Share.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fair2Share.Data.Mappers {
    public class ProfileTransactionIntersectionConfiguration : IEntityTypeConfiguration<ProfileTransactionIntersection> {
        public void Configure(EntityTypeBuilder<ProfileTransactionIntersection> builder) {
            builder.ToTable("ProfileTransaction");
            builder.HasKey(p => new { p.ProfileId, p.TransactionId});
            builder.HasOne(b => b.Profile).WithMany(b => b.Transactions).HasForeignKey(b => b.ProfileId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(b => b.Transaction).WithMany(b => b.ProfilesInTransaction).HasForeignKey(b => b.TransactionId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
