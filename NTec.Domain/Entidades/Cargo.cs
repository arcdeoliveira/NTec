using NTec.Domain.Contratos.Entidades;
using System;
using System.Collections.Generic;

namespace NTec.Domain.Entidades
{
    public class Cargo : ILogResponsavel
    {
        //Propriedades
        public int Id { get; set; }
        public string AlteradoPor { get; set; }
        public DateTime? DataDeAtualizacao { get; set; }
        public DateTime DataDeCadastro { get; set; }
        public DateTime? DataDeExclusao { get; set; }
        public bool Excluido { get; set; }
        public string ExcluidoPor { get; set; }
        public string Nome { get; set; }

        //Relacionamentos
        public ICollection<Colaborador> Colaboradores { get; set;}
    }
}
