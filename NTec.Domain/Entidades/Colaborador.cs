using NTec.Domain.Contratos.Entidades;
using NTec.Domain.Enums;
using System;
using System.Collections.Generic;

namespace NTec.Domain.Entidades
{
    public class Colaborador : ILogResponsavel
    {
        //Propriedades
        public Guid Id { get; set; }
        public string AlteradoPor { get; set; }
        public DateTime Aniversario { get; set; }
        public long Cpf { get; set; }
        public DateTime? DataDeAtualizacao { get; set; }
        public DateTime DataDeCadastro { get; set; }
        public DateTime? DataDeExclusao { get; set; }
        public bool Excluido { get; set; }
        public string ExcluidoPor { get; set; }
        public string Foto { get; set; }
        public GeneroEnum Genero { get; set; }
        public string Nome { get; set; }
        public string SobreNome { get; set;}

        //Chaves estrangeiras
        public int CargoId { get; set; }
        public Guid? ChefeId { get; set; }
        public int SetorId { get; set; }   

        //Relacionamentos
        public Cargo Cargo { get; set;}
        public Colaborador Chefe { get; set; }
        public Setor Setor { get; set; }

        public ICollection<Colaborador> Subordinados { get; set; }
    }
}
