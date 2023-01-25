using Microsoft.AspNetCore.Mvc;
using NTec.Aplicacao.Contratos;
using NTec.Aplicacao.VizualizacoesDeModelo.Comum;
using NTec.Aplicacao.VizualizacoesDeModelo.Setores;
using NTec.Domain.Dtos.Setores;
using NTec.Helper.Aberto.Constantes;
using System.Net;

namespace NTec.Apresentacao.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SetorController : ControllerBase
    {
        private readonly ISetorAplicacaoDeServico _setorAplicacao;

        public SetorController(ISetorAplicacaoDeServico setorAplicacao)
        {
            _setorAplicacao = setorAplicacao;
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(object), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> ObterSetor([FromRoute] int id)
        {
            var resultado = await _setorAplicacao.ObterSetorDetalhe(id);

            return resultado.Codigo switch
            {
                HttpStatusCode.BadRequest => BadRequest(resultado.Mensagem),
                HttpStatusCode.NotFound   => NotFound(resultado.Mensagem),
                HttpStatusCode.OK         => Ok(new { resultado.Objeto }),
                _                         => StatusCode(500, resultado.Mensagem),
            };
        }

        [HttpGet]
        [Route("paginacao")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(RespostaPaginacaoViewModel<SetorPaginacaoDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> ObterSetoresParaPaginacao([FromQuery] SetorFiltroViewModel modelo)
        {
            if(modelo == null)
            {
                return BadRequest(Mensagem.ModeloInvalido);
            }

            var resultado = await _setorAplicacao.ObterDadosParaPaginacao(modelo);

            return resultado.Codigo switch
            {
                HttpStatusCode.OK       => Ok(resultado),
                HttpStatusCode.NotFound => NotFound(resultado.Mensagem),
                _                       => StatusCode(500, resultado.Mensagem),
            };
        }

        [HttpGet]
        [Route("dropdown")]
        [ProducesResponseType(typeof(IEnumerable<object>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> ObterDadosParaSelecao()
        {
            var resultado = await _setorAplicacao.ObterDropDown();

            return resultado.Codigo switch
            {
                HttpStatusCode.OK => Ok(resultado.Objetos),
                _                 => StatusCode(500, resultado.Mensagem)
            };
        }


        [HttpPost]
        [Route("registrar")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> CadastrarSetor([FromBody] SetorCadastroViewModel model)
        {
            if(model == null)
            {
                return BadRequest(Mensagem.ModeloInvalido);
            }

            var resultado = await _setorAplicacao.Cadastrar(model);

            return resultado.Codigo switch
            {
                HttpStatusCode.BadRequest => BadRequest(resultado.Mensagem),
                HttpStatusCode.OK         => Ok(new { resultado.Mensagem }),
                _                         => StatusCode(500, resultado.Mensagem),
            };
        }

        [HttpPut]
        [Route("editar")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> AtualizarSetor([FromBody] SetorAtualizarViewModel model)
        {
            if (model == null)
            {
                return BadRequest(Mensagem.ModeloInvalido);
            }

            var resultado = await _setorAplicacao.Atualizar(model);

            return resultado.Codigo switch
            {
                HttpStatusCode.BadRequest => BadRequest(resultado.Mensagem),
                HttpStatusCode.NoContent  => NoContent(),
                HttpStatusCode.NotFound   => NotFound(resultado.Mensagem),
                _                         => StatusCode(500, resultado.Mensagem),
            };
        }

        [HttpDelete]
        [Route("deletar")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Forbidden)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> ExcluirSetor([FromBody] SetorExcluirViewModel modelo)
        {
            if (modelo == null)
            {
                return BadRequest(Mensagem.ModeloInvalido);
            }

            var resultado = await _setorAplicacao.Excluir(modelo);

            return resultado.Codigo switch
            {
                HttpStatusCode.BadRequest => BadRequest(resultado.Mensagem),
                HttpStatusCode.Forbidden  => StatusCode(403, resultado.Mensagem),
                HttpStatusCode.NoContent  => NoContent(),
                HttpStatusCode.NotFound   => NotFound(resultado.Mensagem),
                _                         => StatusCode(500, resultado.Mensagem)
            };
        }
    }
}
