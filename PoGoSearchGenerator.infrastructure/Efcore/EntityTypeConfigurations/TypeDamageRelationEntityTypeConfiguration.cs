using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PoGoSearchGenerator.Domain.Entities;

namespace PoGoSearchGenerator.infrastructure.Efcore.EntityTypeConfigurations
{
    public class TypeDamageRelationEntityTypeConfiguration
        : IEntityTypeConfiguration<TypeDamageRelation>
    {
        public void Configure(EntityTypeBuilder<TypeDamageRelation> builder)
        {
            builder.HasKey(x => x.Id);

            //builder.HasOne(x => x.DamageRelation).WithMany(x => x.Double_damage_from);
            //builder.HasOne(x => x.DamageRelation).WithMany(x => x.Double_damage_to);
            //builder.HasOne(x => x.DamageRelation).WithMany(x => x.Half_damage_from);
            //builder.HasOne(x => x.DamageRelation).WithMany(x => x.No_damage_from );
        }
    }
}
