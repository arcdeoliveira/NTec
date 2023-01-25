using NTec.Domain.Contratos.Repositorios;
using NTec.Domain.Contratos.ServicosDeDominio;
using NTec.Domain.Dtos.Cargos;
using NTec.Domain.Dtos.Comum;
using NTec.Domain.Entidades;
using NTec.Helper.Aberto.Extensoes;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NTec.Domain.ServicosDeDominio
{
    //classe de regras de negócio
    public class CargoServicoDeDominio : BaseServicoDeDominio<Cargo>, ICargoServicoDeDominio
    {
        private readonly ICargoRepositorio _cargoRepositorio;

        public CargoServicoDeDominio(ICargoRepositorio cargoRepositorio) : base(cargoRepositorio)
        {
            _cargoRepositorio = cargoRepositorio;
        }

        public async Task<PaginacaoColecaoDto<CargoPaginacaoDto>> ObterPaginacao(string cargoNome, int pagina, int quantidadePagina)
        {             
            int skip = pagina.Skip(quantidadePagina);

            return await _cargoRepositorio.ObterCargosPaginados(cargoNome, skip, quantidadePagina);
        }

        public async Task<IEnumerable<object>> ObterDropDown()
        {
            return await _cargoRepositorio.ObterCargosOrdenadoPorNome();
        }

        public async Task<bool> CargoDuplicado(string nome, int? id = null)
        {
            return await _cargoRepositorio.CargoDuplicado(nome, id);
        }
    }
}
