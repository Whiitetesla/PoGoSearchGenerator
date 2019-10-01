using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PoGoSearchGenerator.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PoGoSearchGenerator.infrastructure.Efcore.EntityTypeConfigurations
{
    public class TypeEntityTypeConfiguration
        : IEntityTypeConfiguration<Types>
    {
        public void Configure(EntityTypeBuilder<Types> builder)
        {
            builder.HasKey(x => x.Id);
        }
    }
}
