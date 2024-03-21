using Questao5.Domain.Entidades;
using Questao5.Domain.Servicos.Interfaces;
using Questao5.Domain.Validadores;
using Questao5.Infrastructure.Repositorios.Interfaces;

namespace Questao5.Domain.Servicos
{
    public class ContaCorrenteService : IContaCorrenteService
    {
        private readonly IMovimentoRepositorio _movimentoRepositorio;
        private readonly IContaCorrenteRepositorio _contaCorrenteRepositorio;
        private readonly ILogger<ContaCorrenteService> _logger;

        public ContaCorrenteService(IMovimentoRepositorio movimentoRepositorio, IContaCorrenteRepositorio contaCorrenteRepositorio,
                                    ILogger<ContaCorrenteService> logger) {
            _movimentoRepositorio = movimentoRepositorio;
            _contaCorrenteRepositorio = contaCorrenteRepositorio;
            _logger = logger;
        }

        public async Task<Movimento> RealizarMovimentacao(Movimento movimento) {
            
            var contaCorrente = await BuscarContaCorrente(movimento.NumeroContaCorrente);

            _logger.LogDebug("Validando conta corrente");
            var validador = new ContaCorrenteValidador().Validate(contaCorrente);

            if (validador.Errors.Any()) {
                return new Movimento() { ErrosDeValidacao = validador.Errors };
            }

            _logger.LogDebug("Atualizando valores da conta corrente do movimento");
            movimento.AtualizarValoresDaContaCorrente(contaCorrente);

            _logger.LogDebug("Adicionando o movimento");
            return await _movimentoRepositorio.AdicionarMovimento(movimento);
        }

        public async Task<ContaCorrente> BuscarContaCorrenteCompleta(int numeroConta) {
            var contaCorrente = await BuscarContaCorrente(numeroConta);

            _logger.LogDebug("Validando conta corrente");
            var validador = new ContaCorrenteValidador().Validate(contaCorrente);

            if (validador.Errors.Any()) {
                return new ContaCorrente() { ErrosDeValidacao = validador.Errors };
            }

            _logger.LogDebug("Buscando movimentos da conta corrente", contaCorrente.Id);
            var movimentosDaConta = await _movimentoRepositorio.BuscarMovimentosPorContaCorrente(contaCorrente.Id);

            contaCorrente.AtualizarMovimentos(movimentosDaConta.ToList());

            return contaCorrente;
        }

        private Task<ContaCorrente> BuscarContaCorrente(int numeroConta) {
            _logger.LogDebug("Buscando conta corrente pelo numero", numeroConta);

            return _contaCorrenteRepositorio.BuscarContaCorrente(numeroConta);
        }
    }
}
