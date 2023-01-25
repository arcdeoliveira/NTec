using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NTec.Domain.Entidades;
using NTec.Domain.Enums;
using System;

namespace NTec.Infra.Mapeamentos
{
    public class ColaboradorMapeamento : IEntityTypeConfiguration<Colaborador>
    {
        public void Configure(EntityTypeBuilder<Colaborador> builder)
        {
            //Mapeamento nome de tabela e chave primária
            builder.HasKey(h => h.Id);
            builder.ToTable("colaboradores");

            //Mapeamento personalizado das propriedades com os campos da tabela.
            builder.Property(p => p.Id)
                .IsRequired()
                .HasColumnOrder(1)
                .ValueGeneratedOnAdd();

            builder.Property(p => p.AlteradoPor)
                .IsRequired(false)
                .HasDefaultValue(null)
                .HasMaxLength(200)
                .HasColumnOrder(7);

            builder.Property(p => p.Aniversario)
               .IsRequired()
               .HasColumnType("date");

            builder.Property(p => p.Cpf)
               .IsRequired()                              
               .IsFixedLength(true)
               .HasMaxLength(11)
               .HasColumnOrder(2);

            builder.Property(p => p.DataDeAtualizacao)
               .IsRequired(false)
               .HasDefaultValue(null)
               .HasColumnOrder(8);

            builder.Property(p => p.DataDeCadastro)
               .IsRequired(true)
               .HasDefaultValue(DateTime.Now);

            builder.Property(p => p.DataDeExclusao)
               .IsRequired(false)
               .HasDefaultValue(null)
               .HasColumnOrder(6);

            builder.Property(p => p.Excluido)
               .IsRequired()
               .HasDefaultValue(false)
               .HasColumnOrder(4);

            builder.Property(p => p.ExcluidoPor)
               .IsRequired(false)
               .HasDefaultValue(null)
               .HasMaxLength(200)
               .HasColumnOrder(5);

            builder.Property(p => p.Foto)
                .IsRequired(false)
                .HasDefaultValue(null)
                .HasMaxLength(50);

            builder.Property(p => p.Genero)
                .IsRequired(true)
                .HasDefaultValue(GeneroEnum.NaoInformar) 
                .HasColumnOrder(3);

            builder.Property(p => p.Nome)
              .IsRequired(true)
              .HasMaxLength(50);

            builder.Property(p => p.SobreNome)
              .IsRequired(true)
              .HasMaxLength(150);


            //Mapeamento dos relacionamentos com as entidades
            builder
                .HasOne(h => h.Setor)
                .WithMany(w => w.Colaboradores)
                .HasForeignKey(h => h.SetorId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder
               .HasOne(h => h.Cargo)
               .WithMany(w => w.Colaboradores)
               .HasForeignKey(h => h.CargoId)
               .IsRequired()
               .OnDelete(DeleteBehavior.Restrict);

            builder
               .HasOne(h => h.Chefe)
               .WithMany(w => w.Subordinados)
               .HasForeignKey(h =>h.ChefeId)
               .IsRequired(false)
               .OnDelete(DeleteBehavior.NoAction);
        }
    }
}