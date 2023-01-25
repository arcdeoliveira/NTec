using Microsoft.EntityFrameworkCore;
using NTec.Domain.Entidades;
using NTec.Infra.Mapeamentos;

namespace NTec.Infra.Contexto
{
    public class DataContexto : DbContext
    {
        public DataContexto(DbContextOptions contextOptions) : base(contextOptions) { }

        public DbSet<Cargo> Cargos { get; set; }
        public DbSet<Colaborador> Colaboradors { get; set; }
        public DbSet<Setor> Setors { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CargoMapeamento());
            modelBuilder.ApplyConfiguration(new ColaboradorMapeamento());
            modelBuilder.ApplyConfiguration(new SetorMapeamento());

            base.OnModelCreating(modelBuilder);
        }
    }
}
