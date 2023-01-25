using NTec.Aplicacao.VizualizacoesDeModelo.Colaboradores;
using NTec.Aplicacao.VizualizacoesDeModelo.Comum;
using NTec.Domain.Dtos.Colaboradores;
using System;
using System.Threading.Tasks;

namespace NTec.Aplicacao.Contratos
{
    public interface IColaboradorAplicacaoServico
    {
        Task<RespostaSingularViewModel<ColaboradorNodeDto>> ObterOrganograma();
        Task<RespostaSingularViewModel<ColaboradorDetalheViewModel>> ObterColaboradorDetalhe(Guid? id);
        Task<RespostaSimplesViewModel> Atualizar(ColaboradorAtualizarViewModel modelo);
        Task<RespostaSimplesViewModel> Cadastrar(ColaboradorCadastroViewModel modelo, string caminhoRaiz);
        Task<RespostaSimplesViewModel> Excluir(ColaboradorExcluirViewModel modelo, string caminhoRaiz);
        Task<RespostaColecaoViewModel<object>> ObterDropDown();
        Task<RespostaPaginacaoViewModel<ColaboradorPaginacaoDto>> ObterDadosParaPaginacao(ColaboradorFiltroViewModel modelo, string caminhoRaiz);
    }
}
