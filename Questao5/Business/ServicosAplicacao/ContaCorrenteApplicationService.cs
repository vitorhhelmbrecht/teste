using Newtonsoft.Json;
using Questao5.Business.Abstracoes.ContaCorrente;
using Questao5.Business.Abstracoes.Saldo;
using Questao5.Business.Abstracoes.Validadores.ContaCorrente;
using Questao5.Business.ServicosAplicacao.Interfaces;
using Questao5.Domain.Entidades;
using Questao5.Domain.Servicos.Interfaces;

namespace Questao5.Business.ServicosAplicacao
{
    public class ContaCorrenteApplicationService : IContaCorrenteApplicationService
    {
        private readonly IContaCorrenteService _contaCorrenteService;
        private readonly IIdEmPotenciaApplicationService _idEmPotenciaApplicationService;
        private readonly ILogger<ContaCorrenteApplicationService> _logger;

        public ContaCorrenteApplicationService(IContaCorrenteService contaCorrenteService, IIdEmPotenciaApplicationService idEmPotenciaApplicationService,
                                               ILogger<ContaCorrenteApplicationService> logger) {
            _contaCorrenteService = contaCorrenteService;
            _idEmPotenciaApplicationService = idEmPotenciaApplicationService;
            _logger = logger;
        }

        public async Task<RespostaMovimento> RealizarMovimentacao(MovimentoDto movimentoDto) {
            var validador = new MovimentoValidador().Validate(movimentoDto);

            if (validador.Errors.Any()) {
                return new RespostaMovimento() { ErrosDeValidacao = validador.Errors };
            }

            _logger.LogDebug("Resgatando resposta anterior de um id em potência");
            var respostaPassada = await _idEmPotenciaApplicationService.BuscarRespostaPassada(movimentoDto.IdRequisicao);
            if (respostaPassada != null) {
                _logger.LogDebug("Resposta retornada do id em potência");
                return respostaPassada;
            }

            var movimento = new Movimento(movimentoDto.NumeroContaCorrente, movimentoDto.Tipo, movimentoDto.Valor);

            _logger.LogDebug("Id em potência não encontrado. Realizando a movimentação");
            var movimentoGerado = await _contaCorrenteService.RealizarMovimentacao(movimento);

            if (movimentoGerado.ErrosDeValidacao.Any()) {
                return new RespostaMovimento() { ErrosDeValidacao = movimentoGerado.ErrosDeValidacao };
            }

            var resposta = new RespostaMovimento() {
                Id = movimentoGerado.Id
            };

            _logger.LogDebug("Salvando resposta no id em potência");
            await _idEmPotenciaApplicationService.SalvarRespostaComoIdEmPotencial(movimentoDto, resposta);

            return resposta;
        }

        public async Task<RespostaSaldo> BuscarSaldo(int numeroConta) {
            _logger.LogDebug("Buscando conta corrente");
            var contaCorrente = await _contaCorrenteService.BuscarContaCorrenteCompleta(numeroConta);

            if (contaCorrente.ErrosDeValidacao.Any()) {
                return new RespostaSaldo() { ErrosDeValidacao = contaCorrente.ErrosDeValidacao };
            }

            _logger.LogDebug("Calculando saldo");
            return new RespostaSaldo() {
                DataResposta = DateTime.UtcNow,
                NomeTitularConta = contaCorrente.Titular,
                NumeroContaCorrente = contaCorrente.Numero,
                SaldoAtual = contaCorrente.CalcularSaldo(),
            };
        }
    }
}
