using NTec.Domain.Enums;

namespace NTec.Domain.Dtos.Colaboradores
{
    public class FiltroColaboradorPaginacaoDto
    {
        public string Nome { get; set; }
        public int? Cpf { get; set; }
        public GeneroEnum? Genero { get; set; }

        public int? CargoId { get; set; }
        public bool? Chefe { get; set; }
        public int? SetorId { get; set; }

        public int PaginaNumero { get; set; }
        public int PaginaQuantidade { get; set; }
    }
}
