using Newtonsoft.Json;
using Questao5.Business.Abstracoes.ContaCorrente;
using Questao5.Business.ServicosAplicacao.Interfaces;
using Questao5.Domain.Entidades;
using Questao5.Domain.Servicos.Interfaces;

namespace Questao5.Business.ServicosAplicacao
{
    public class IdEmPotenciaApplicationService : IIdEmPotenciaApplicationService
    {
        private readonly IIdEmPotenciaService _idEmPotenciaService;
        private readonly ILogger<IdEmPotenciaApplicationService> _logger;

        public IdEmPotenciaApplicationService(IIdEmPotenciaService idEmPotenciaService, ILogger<IdEmPotenciaApplicationService> logger) {
            _idEmPotenciaService = idEmPotenciaService;
            _logger = logger;
        }

        public async Task<RespostaMovimento?> BuscarRespostaPassada(string idRequisicao) {
            var idEmPotencia = await _idEmPotenciaService.BuscarRequisicao(idRequisicao);

            if (idEmPotencia != null) {
                try {
                    var resultadoConvertido = JsonConvert.DeserializeObject<RespostaMovimento>(idEmPotencia.Resultado);
                    return resultadoConvertido;
                } catch (Exception ex) {
                    _logger.LogError("Erro ao converter o resultado do id em potência: {message}, {error}", ex.Message, ex);
                }
            }

            return null;
        }

        public async Task SalvarRespostaComoIdEmPotencial(MovimentoDto movimentoDto, RespostaMovimento respostaMovimento) {
            var jsonRequisicao = JsonConvert.SerializeObject(movimentoDto);
            var jsonResultado = JsonConvert.SerializeObject(respostaMovimento);

            var idEmPotencial = new IdEmPotencia() {
                Id = movimentoDto.IdRequisicao,
                Requisicao = jsonRequisicao,
                Resultado = jsonResultado
            };

            await _idEmPotenciaService.SalvarRequisicao(idEmPotencial);
        }
    }
}
