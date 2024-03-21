using Questao5.Domain.Servicos.Interfaces;
using Xunit;
using FluentAssertions;
using NSubstitute;
using Questao5.Business.ServicosAplicacao;
using Questao5.Domain.Entidades;
using Questao5.Business.Abstracoes.ContaCorrente;
using Newtonsoft.Json;

namespace Questao5.Business.Testes.ServicosAplicacao
{
    public class IdEmPotenciaApplicationServiceTestes
    {
        private readonly IIdEmPotenciaService _idEmPotenciaService;
        private readonly ILogger<IdEmPotenciaApplicationService> _logger;
        private readonly IdEmPotenciaApplicationService _idEmPotenciaApplicationService;

        public IdEmPotenciaApplicationServiceTestes() {
            _idEmPotenciaService = Substitute.For<IIdEmPotenciaService>();
            _logger = Substitute.For<ILogger<IdEmPotenciaApplicationService>>();

            _idEmPotenciaApplicationService = new (_idEmPotenciaService, _logger);
        }

        [Fact]
        public async Task DeveriaBuscarRespostaPassada() {
            string idRequisicao = "123";

            RespostaMovimento respostaMovimentoEsperada = new RespostaMovimento() {
                Id = Guid.NewGuid()
            };

            IdEmPotencia idEmPotencia = new () {
                Id = idRequisicao,
                Requisicao = "",
                Resultado = JsonConvert.SerializeObject(respostaMovimentoEsperada)
            };

            _idEmPotenciaService.BuscarRequisicao(idRequisicao).Returns(Task.FromResult<IdEmPotencia?>(idEmPotencia));

            var resultado = await _idEmPotenciaApplicationService.BuscarRespostaPassada(idRequisicao);

            resultado.Should().BeEquivalentTo(respostaMovimentoEsperada);
        }

        [Fact]
        public async Task DeveriaRetornarNullAoBuscarRespostaPassadaAoNaoAchar() {
            string idRequisicao = "123";

            _idEmPotenciaService.BuscarRequisicao(idRequisicao).Returns(Task.FromResult<IdEmPotencia?>(null));

            var resultado = await _idEmPotenciaApplicationService.BuscarRespostaPassada(idRequisicao);

            resultado.Should().BeNull();
        }

        [Fact]
        public async Task DeveriaRetornarNullAoBuscarRespostaPassadaAoNaoConseguirConverter() {
            string idRequisicao = "123";

            IdEmPotencia idEmPotencia = new () {
                Id = idRequisicao,
                Requisicao = "",
                Resultado = "{ teste: testing }"
            };

            _idEmPotenciaService.BuscarRequisicao(idRequisicao).Returns(Task.FromResult<IdEmPotencia?>(idEmPotencia));

            var resultado = await _idEmPotenciaApplicationService.BuscarRespostaPassada(idRequisicao);

            resultado.Should().BeNull();
        }
    }
}
