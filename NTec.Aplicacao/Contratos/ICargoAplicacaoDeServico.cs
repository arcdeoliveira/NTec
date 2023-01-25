using NTec.Aplicacao.VizualizacoesDeModelo.Cargos;
using NTec.Aplicacao.VizualizacoesDeModelo.Comum;
using NTec.Domain.Dtos.Cargos;
using System.Threading.Tasks;

namespace NTec.Aplicacao.Contratos
{
    public interface ICargoAplicacaoDeServico
    {
        Task<RespostaSingularViewModel<string>> ObterCargoDetalhe(int id);
        Task<RespostaSimplesViewModel> Atualizar(CargoAtualizarViewModel modelo);
        Task<RespostaSimplesViewModel> Cadastrar(CargoCadastroViewModel modelo);
        Task<RespostaSimplesViewModel> Excluir(CargoExcluirViewModel modelo);
        Task<RespostaColecaoViewModel<object>> ObterDropDown();
        Task<RespostaPaginacaoViewModel<CargoPaginacaoDto>> ObterDadosParaPaginacao(CargoFiltroViewModel modelo);
    }
}
