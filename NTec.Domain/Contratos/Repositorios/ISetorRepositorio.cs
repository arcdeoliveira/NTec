using NTec.Domain.Dtos.Comum;
using NTec.Domain.Dtos.Setores;
using NTec.Domain.Entidades;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NTec.Domain.Contratos.Repositorios
{
    public interface ISetorRepositorio : IBaseRepositorio<Setor>
    {
        Task<bool> SetorDuplicado(string nome, int? id = null);
        Task<bool> SetorExiste(int setorId);

        Task<IEnumerable<object>> ObterSetoresOrdenadoPorNome();
        Task<PaginacaoColecaoDto<SetorPaginacaoDto>> ObterSetoresPaginados(string nomeSetor, int skip, int take);
    }
}
