using Fair2Share.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fair2Share.Data.Mappers {
    public class FriendsConfiguration : IEntityTypeConfiguration<Friends> {
        public void Configure(EntityTypeBuilder<Friends> builder) {
            builder.ToTable("Friends");
            builder.HasKey(p => new { p.ProfileId, p.FriendId });
            builder.Property(p => p.TimeStamp).IsRequired(true);
            builder.HasOne(x => x.Profile).WithMany(x => x.Friends).HasForeignKey(x => x.ProfileId).OnDelete(DeleteBehavior.Cascade); //.OnDelete(DeleteBehavior.SetNull);
            builder.HasOne(x => x.Friend).WithMany(x => x.FriendOf).HasForeignKey(x => x.FriendId).OnDelete(DeleteBehavior.Cascade);  //.OnDelete(DeleteBehavior.SetNull);
        }
    }
}
