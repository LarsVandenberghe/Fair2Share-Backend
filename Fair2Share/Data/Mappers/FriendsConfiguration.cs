﻿using Fair2Share.Models;
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
            builder.Property(p => p.Friend).IsRequired(true);
            builder.Property(p => p.Profile).IsRequired(true);

            builder.HasOne(x => x.Profile).WithMany().HasForeignKey(x => x.ProfileId); //.OnDelete(DeleteBehavior.SetNull);
            builder.HasOne(x => x.Friend).WithMany().HasForeignKey(x => x.FriendId); //.OnDelete(DeleteBehavior.SetNull);
        }
    }
}
