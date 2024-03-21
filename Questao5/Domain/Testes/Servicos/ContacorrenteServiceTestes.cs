using Questao5.Domain.Servicos;
using Questao5.Infrastructure.Repositorios.Interfaces;
using NSubstitute;
using Questao5.Domain.Entidades;
using Xunit;
using FluentAssertions;

namespace Questao5.Domain.Testes.Servicos
{
    public class ContacorrenteServiceTestes
    {
        private readonly IMovimentoRepositorio _movimentoRepositorio;
        private readonly IContaCorrenteRepositorio _contaCorrenteRepositorio;
        private readonly ILogger<ContaCorrenteService> _logger;
        private readonly ContaCorrenteService _contaCorrenteService;

        public ContacorrenteServiceTestes() {
            _movimentoRepositorio = Substitute.For<IMovimentoRepositorio>();
            _contaCorrenteRepositorio = Substitute.For<IContaCorrenteRepositorio>();
            _logger = Substitute.For<ILogger<ContaCorrenteService>>();

            _contaCorrenteService = new(_movimentoRepositorio, _contaCorrenteRepositorio, _logger);
        }

        [Fact]
        public async Task DeveriaRalizarMovimentacao() {
            int numeroConta = 123;
            string idConta = "testing";

            Movimento movimentoFake = new(numeroConta, "D", 100);

            ContaCorrente contaCorrenteFake = new(idConta, numeroConta, 1);

            _contaCorrenteRepositorio.BuscarContaCorrente(numeroConta).Returns(Task.FromResult(contaCorrenteFake));
            _movimentoRepositorio.AdicionarMovimento(movimentoFake).Returns(Task.FromResult(movimentoFake));

            var resultado = await _contaCorrenteService.RealizarMovimentacao(movimentoFake);

            resultado.Should().Be(movimentoFake);
            await _contaCorrenteRepositorio.Received().BuscarContaCorrente(numeroConta);
            await _movimentoRepositorio.Received().AdicionarMovimento(movimentoFake);
        }

        [Fact]
        public async Task DeveriaRetornarErrosAoRalizarMovimentacaoEPossuirErrosDeValidacao() {
            int numeroConta = 123;

            Movimento movimentoFake = new(numeroConta, "D", 100);
            ContaCorrente contaCorrenteFake = new("", numeroConta, 0);

            _contaCorrenteRepositorio.BuscarContaCorrente(numeroConta).Returns(Task.FromResult(contaCorrenteFake));

            var resultado = await _contaCorrenteService.RealizarMovimentacao(movimentoFake);

            await _contaCorrenteRepositorio.Received().BuscarContaCorrente(numeroConta);
            await _movimentoRepositorio.DidNotReceive().AdicionarMovimento(movimentoFake);

            resultado.ErrosDeValidacao.Should().HaveCount(2);
            resultado.ErrosDeValidacao[0].ErrorMessage.Should().Be("Apenas contas correntes cadastradas podem realizar essa ação");
            resultado.ErrosDeValidacao[0].ErrorCode.Should().Be("INVALID_ACCOUNT");
            resultado.ErrosDeValidacao[1].ErrorMessage.Should().Be("Apenas contas correntes ativas podem  realizar essa ação");
            resultado.ErrosDeValidacao[1].ErrorCode.Should().Be("INACTIVE_ACCOUNT");
        }

        [Fact]
        public async Task DeveriaBuscarContaCorrenteCompleta() {
            int numeroConta = 123;
            string idConta = "testing";

            List<Movimento> movimentosFake = new() {
                new(numeroConta, "D", 100),
                new(numeroConta, "D", 200),
                new(numeroConta, "C", 300)
            };

            ContaCorrente contaCorrenteFake = new(idConta, numeroConta, 1);

            _contaCorrenteRepositorio.BuscarContaCorrente(numeroConta).Returns(Task.FromResult(contaCorrenteFake));
            _movimentoRepositorio.BuscarMovimentosPorContaCorrente(idConta).Returns(Task.FromResult<IEnumerable<Movimento>>(movimentosFake));

            var resultado = await _contaCorrenteService.BuscarContaCorrenteCompleta(numeroConta);

            resultado.Movimentos.Should().BeEquivalentTo(movimentosFake);
            await _contaCorrenteRepositorio.Received().BuscarContaCorrente(numeroConta);
            await _movimentoRepositorio.Received().BuscarMovimentosPorContaCorrente(idConta);
        }

        [Fact]
        public async Task DeveriaRetornarErrosAoBuscarContaCorrenteCompletaEPossuirErrosDeValidacao() {
            int numeroConta = 123;

            ContaCorrente contaCorrenteFake = new("", numeroConta, 0);

            _contaCorrenteRepositorio.BuscarContaCorrente(numeroConta).Returns(Task.FromResult(contaCorrenteFake));

            var resultado = await _contaCorrenteService.BuscarContaCorrenteCompleta(numeroConta);

            await _contaCorrenteRepositorio.Received().BuscarContaCorrente(numeroConta);
            await _movimentoRepositorio.DidNotReceive().BuscarMovimentosPorContaCorrente(Arg.Any<string>());

            resultado.ErrosDeValidacao.Should().HaveCount(2);
            resultado.ErrosDeValidacao[0].ErrorMessage.Should().Be("Apenas contas correntes cadastradas podem realizar essa ação");
            resultado.ErrosDeValidacao[0].ErrorCode.Should().Be("INVALID_ACCOUNT");
            resultado.ErrosDeValidacao[1].ErrorMessage.Should().Be("Apenas contas correntes ativas podem  realizar essa ação");
            resultado.ErrosDeValidacao[1].ErrorCode.Should().Be("INACTIVE_ACCOUNT");
        }
    }
}
