using Microsoft.AspNetCore.Mvc;
using NTec.Aplicacao.Contratos;
using NTec.Aplicacao.VizualizacoesDeModelo.Colaboradores;
using NTec.Aplicacao.VizualizacoesDeModelo.Comum;
using NTec.Domain.Dtos.Colaboradores;
using NTec.Helper.Aberto.Constantes;
using System.Net;

namespace NTec.Apresentacao.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ColaboradorController : ControllerBase
    {
        private readonly IColaboradorAplicacaoServico _colaboradorAplicacao;
        private readonly IWebHostEnvironment          _environment;

        public ColaboradorController(IColaboradorAplicacaoServico colaboradorAplicacao, IWebHostEnvironment environment)
        {
            _colaboradorAplicacao = colaboradorAplicacao;
            _environment          = environment;
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ColaboradorDetalheViewModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> ObterCargo([FromRoute] Guid? id)
        {
            var resultado = await _colaboradorAplicacao.ObterColaboradorDetalhe(id);

            return resultado.Codigo switch
            {
                HttpStatusCode.BadRequest => BadRequest(resultado.Mensagem),
                HttpStatusCode.NotFound   => NotFound(resultado.Mensagem),
                HttpStatusCode.OK         => Ok(resultado.Objeto),
                _                         => StatusCode(500, resultado.Mensagem),
            };
        }

        [HttpGet]
        [Route("paginacao")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(RespostaPaginacaoViewModel<ColaboradorPaginacaoDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> ObterColaboradoresParaPaginacao([FromQuery] ColaboradorFiltroViewModel modelo)
        {
            if (modelo == null)
            {
                return BadRequest(Mensagem.ModeloInvalido);
            }

            var resultado = await _colaboradorAplicacao.ObterDadosParaPaginacao(modelo, _environment.WebRootPath);

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
        public async Task<IActionResult> ObterDadosParaDropDown()
        {
            var resultado = await _colaboradorAplicacao.ObterDropDown();

            return resultado.Codigo switch
            {
                HttpStatusCode.OK => Ok(resultado.Objetos),
                _                 => StatusCode(500, resultado.Mensagem)
            };
        }

        [HttpGet]
        [Route("organograma")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(RespostaSingularViewModel<ColaboradorNodeDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> ObterOrganogramaColaboradores()
        {
            var resultado = await _colaboradorAplicacao.ObterOrganograma();

            return resultado.Codigo switch
            {
                HttpStatusCode.NotFound => NotFound(resultado.Mensagem),
                HttpStatusCode.OK       => Ok(resultado.Objeto),
                _                       => StatusCode(500, resultado.Mensagem)
            };
        }


        [HttpPost]
        [Route("registrar")]
        [ProducesResponseType(typeof(string), (int) HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> CadastrarColaborador([FromBody] ColaboradorCadastroViewModel modelo)
        {
            if (modelo == null)
            {
                return BadRequest(Mensagem.ModeloInvalido);
            }

            var resultado = await _colaboradorAplicacao.Cadastrar(modelo, caminhoRaiz: _environment.WebRootPath);

            return resultado.Codigo switch
            {
                HttpStatusCode.BadRequest => BadRequest(resultado.Mensagem),
                HttpStatusCode.OK => Ok(new { resultado.Mensagem }),
                _ => StatusCode(500, resultado.Mensagem),
            };
        }

        [HttpPut]
        [Route("editar")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]

        public async Task<IActionResult> AtualizarColaborador(ColaboradorAtualizarViewModel modelo)
        {
            if (modelo == null)
            {
                return BadRequest(Mensagem.ModeloInvalido);
            }

            var resultado = await _colaboradorAplicacao.Atualizar(modelo);

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
        public async Task<IActionResult> ExcluirColaborador(ColaboradorExcluirViewModel modelo)
        {
            if (modelo == null)
            {
                return BadRequest(Mensagem.ModeloInvalido);
            }

            var resultado = await _colaboradorAplicacao.Excluir(modelo, _environment.WebRootPath);

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
