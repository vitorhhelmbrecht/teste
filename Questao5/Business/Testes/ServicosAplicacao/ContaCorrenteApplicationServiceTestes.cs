using FluentAssertions;
using NSubstitute;
using Questao5.Business.ServicosAplicacao.Interfaces;
using Questao5.Business.ServicosAplicacao;
using Questao5.Domain.Servicos.Interfaces;
using Xunit;
using Questao5.Business.Abstracoes.ContaCorrente;
using Questao5.Domain.Entidades;
using FluentValidation.Results;

namespace Questao5.Business.Testes.ServicosAplicacao
{
    public class ContaCorrenteApplicationServiceTestes
    {
        private readonly IContaCorrenteService _contaCorrenteService;
        private readonly IIdEmPotenciaApplicationService _idEmPotenciaApplicationService;
        private readonly ILogger<ContaCorrenteApplicationService> _logger;
        private readonly ContaCorrenteApplicationService _contaCorrenteApplicationService;

        public ContaCorrenteApplicationServiceTestes() {
            _contaCorrenteService = Substitute.For<IContaCorrenteService>();
            _idEmPotenciaApplicationService = Substitute.For<IIdEmPotenciaApplicationService>();
            _logger = Substitute.For<ILogger<ContaCorrenteApplicationService>>();

            _contaCorrenteApplicationService = new ContaCorrenteApplicationService(_contaCorrenteService, _idEmPotenciaApplicationService, _logger);
        }

        [Fact]
        public async Task DeveriaRealizarMovimentacao() {
            MovimentoDto movimentoDto = new () {
                IdRequisicao = "123",
                NumeroContaCorrente = 123,
                Tipo = "D",
                Valor = 200.00
            };

            Movimento movimentoFake = new(654, "D", 200.00);

            _idEmPotenciaApplicationService.BuscarRespostaPassada("123").Returns(Task.FromResult<RespostaMovimento?>(null));
            _contaCorrenteService.RealizarMovimentacao(Arg.Any<Movimento>()).Returns(Task.FromResult(movimentoFake));

            var resposta = await _contaCorrenteApplicationService.RealizarMovimentacao(movimentoDto);

            await _idEmPotenciaApplicationService.Received().BuscarRespostaPassada("123");
            await _contaCorrenteService.Received().RealizarMovimentacao(Arg.Any<Movimento>());
            await _idEmPotenciaApplicationService.Received().SalvarRespostaComoIdEmPotencial(movimentoDto, Arg.Any<RespostaMovimento>());

            resposta.Id.Should().Be(movimentoFake.Id);
        }

        [Fact]
        public async Task DeveriaRetornarMovimentacaoJaRealizada() {
            MovimentoDto movimentoDto = new() {
                IdRequisicao = "123",
                NumeroContaCorrente = 123,
                Tipo = "D",
                Valor = 200.00
            };

            RespostaMovimento respostaFake = new() {
                Id = Guid.NewGuid()
            };

            _idEmPotenciaApplicationService.BuscarRespostaPassada("123").Returns(Task.FromResult<RespostaMovimento?>(respostaFake));

            var resposta = await _contaCorrenteApplicationService.RealizarMovimentacao(movimentoDto);

            await _idEmPotenciaApplicationService.Received().BuscarRespostaPassada("123");
            await _contaCorrenteService.DidNotReceive().RealizarMovimentacao(Arg.Any<Movimento>());
            await _idEmPotenciaApplicationService.DidNotReceive().SalvarRespostaComoIdEmPotencial(movimentoDto, Arg.Any<RespostaMovimento>());

            resposta.Should().Be(respostaFake);
        }

        [Fact]
        public async Task DeveriaRetornarRespostaComErrosDeValidacao() {
            MovimentoDto movimentoDto = new() {
                Tipo = "F",
                Valor = 0
            };

            RespostaMovimento respostaFake = new() {
                Id = Guid.NewGuid()
            };

            var resposta = await _contaCorrenteApplicationService.RealizarMovimentacao(movimentoDto);

            await _idEmPotenciaApplicationService.DidNotReceive().BuscarRespostaPassada(Arg.Any<string>());
            await _contaCorrenteService.DidNotReceive().RealizarMovimentacao(Arg.Any<Movimento>());
            await _idEmPotenciaApplicationService.DidNotReceive().SalvarRespostaComoIdEmPotencial(Arg.Any<MovimentoDto>(), Arg.Any<RespostaMovimento>());

            resposta.ErrosDeValidacao.Should().HaveCount(4);
            resposta.ErrosDeValidacao[0].ErrorMessage.Should().Be("O identificador da conta não pode estar vazio");
            resposta.ErrosDeValidacao[0].ErrorCode.Should().Be("INVALID_VALUE");
            resposta.ErrosDeValidacao[1].ErrorMessage.Should().Be("O número da conta não pode estar vazio");
            resposta.ErrosDeValidacao[1].ErrorCode.Should().Be("INVALID_VALUE");
            resposta.ErrosDeValidacao[2].ErrorMessage.Should().Be("O valor da transação deve ser maior que 0");
            resposta.ErrosDeValidacao[2].ErrorCode.Should().Be("INVALID_VALUE");
            resposta.ErrosDeValidacao[3].ErrorMessage.Should().Be("O tipo da transação deve ser preenchido ou como 'D' (Débito) ou 'C' (Crédito)");
            resposta.ErrosDeValidacao[3].ErrorCode.Should().Be("INVALID_TYPE");
        }

        [Fact]
        public async Task DeveriaRetornarRespostaComErrosDeValidacaoDoMovimentoGerado() {
            MovimentoDto movimentoDto = new() {
                IdRequisicao = "123",
                NumeroContaCorrente = 123,
                Tipo = "D",
                Valor = 200.00
            };

            Movimento movimentoFake = new() {
                ErrosDeValidacao = new List<ValidationFailure>() {
                    new ValidationFailure("teste", "testando")
                }
            };

            _idEmPotenciaApplicationService.BuscarRespostaPassada("123").Returns(Task.FromResult<RespostaMovimento?>(null));
            _contaCorrenteService.RealizarMovimentacao(Arg.Any<Movimento>()).Returns(Task.FromResult(movimentoFake));

            var resposta = await _contaCorrenteApplicationService.RealizarMovimentacao(movimentoDto);

            await _idEmPotenciaApplicationService.Received().BuscarRespostaPassada("123");
            await _contaCorrenteService.Received().RealizarMovimentacao(Arg.Any<Movimento>());
            await _idEmPotenciaApplicationService.DidNotReceive().SalvarRespostaComoIdEmPotencial(Arg.Any<MovimentoDto>(), Arg.Any<RespostaMovimento>());

            resposta.ErrosDeValidacao.Should().HaveCount(1);
            resposta.ErrosDeValidacao[0].PropertyName.Should().Be("teste");
            resposta.ErrosDeValidacao[0].ErrorMessage.Should().Be("testando");
        }

        [Fact]
        public async Task DeveriaBuscarSaldo() {
            int numeroConta = 123;

            ContaCorrente contaCorrenteFake = new ("123", 123, 1);

            List<Movimento> movimentos = new () {
                new Movimento(numeroConta, "D", 50),
                new Movimento(numeroConta, "D", 100),
                new Movimento(numeroConta, "C", 500),
                new Movimento(numeroConta, "D", 1000),
                new Movimento(numeroConta, "C", 50)
            };

            contaCorrenteFake.AtualizarMovimentos(movimentos);

            _contaCorrenteService.BuscarContaCorrenteCompleta(numeroConta).Returns(Task.FromResult(contaCorrenteFake));

            var resposta = await _contaCorrenteApplicationService.BuscarSaldo(numeroConta);

            resposta.DataResposta.Date.Should().Be(DateTime.UtcNow.Date);
            resposta.NomeTitularConta.Should().Be(contaCorrenteFake.Titular);
            resposta.NumeroContaCorrente.Should().Be(contaCorrenteFake.Numero);
            resposta.SaldoAtual.Should().Be(600);
        }

        [Fact]
        public async Task DeveriaRetornarListaDeErrosAoBuscarSaldo() {
            int numeroConta = 123;

            ContaCorrente contaCorrenteFake = new() {
                ErrosDeValidacao = new List<ValidationFailure>() {
                    new ValidationFailure("teste", "testando")
                }
            };

            _contaCorrenteService.BuscarContaCorrenteCompleta(numeroConta).Returns(Task.FromResult(contaCorrenteFake));

            var resposta = await _contaCorrenteApplicationService.BuscarSaldo(numeroConta);

            resposta.ErrosDeValidacao.Should().HaveCount(1);
            resposta.ErrosDeValidacao[0].PropertyName.Should().Be("teste");
            resposta.ErrosDeValidacao[0].ErrorMessage.Should().Be("testando");
        }
    }
}
