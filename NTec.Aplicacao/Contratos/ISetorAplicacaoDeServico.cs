using NTec.Aplicacao.VizualizacoesDeModelo.Comum;
using NTec.Aplicacao.VizualizacoesDeModelo.Setores;
using NTec.Domain.Dtos.Setores;
using System.Threading.Tasks;

namespace NTec.Aplicacao.Contratos
{
    public interface ISetorAplicacaoDeServico
    {
        Task<RespostaSingularViewModel<string>> ObterSetorDetalhe(int id);

        Task<RespostaSimplesViewModel> Atualizar(SetorAtualizarViewModel modelo);
        Task<RespostaSimplesViewModel> Cadastrar(SetorCadastroViewModel modelo);
        Task<RespostaSimplesViewModel> Excluir(SetorExcluirViewModel modelo);
        Task<RespostaColecaoViewModel<object>> ObterDropDown();
        Task<RespostaPaginacaoViewModel<SetorPaginacaoDto>> ObterDadosParaPaginacao(SetorFiltroViewModel modelo);
    }
}
