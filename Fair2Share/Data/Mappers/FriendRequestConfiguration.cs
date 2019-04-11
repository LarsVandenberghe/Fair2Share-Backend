using Fair2Share.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fair2Share.Data.Mappers {
    public class FriendRequestConfiguration : IEntityTypeConfiguration<FriendRequests> {
        public void Configure(EntityTypeBuilder<FriendRequests> builder) {
            builder.ToTable("FriendRequest");
            builder.HasKey(p => new { p.UserId, p.FutureFriendId });
            builder.Property(p => p.TimeStamp).IsRequired(true);
            builder.Property(p => p.State).IsRequired(true);
            //Check cascading
            builder.HasOne(p => p.User).WithMany(p => p.SentFriendRequests).HasForeignKey(p => p.UserId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(p => p.FutureFriend).WithMany(p => p.ReceivedFriendRequests).HasForeignKey(p => p.FutureFriendId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
