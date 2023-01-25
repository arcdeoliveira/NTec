using NTec.Domain.Enums;
using System;

namespace NTec.Domain.Dtos.Colaboradores
{
    public class ColaboradorPaginacaoDto
    {
        public Guid Id { get; set; }
        public string PosicaoChefe { get; set; }
        public long Cpf { get; set; }
        public string Data { get; set; }
        public string FotoCaminho { get;set; }
        public string Genero { get; set; }
        public string CargoNome { get; set; }
        public string NomeCompleto { get; set; }
        public string SetorNome { get; set; }
    }
}
