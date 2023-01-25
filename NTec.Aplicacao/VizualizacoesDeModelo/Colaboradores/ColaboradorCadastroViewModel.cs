using Microsoft.AspNetCore.Http;
using NTec.Domain.Enums;
using System;
using System.Text.Json.Serialization;

namespace NTec.Aplicacao.VizualizacoesDeModelo.Colaboradores
{
    public class ColaboradorCadastroViewModel
    {
        public DateTime Aniversario { get; set; }
        public long Cpf { get; set; }
        public GeneroEnum Genero { get; set; }

        [JsonIgnore] //tempo para implementar
        public IFormFile Foto { get; set; }
        public string Nome { get; set; }    
        public string SobreNome { get; set; }

        public int CargoId { get; set; }
        public Guid? ChefeId { get; set; }
        public int SetorId { get; set; }
    }
}
