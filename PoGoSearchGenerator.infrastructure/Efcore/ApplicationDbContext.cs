using Microsoft.EntityFrameworkCore;
using PoGoSearchGenerator.infrastructure.Efcore.EntityTypeConfigurations;
using System;
using System.Collections.Generic;
using System.Text;

namespace PoGoSearchGenerator.infrastructure.Efcore
{
    public class ApplicationDbContext
     : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new TypeEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new DamageRelationEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new TypeDamageRelationEntityTypeConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
