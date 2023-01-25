using NTec.Domain.Dtos.Cargos;
using NTec.Domain.Dtos.Comum;
using NTec.Domain.Entidades;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NTec.Domain.Contratos.ServicosDeDominio
{
    public interface ICargoServicoDeDominio : IBaseServicoDeDominio<Cargo>
    {
        Task<bool> CargoDuplicado(string nome, int? id = null);

        Task<IEnumerable<object>> ObterDropDown(); 
        Task<PaginacaoColecaoDto<CargoPaginacaoDto>> ObterPaginacao(string cargoNome, int pagina, int quantidadePagina);
    }
}
