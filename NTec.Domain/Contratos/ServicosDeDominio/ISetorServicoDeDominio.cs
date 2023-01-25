using NTec.Domain.Dtos.Comum;
using NTec.Domain.Dtos.Setores;
using NTec.Domain.Entidades;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NTec.Domain.Contratos.ServicosDeDominio
{
    public interface ISetorServicoDeDominio : IBaseServicoDeDominio<Setor>
    {
        Task<bool> SetorDuplicado(string nome, int? id = null);

        Task<PaginacaoColecaoDto<SetorPaginacaoDto>> ObterPaginacao(string nome, int pagina, int quantidadePagina);
        Task<IEnumerable<object>> ObterDrowDown();
    }
}
