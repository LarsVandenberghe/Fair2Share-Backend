using Fair2Share.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fair2Share.Data.Mappers {
    public class TransactionConfiguration : IEntityTypeConfiguration<Transaction> {
        public void Configure(EntityTypeBuilder<Transaction> builder) {
            builder.ToTable("Transaction");
            builder.HasKey(p => p.TransactionId);
            builder.Property(p => p.Name).IsRequired(true).HasMaxLength(100);
            //builder.Property(p => p.PaidBy).IsRequired(true);
            builder.HasOne(p => p.PaidBy).WithMany().IsRequired(true);
        }
    }
}
