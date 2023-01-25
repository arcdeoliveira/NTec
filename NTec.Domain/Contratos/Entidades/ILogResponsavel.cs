using System;

namespace NTec.Domain.Contratos.Entidades
{
    //Contrato para utilizar o conceito de responsabilidades de ação(log) e "soft delete"
    public interface ILogResponsavel
    {
        string AlteradoPor { get; set; }
        DateTime? DataDeAtualizacao { get; set; }
        DateTime DataDeCadastro { get; set; }
        DateTime? DataDeExclusao { get; set; }
        bool Excluido { get; set; }
        public string ExcluidoPor { get; set; }


    }
}
