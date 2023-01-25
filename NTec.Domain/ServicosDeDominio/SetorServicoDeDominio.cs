using NTec.Domain.Contratos.Repositorios;
using NTec.Domain.Contratos.ServicosDeDominio;
using NTec.Domain.Dtos.Comum;
using NTec.Domain.Dtos.Setores;
using NTec.Domain.Entidades;
using NTec.Helper.Aberto.Extensoes;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NTec.Domain.ServicosDeDominio
{
    public class SetorServicoDeDominio : BaseServicoDeDominio<Setor>, ISetorServicoDeDominio
    {
        private readonly ISetorRepositorio _setorRepositorio;

        public SetorServicoDeDominio(ISetorRepositorio setorRepositorio) : base(setorRepositorio) 
        {
            _setorRepositorio = setorRepositorio;
        }

        public async Task<PaginacaoColecaoDto<SetorPaginacaoDto>> ObterPaginacao(string nomeSetor, int pagina, int quantidadePagina)
        {
            int skip = pagina.Skip(quantidadePagina);

            return await _setorRepositorio.ObterSetoresPaginados(nomeSetor, skip, quantidadePagina);
        }

        public async Task<IEnumerable<object>> ObterDrowDown()
        {
            return await _setorRepositorio.ObterSetoresOrdenadoPorNome();
        }

        public async Task<bool> SetorDuplicado(string nome, int? id = null)
        {
            return await _setorRepositorio.SetorDuplicado(nome, id);
        }
    }
}
