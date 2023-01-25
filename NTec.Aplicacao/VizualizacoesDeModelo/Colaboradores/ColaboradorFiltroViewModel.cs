using NTec.Domain.Dtos.Comum;
using NTec.Domain.Enums;

namespace NTec.Aplicacao.VizualizacoesDeModelo.Colaboradores
{
    public class ColaboradorFiltroViewModel : PaginacaoDto
    {
        public string Nome { get; set; }
        public int? Cpf { get; set; }
        public GeneroEnum? Genero { get; set; }

        public int? CargoId { get; set; }
        public bool? Chefe { get; set; }
        public int? SetorId { get; set; }
    }
}
