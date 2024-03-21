using NSubstitute;
using FluentAssertions;
using Xunit;
using Questao5.Business.ServicosAplicacao.Interfaces;
using Questao5.Application.Controllers.v1;
using Questao5.Business.Abstracoes.ContaCorrente;
using Microsoft.AspNetCore.Mvc;
using FluentValidation.Results;
using Questao5.Business.Abstracoes.Saldo;
using Newtonsoft.Json;

namespace Questao5.Application.Testes.Controllers
{
    public class ContaCorrenteControllerTestes
    {
        private readonly IContaCorrenteApplicationService _contaCorrenteService;
        private readonly ILogger<ContaCorrenteController> _logger;
        private readonly ContaCorrenteController _contaCorrenteController;

        public ContaCorrenteControllerTestes() {
            _contaCorrenteService = Substitute.For<IContaCorrenteApplicationService>();
            _logger = Substitute.For<ILogger<ContaCorrenteController>>();

            _contaCorrenteController = new ContaCorrenteController(_contaCorrenteService, _logger);
        }

        [Fact]
        public async Task DeveriaRetornarOkAoRealizarMovimento() {
            MovimentoDto dto = new MovimentoDto() {
                IdRequisicao = Guid.Empty.ToString(),
                NumeroContaCorrente = 123,
                Tipo = "D",
                Valor = 500
            };

            var fakeResult = new RespostaMovimento() {
                Id = Guid.NewGuid()
            };

            _contaCorrenteService.RealizarMovimentacao(dto).Returns(Task.FromResult(fakeResult));

            var resposta = await _contaCorrenteController.RealizarMovimento(dto);

            await _contaCorrenteService.Received().RealizarMovimentacao(dto);
            var respostaOk = resposta.Result as OkObjectResult;
            respostaOk!.StatusCode.Should().Be(200);
            JsonConvert.SerializeObject(respostaOk.Value)
                .Should().Be(JsonConvert.SerializeObject(fakeResult));
        }

        [Fact]
        public async Task DeveriaRetornarBadRequestAoRealizarMovimentoAoPossuirErroDeValidacao() {
            MovimentoDto dto = new MovimentoDto() {
                IdRequisicao = Guid.Empty.ToString(),
                NumeroContaCorrente = 123,
                Tipo = "D",
                Valor = 500
            };

            var fakeResult = new RespostaMovimento() {
                ErrosDeValidacao = new List<ValidationFailure>() {
                    new ValidationFailure("Valor", "Valor deve ser maior que 0")
                }
            };

            _contaCorrenteService.RealizarMovimentacao(dto).Returns(Task.FromResult(fakeResult));

            var resposta = await _contaCorrenteController.RealizarMovimento(dto);

            await _contaCorrenteService.Received().RealizarMovimentacao(dto);
            var respostaBadRequest = resposta.Result as BadRequestObjectResult;
            respostaBadRequest!.StatusCode.Should().Be(400);
            JsonConvert.SerializeObject(respostaBadRequest.Value)
                .Should().Be(JsonConvert.SerializeObject(fakeResult.ErrosDeValidacao));
        }

        [Fact]
        public async Task DeveriaRetornarOkAoBuscarSaldo() {
            var fakeResult = new RespostaSaldo() {
                DataResposta = DateTime.Now,
                NomeTitularConta = string.Empty,
                NumeroContaCorrente = 123,
                SaldoAtual = 10
            };

            int numeroConta = 123;

            _contaCorrenteService.BuscarSaldo(numeroConta).Returns(Task.FromResult(fakeResult));

            var resposta = await _contaCorrenteController.CalcularSaldo(numeroConta);

            await _contaCorrenteService.Received().BuscarSaldo(numeroConta);
            var respostaOk = resposta.Result as OkObjectResult;
            respostaOk!.StatusCode.Should().Be(200);
            JsonConvert.SerializeObject(respostaOk.Value)
                .Should().Be(JsonConvert.SerializeObject(fakeResult));
        }

        [Fact]
        public async Task DeveriaRetornarBadRequestAoBuscarSaldoAoPossuirErroDeValidacao() {
            var fakeResult = new RespostaSaldo() {
                ErrosDeValidacao = new List<ValidationFailure>() {
                    new ValidationFailure("numeroConta", "O numero da conta não é válido")
                }
            };

            int numeroConta = 123;

            _contaCorrenteService.BuscarSaldo(numeroConta).Returns(Task.FromResult(fakeResult));

            var resposta = await _contaCorrenteController.CalcularSaldo(numeroConta);

            await _contaCorrenteService.Received().BuscarSaldo(numeroConta);
            var respostaBadRequest = resposta.Result as BadRequestObjectResult;
            respostaBadRequest!.StatusCode.Should().Be(400);
            JsonConvert.SerializeObject(respostaBadRequest.Value)
                .Should().Be(JsonConvert.SerializeObject(fakeResult.ErrosDeValidacao));
        }
    }
}
