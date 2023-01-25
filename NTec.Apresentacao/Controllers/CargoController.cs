using Microsoft.AspNetCore.Mvc;
using NTec.Aplicacao.Contratos;
using NTec.Aplicacao.VizualizacoesDeModelo.Cargos;
using NTec.Aplicacao.VizualizacoesDeModelo.Comum;
using NTec.Domain.Dtos.Cargos;
using NTec.Helper.Aberto.Constantes;
using System.Net;

namespace NTec.Apresentacao.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CargoController : ControllerBase
    {
        private readonly ICargoAplicacaoDeServico _cargoAplicacao;

        public CargoController(ICargoAplicacaoDeServico cargoAplicacao)
        {
            _cargoAplicacao = cargoAplicacao;
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(object), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> ObterCargo([FromRoute] int id)
        {
            var resultado = await _cargoAplicacao.ObterCargoDetalhe(id);

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
        [ProducesResponseType(typeof(RespostaPaginacaoViewModel<CargoPaginacaoDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> ObterCargosParaPaginacao([FromQuery] CargoFiltroViewModel modelo)
        {
            if (modelo == null)
            {
                return BadRequest(Mensagem.ModeloInvalido);
            }

            var resultado = await _cargoAplicacao.ObterDadosParaPaginacao(modelo);

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
            var resultado = await _cargoAplicacao.ObterDropDown();

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
        public async Task<IActionResult> CadastrarCargo([FromBody] CargoCadastroViewModel modelo)
        {
            if (modelo == null)
            {
                return BadRequest(Mensagem.ModeloInvalido);
            }

            var resultado = await _cargoAplicacao.Cadastrar(modelo);

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
        public async Task<IActionResult> AtualizarCargo([FromBody] CargoAtualizarViewModel modelo)
        {
            if (modelo == null)
            {
                return BadRequest(Mensagem.ModeloInvalido);
            }

            var resultado = await _cargoAplicacao.Atualizar(modelo);

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
        public async Task<IActionResult> ExcluirCargo([FromBody] CargoExcluirViewModel modelo)
        {
            if (modelo == null)
            {
                return BadRequest(Mensagem.ModeloInvalido);
            }

            var resultado = await _cargoAplicacao.Excluir(modelo);

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
