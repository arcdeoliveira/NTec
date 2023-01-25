using AutoMapper;
using NTec.Aplicacao.Contratos;
using NTec.Aplicacao.Properties;
using NTec.Aplicacao.VizualizacoesDeModelo.Comum;
using NTec.Aplicacao.VizualizacoesDeModelo.Setores;
using NTec.Domain.Contratos.ServicosDeDominio;
using NTec.Domain.Dtos.Setores;
using NTec.Domain.Entidades;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace NTec.Aplicacao.AplicacaoDeServicos
{
    public class SetorAplicacaoDeServico : ISetorAplicacaoDeServico
    {
        private readonly IColaboradorServicoDeDominio _colaboradorServico;
        private readonly IMapper                _mapper;
        private readonly ISetorServicoDeDominio _setorServico;

        public SetorAplicacaoDeServico(IColaboradorServicoDeDominio colaboradorServico, IMapper mapper, ISetorServicoDeDominio setorServico)
        {
            _colaboradorServico = colaboradorServico;
            _mapper             = mapper;
            _setorServico       = setorServico;
        }

        public async Task<RespostaPaginacaoViewModel<SetorPaginacaoDto>> ObterDadosParaPaginacao(SetorFiltroViewModel modelo)
        {
            try
            {
                var resultado = await _setorServico.ObterPaginacao(modelo.Nome, modelo.PaginaNumero, modelo.PaginaQuantidade);                

                return resultado.Total == 0 
                    ? new RespostaPaginacaoViewModel<SetorPaginacaoDto>(null, HttpStatusCode.NotFound, Resources.NaoEncontrado)
                    : new RespostaPaginacaoViewModel<SetorPaginacaoDto>(resultado.Objetos, HttpStatusCode.OK, null)                
                    {
                        Total     = resultado.Total,
                        Paginacao = modelo                
                    };
            }
            catch (Exception ex)
            {                
                return new RespostaPaginacaoViewModel<SetorPaginacaoDto>(null, HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        public async Task<RespostaColecaoViewModel<object>> ObterDropDown()
        {
            try
            {
                var setores = await _setorServico.ObterDrowDown();

                return new RespostaColecaoViewModel<object>(setores, HttpStatusCode.OK, string.Empty);
            }
            catch(Exception ex)
            {
                return new RespostaColecaoViewModel<object>(null, HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        public async Task<RespostaSimplesViewModel> Cadastrar(SetorCadastroViewModel modelo)
        {
            try
            {
                if (await _setorServico.SetorDuplicado(modelo.Nome))
                {
                    var duplicadoMensagem = string.Format(Resources.CadastroDuplicado, "setor", modelo.Nome);
                    return new RespostaSimplesViewModel(HttpStatusCode.BadRequest, duplicadoMensagem);
                }

                var setor = _mapper.Map<Setor>(modelo);

                _setorServico.Cadastrar(setor);
                
                var cadastrado = await _setorServico.Salvar();
                var codigo     = cadastrado ? HttpStatusCode.OK : HttpStatusCode.BadRequest;
                var mensagem   = cadastrado 
                    ? string.Format(Resources.CadastroSucesso, nameof(setor)) 
                    : string.Format(Resources.CadastroErro, nameof(setor));

                return new RespostaSimplesViewModel(codigo, mensagem);
            }
            catch(Exception ex)
            {
                return new RespostaSimplesViewModel(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        public async Task<RespostaSimplesViewModel> Atualizar(SetorAtualizarViewModel modelo)
        {
            try
            {                
                var setor = await _setorServico.ObterPorId(modelo.Id);
                if (setor == null || setor.Excluido)
                {
                    return new RespostaSimplesViewModel(HttpStatusCode.NotFound, Resources.SetorNaoEncontrado);
                }

                if (await _setorServico.SetorDuplicado(modelo.Nome, setor.Id))
                {
                    var duplicadoMensagem = string.Format(Resources.CadastroDuplicado, "setor", modelo.Nome);
                    return new RespostaSimplesViewModel(HttpStatusCode.BadRequest, duplicadoMensagem);
                }

                _mapper.Map(modelo, setor);
                _setorServico.Atualizar(setor);

                var atualizado = await _setorServico.Salvar();
                var codigo     = atualizado ? HttpStatusCode.NoContent : HttpStatusCode.BadRequest;                
                var mensagem   = atualizado ? null : string.Format(Resources.AtualizacaoErro, nameof(setor));

                return new RespostaSimplesViewModel(codigo, mensagem);
            }
            catch (Exception ex)
            {
                return new RespostaSimplesViewModel(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        public async Task<RespostaSimplesViewModel> Excluir(SetorExcluirViewModel modelo)
        {
            try
            {
                var setor = await _setorServico.ObterPorId(modelo.Id);
                if (setor == null || setor.Excluido)
                {
                    return new RespostaSimplesViewModel(HttpStatusCode.NotFound, Resources.SetorNaoEncontrado);
                }

                if (await _colaboradorServico.SetorCadastradoEmColaborador(setor.Id))
                {
                    return new RespostaSimplesViewModel(HttpStatusCode.Forbidden, Resources.RegraExclusaoSetor);
                }

                _mapper.Map(modelo, setor);
                _setorServico.Atualizar(setor);

                var deletado = await _setorServico.Salvar();
                var codigo   = deletado ? HttpStatusCode.NoContent : HttpStatusCode.BadRequest;
                var mensagem = deletado ? null : string.Format(Resources.ExclusaoErro, nameof(setor));

                return new RespostaSimplesViewModel(codigo, mensagem);
            }
            catch (Exception ex)
            {
                return new RespostaSimplesViewModel(HttpStatusCode.InternalServerError, ex.Message);
            }            
        }

        public async Task<RespostaSingularViewModel<string>> ObterSetorDetalhe(int id)
        {
            try
            {
                if (id == 0)
                {
                    return new RespostaSingularViewModel<string>(HttpStatusCode.BadRequest, Resources.IdObrigatorio);
                }

                var setor = await _setorServico.ObterPorId(id);

                return setor == null || setor.Excluido
                    ? new RespostaSingularViewModel<string>(HttpStatusCode.NotFound, Resources.SetorNaoEncontrado)
                    : new RespostaSingularViewModel<string>(HttpStatusCode.OK, null) { Objeto = setor.Nome };
            }
            catch (Exception ex)
            {
                return new RespostaSingularViewModel<string>(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}