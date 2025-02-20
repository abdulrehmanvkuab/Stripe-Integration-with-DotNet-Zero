using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arch.Merchants;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Arch.EntityFrameworkCore.Mappings
{
    public class MerchantMap : IEntityTypeConfiguration<Merchant>
    {
        public void Configure(EntityTypeBuilder<Merchant> builder)
        {
            builder.ToTable("Merchants");
            builder.HasKey(m => m.Id);
            builder.Property(m => m.Name).IsRequired().HasMaxLength(250);
            builder.Property(m => m.Email).IsRequired();
            builder.Property(m => m.Description).HasMaxLength(500);
        }
    }
}
