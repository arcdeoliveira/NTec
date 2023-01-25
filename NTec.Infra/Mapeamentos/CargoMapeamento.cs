using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NTec.Domain.Entidades;
using System;

namespace NTec.Infra.Mapeamentos
{
    public class CargoMapeamento : IEntityTypeConfiguration<Cargo>
    {
        public void Configure(EntityTypeBuilder<Cargo> builder)
        {
            //Mapeamento nome de tabela e chave primária
            builder.HasKey(h => h.Id);
            builder.ToTable("cargos");

            //Mapeamento personalizado das propriedades com os campos da tabela.
            builder.Property(p => p.Id)
                .IsRequired()
                .HasColumnOrder(1)
                .ValueGeneratedOnAdd();

            builder.Property(p => p.AlteradoPor)
                .IsRequired(false)
                .HasDefaultValue(null)
                .HasMaxLength(200);

            builder.Property(p => p.DataDeAtualizacao)
               .IsRequired(false)
               .HasDefaultValue(null);

            builder.Property(p => p.DataDeCadastro)
               .IsRequired(true)
               .HasDefaultValue(DateTime.Now);

            builder.Property(p => p.DataDeExclusao)
               .IsRequired(false)
               .HasDefaultValue(null);

            builder.Property(p => p.Excluido)
               .IsRequired()
               .HasDefaultValue(false);

            builder.Property(p => p.ExcluidoPor)
               .IsRequired(false)
               .HasDefaultValue(null)
               .HasMaxLength(200);

            builder.Property(p => p.Nome)
              .IsRequired(true)
              .HasMaxLength(100)
              .HasColumnOrder(2);
        }
    }
}
