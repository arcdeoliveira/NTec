using AutoMapper;
using NTec.Aplicacao.Contratos;
using NTec.Aplicacao.Properties;
using NTec.Aplicacao.VizualizacoesDeModelo.Colaboradores;
using NTec.Aplicacao.VizualizacoesDeModelo.Comum;
using NTec.Domain.Contratos.ServicosDeDominio;
using NTec.Domain.Dtos.Colaboradores;
using NTec.Domain.Entidades;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace NTec.Aplicacao.AplicacaoDeServicos
{
    public class ColaboradorAplicacaoDeServico : IColaboradorAplicacaoServico
    {
        private readonly IColaboradorServicoDeDominio _colaboradorServico;
        private readonly IMapper _mapper;

        public ColaboradorAplicacaoDeServico(IColaboradorServicoDeDominio colaboradorServico, IMapper mapper)
        {
            _colaboradorServico = colaboradorServico;
            _mapper             = mapper;
        }

        public async Task<RespostaSimplesViewModel> Atualizar(ColaboradorAtualizarViewModel modelo)
        {
            try
            {
                var validacao = await _colaboradorServico.ValidarRelacionamentosColaborador(modelo.ChefeId, modelo.CargoId, modelo.SetorId);
                if (!string.IsNullOrEmpty(validacao))
                {
                    return new RespostaSimplesViewModel(HttpStatusCode.BadRequest, validacao);
                }

                var colaborador = await _colaboradorServico.ObterPorId(modelo.Id);
                if (colaborador == null || colaborador.Excluido)
                {
                    return new RespostaSimplesViewModel(HttpStatusCode.NotFound, Resources.ColaboradorNaoEncontrado);
                }

                if (await _colaboradorServico.ColaboradorDuplicado(modelo.Cpf, modelo.Id))
                {
                    var duplicadoMensagem = string.Format(Resources.CadastroDuplicado, nameof(colaborador), modelo.Nome);
                    return new RespostaSimplesViewModel(HttpStatusCode.BadRequest, duplicadoMensagem);
                }

                _mapper.Map(modelo, colaborador);
                _colaboradorServico.Atualizar(colaborador);

                var atualizado = await _colaboradorServico.Salvar();

                var codigo   = atualizado ? HttpStatusCode.NoContent : HttpStatusCode.BadRequest;
                var mensagem = atualizado ? null : string.Format(Resources.AtualizacaoErro, nameof(colaborador));

                return new RespostaSimplesViewModel(codigo, mensagem);
            }
            catch (Exception ex)
            {
                return new RespostaSimplesViewModel(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        public async Task<RespostaSimplesViewModel> Cadastrar( ColaboradorCadastroViewModel modelo, string caminhoRaiz)
        {
            try
            {
                if (await _colaboradorServico.ColaboradorDuplicado(modelo.Cpf))
                {
                    var duplicadoMensagem = string.Format(Resources.CadastroDuplicado, "colaborador", modelo.Nome);
                    return new RespostaSimplesViewModel(HttpStatusCode.BadRequest, duplicadoMensagem);
                }

                var validacao = await _colaboradorServico.ValidarRelacionamentosColaborador(modelo.ChefeId, modelo.CargoId, modelo.SetorId);
                if (!string.IsNullOrEmpty(validacao))
                {
                    return new RespostaSimplesViewModel(HttpStatusCode.BadRequest, validacao);
                }
                    
                var retorno = await _colaboradorServico.SalvarFoto(modelo.Foto, caminhoRaiz);
                if (retorno != null && !retorno.Contains(modelo.Foto.FileName))
                {
                    return new RespostaSimplesViewModel(HttpStatusCode.BadRequest, retorno);
                }

                var colaborador = _mapper.Map<Colaborador>(modelo);
                colaborador.Foto = retorno;

                _colaboradorServico.Cadastrar(colaborador);

                var cadastrado = await _colaboradorServico.Salvar();

                var codigo   = cadastrado ? HttpStatusCode.OK : HttpStatusCode.BadRequest;
                var mensagem = cadastrado 
                    ? string.Format(Resources.CadastroSucesso, nameof(colaborador)) 
                    : string.Format(Resources.CadastroErro, nameof(colaborador));

                return new RespostaSimplesViewModel(codigo, mensagem);
            }
            catch (Exception ex)
            {
                return new RespostaSimplesViewModel(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        public async Task<RespostaSimplesViewModel> Excluir(ColaboradorExcluirViewModel modelo, string caminhoRaiz)
        {
            try
            {
                var colaborador = await _colaboradorServico.ObterPorId(modelo.Id);
                if (colaborador == null || colaborador.Excluido)
                {
                    return new RespostaSimplesViewModel(HttpStatusCode.NotFound, Resources.ColaboradorNaoEncontrado);
                }

                var posicaoDeChefe = await _colaboradorServico.PossuiSubordinados(colaborador.Id);
                if (posicaoDeChefe)
                {
                    return new RespostaSimplesViewModel(HttpStatusCode.Forbidden, Resources.RegraExclusaoColaborador);
                }

                _mapper.Map(modelo, colaborador);
                _colaboradorServico.Atualizar(colaborador);

                var deletado = await _colaboradorServico.Salvar();
                if(deletado)
                {                       
                    _colaboradorServico.DeletarFoto(caminhoRaiz, colaborador.Foto);                    
                }
                    
                var codigo   = deletado ? HttpStatusCode.NoContent : HttpStatusCode.BadRequest;
                var mensagem = deletado
                    ? string.Format(Resources.ExclusaoSucesso, nameof(colaborador))
                    : string.Format(Resources.ExclusaoErro, nameof(colaborador));

                return new RespostaSimplesViewModel(codigo, mensagem);
            }
            catch (Exception ex)
            {
                return new RespostaSimplesViewModel(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        public async Task<RespostaSingularViewModel<ColaboradorDetalheViewModel>> ObterColaboradorDetalhe(Guid? id)
        {
            try
            {
                if (!id.HasValue)
                {
                    return new RespostaSingularViewModel<ColaboradorDetalheViewModel>(HttpStatusCode.BadRequest, Resources.IdObrigatorio);
                }

                var colaborador = await _colaboradorServico.ObterPorId(id);

                return colaborador == null || colaborador.Excluido
                    ? new RespostaSingularViewModel<ColaboradorDetalheViewModel>(HttpStatusCode.NotFound, Resources.ColaboradorNaoEncontrado)
                    : new RespostaSingularViewModel<ColaboradorDetalheViewModel>(HttpStatusCode.OK, null) 
                    { 
                        Objeto = _mapper.Map<ColaboradorDetalheViewModel>(colaborador)
                    };
            }
            catch (Exception ex)
            {
                return new RespostaSingularViewModel<ColaboradorDetalheViewModel>(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        public async Task<RespostaPaginacaoViewModel<ColaboradorPaginacaoDto>> ObterDadosParaPaginacao(ColaboradorFiltroViewModel modelo, 
            string caminhoRaiz)
        {
            try
            {
                var filtroDto = _mapper.Map<FiltroColaboradorPaginacaoDto>(modelo);
                var resultado = await _colaboradorServico.ObterPaginacao(filtroDto, caminhoRaiz);
                    
                return resultado.Total == 0 
                    ? new RespostaPaginacaoViewModel<ColaboradorPaginacaoDto>(null, HttpStatusCode.NotFound, Resources.NaoEncontrado)
                    : new RespostaPaginacaoViewModel<ColaboradorPaginacaoDto>(resultado.Objetos, HttpStatusCode.OK, null)
                    {
                        Paginacao = modelo,
                        Total     = resultado.Total
                    };
            }
            catch (Exception ex)
            {
                return new RespostaPaginacaoViewModel<ColaboradorPaginacaoDto>(null, HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        public async Task<RespostaColecaoViewModel<object>> ObterDropDown()
        {
            try
            {
                var colaboradores = await _colaboradorServico.ObterDropDown();

                return new RespostaColecaoViewModel<object>(colaboradores, HttpStatusCode.OK, string.Empty);
            }
            catch (Exception ex)
            {
                return new RespostaColecaoViewModel<object>(null, HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        public async Task<RespostaSingularViewModel<ColaboradorNodeDto>> ObterOrganograma()
        {
            try
            {
                var hierarquia = await _colaboradorServico.ObterHierarquia();

                return hierarquia == null
                    ? new RespostaSingularViewModel<ColaboradorNodeDto>(HttpStatusCode.NotFound, Resources.ColaboradorHierarquiaErro)
                    : new RespostaSingularViewModel<ColaboradorNodeDto>(HttpStatusCode.OK, null)
                    {
                        Objeto = hierarquia
                    };
             
            }
            catch(Exception ex)
            {
                return new RespostaSingularViewModel<ColaboradorNodeDto>(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
