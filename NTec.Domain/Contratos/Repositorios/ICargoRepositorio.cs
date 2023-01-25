using NTec.Domain.Dtos.Cargos;
using NTec.Domain.Dtos.Comum;
using NTec.Domain.Entidades;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NTec.Domain.Contratos.Repositorios
{
    public interface ICargoRepositorio : IBaseRepositorio<Cargo>
    {
        Task<bool> CargoExiste(int cargoId);
        Task<bool> CargoDuplicado(string nome, int? id = null);
        Task<IEnumerable<object>> ObterCargosOrdenadoPorNome();
        Task<PaginacaoColecaoDto<CargoPaginacaoDto>> ObterCargosPaginados(string nomeCargo, int skip, int take);
    }
}
