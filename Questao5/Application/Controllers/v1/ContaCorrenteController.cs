using Microsoft.AspNetCore.Mvc;
using Questao5.Business.Abstracoes.ContaCorrente;
using Questao5.Business.ServicosAplicacao.Interfaces;

namespace Questao5.Application.Controllers.v1
{
    [ApiController]
    [ApiVersion("1")]
    [Route("conta-corrente")]
    public class ContaCorrenteController : ControllerBase
    {
        private readonly IContaCorrenteApplicationService _contaCorrenteApplicationService;
        private readonly ILogger<ContaCorrenteController> _logger;

        public ContaCorrenteController(IContaCorrenteApplicationService contaCorrenteApplicationService,
                                       ILogger<ContaCorrenteController> logger) {
            _contaCorrenteApplicationService = contaCorrenteApplicationService;
            _logger = logger;
        }

        [HttpPost("movimento")]
        public async Task<ActionResult<HttpResponse>> RealizarMovimento([FromBody] MovimentoDto movimentoDto) {
            _logger.LogDebug("Processando RealizarMovimento");
            var respostaMovimento = await _contaCorrenteApplicationService.RealizarMovimentacao(movimentoDto);

            if (respostaMovimento.ErrosDeValidacao.Any()) {
                return BadRequest(respostaMovimento.ErrosDeValidacao);
            }

            return Ok(respostaMovimento);
        }

        [HttpGet("saldo")]
        public async Task<ActionResult<HttpResponse>> CalcularSaldo([FromQuery] int numeroConta) {
            _logger.LogDebug("Processando CalcularSaldo");
            var respostaSaldo = await _contaCorrenteApplicationService.BuscarSaldo(numeroConta);

            if (respostaSaldo.ErrosDeValidacao.Any()) {
                return BadRequest(respostaSaldo.ErrosDeValidacao);
            }

            return Ok(respostaSaldo);
        }
    }
}
