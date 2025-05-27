using CyberdyneBankAtm.Domain.Accounts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CyberdyneBankAtm.Domain.Transactions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CyberdyneBankAtm.Infrastructure.Transactions
{
    internal sealed class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Id)
                .ValueGeneratedOnAdd();
            builder.Property(t => t.Amount).IsRequired();
            builder.Property(t => t.Amount).HasColumnType("REAL");
            builder.Property(t => t.Description).HasMaxLength(255);
            builder.Property(t => t.CreatedOn).IsRequired();
            builder.Property(t => t.CreatedOn).HasColumnType("TEXT");

            builder.Property(t => t.TransactionType)
                .IsRequired()
                .HasConversion<int>(); // If enum

            builder.HasOne(t => t.Account)
                .WithMany(a => a.Transactions)
                .HasForeignKey(t => t.AccountId);

            // Optional: If you want a relationship for RelatedAccountId
            builder.HasOne<Account>()
                .WithMany()
                .HasForeignKey(t => t.RelatedAccountId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
