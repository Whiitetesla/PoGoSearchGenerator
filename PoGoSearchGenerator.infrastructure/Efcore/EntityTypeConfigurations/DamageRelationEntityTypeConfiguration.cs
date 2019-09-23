using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PoGoSearchGenerator.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PoGoSearchGenerator.infrastructure.Efcore.EntityTypeConfigurations
{
    public class DamageRelationEntityTypeConfiguration
        : IEntityTypeConfiguration<DamageRelation>
    {
        public void Configure(EntityTypeBuilder<DamageRelation> builder)
        {
            builder.HasKey(x => x.Id);

        }
    }


}
