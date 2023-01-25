using AutoMapper;
using NTec.Aplicacao.Contratos;
using NTec.Aplicacao.Properties;
using NTec.Aplicacao.VizualizacoesDeModelo.Cargos;
using NTec.Aplicacao.VizualizacoesDeModelo.Comum;
using NTec.Domain.Contratos.ServicosDeDominio;
using NTec.Domain.Dtos.Cargos;
using NTec.Domain.Dtos.Comum;
using NTec.Domain.Entidades;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace NTec.Aplicacao.AplicacaoDeServicos
{
    public class CargoAplicacaoDeServico : ICargoAplicacaoDeServico
    {
        private readonly ICargoServicoDeDominio _cargoServico;
        private readonly IColaboradorServicoDeDominio _colaboradorServico;
        private readonly IMapper _mapper;

        public CargoAplicacaoDeServico(ICargoServicoDeDominio cargoServico, IColaboradorServicoDeDominio colaboradorServico, IMapper mapper)
        {
            _cargoServico       = cargoServico;
            _colaboradorServico = colaboradorServico;
            _mapper             = mapper;
        }

        public async Task<RespostaSimplesViewModel> Atualizar(CargoAtualizarViewModel modelo)
        {
            try
            {
                var cargo = await _cargoServico.ObterPorId(modelo.Id);
                if (cargo == null || cargo.Excluido)
                {
                    return new RespostaSimplesViewModel(HttpStatusCode.NotFound, Resources.CargoNaoEncontrado);
                }

                if (await _cargoServico.CargoDuplicado(modelo.Nome, cargo.Id))
                {
                    var duplicadoMensagem = string.Format(Resources.CadastroDuplicado, nameof(cargo), modelo.Nome);
                    return new RespostaSimplesViewModel(HttpStatusCode.BadRequest, duplicadoMensagem);
                }

                _mapper.Map(modelo, cargo);
                _cargoServico.Atualizar(cargo);

                var atualizado = await _cargoServico.Salvar();
                var codigo     = atualizado ? HttpStatusCode.NoContent : HttpStatusCode.BadRequest;
                var mensagem   = atualizado ? null : string.Format(Resources.AtualizacaoErro, nameof(cargo));

                return new RespostaSimplesViewModel(codigo, mensagem);
            }
            catch (Exception ex)
            {
                return new RespostaSimplesViewModel(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        public async Task<RespostaSimplesViewModel> Cadastrar(CargoCadastroViewModel modelo)
        {
            try
            {
                if (await _cargoServico.CargoDuplicado(modelo.Nome)) 
                {
                    var duplicadoMensagem = string.Format(Resources.CadastroDuplicado, "cargo", modelo.Nome);
                    return new RespostaSimplesViewModel(HttpStatusCode.BadRequest, duplicadoMensagem);
                }

                var cargo = _mapper.Map<Cargo>(modelo);

                _cargoServico.Cadastrar(cargo);

                var cadastrado = await _cargoServico.Salvar();
                var codigo     = cadastrado ? HttpStatusCode.OK : HttpStatusCode.BadRequest;                
                var mensagem   = cadastrado 
                    ? string.Format(Resources.CadastroSucesso, nameof(cargo)) 
                    : string.Format(Resources.CadastroErro, nameof(cargo));

                return new RespostaSimplesViewModel(codigo, mensagem);
            }
            catch (Exception ex)
            {
                return new RespostaSimplesViewModel(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        public async Task<RespostaSimplesViewModel> Excluir(CargoExcluirViewModel modelo)
        {
            try
            {
                var cargo = await _cargoServico.ObterPorId(modelo.Id);
                if (cargo == null || cargo.Excluido)
                {
                    return new RespostaSimplesViewModel(HttpStatusCode.NotFound, Resources.CargoNaoEncontrado);
                }

                if (await _colaboradorServico.CargoCadastradoEmColaborador(cargo.Id))
                {
                    return new RespostaSimplesViewModel(HttpStatusCode.Forbidden, Resources.RegraExclusaoCargo);
                }

                _mapper.Map(modelo, cargo);
                _cargoServico.Atualizar(cargo);

                var deletado = await _cargoServico.Salvar();
                var codigo   = deletado ? HttpStatusCode.NoContent : HttpStatusCode.BadRequest;
                var mensagem = deletado ? null : string.Format(Resources.ExclusaoErro, nameof(cargo));

                return new RespostaSimplesViewModel(codigo, mensagem);
            }
            catch (Exception ex)
            {
                return new RespostaSimplesViewModel(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        public async Task<RespostaSingularViewModel<string>> ObterCargoDetalhe(int id)
        {
            try
            {
                if(id == 0)
                {
                    return new RespostaSingularViewModel<string>(HttpStatusCode.BadRequest, Resources.IdObrigatorio);
                }

                var cargo = await _cargoServico.ObterPorId(id);

                return cargo == null || cargo.Excluido
                    ? new RespostaSingularViewModel<string>(HttpStatusCode.NotFound, Resources.CargoNaoEncontrado)
                    : new RespostaSingularViewModel<string>(HttpStatusCode.OK, null) { Objeto = cargo.Nome };
            }
            catch (Exception ex)
            {
                return new RespostaSingularViewModel<string>(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        public async Task<RespostaPaginacaoViewModel<CargoPaginacaoDto>> ObterDadosParaPaginacao(CargoFiltroViewModel modelo)
        {
            try
            {
                var resultado = await _cargoServico.ObterPaginacao(modelo.Nome, modelo.PaginaNumero, modelo.PaginaQuantidade);

                return resultado.Total == 0 
                    ? new RespostaPaginacaoViewModel<CargoPaginacaoDto>(null, HttpStatusCode.NotFound, Resources.NaoEncontrado)
                    : new RespostaPaginacaoViewModel<CargoPaginacaoDto>(resultado.Objetos, HttpStatusCode.OK, null)
                    {
                        Total     = resultado.Total,
                        Paginacao = new PaginacaoDto(modelo.PaginaNumero, modelo.PaginaQuantidade)                
                    };
            }
            catch (Exception ex)
            {
                return new RespostaPaginacaoViewModel<CargoPaginacaoDto>(null, HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        public async Task<RespostaColecaoViewModel<object>> ObterDropDown()
        {
            try
            {
                var cargos = await _cargoServico.ObterDropDown();

                return new RespostaColecaoViewModel<object>(cargos, HttpStatusCode.OK, string.Empty);
            }
            catch (Exception ex)
            {
                return new RespostaColecaoViewModel<object>(null, HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}