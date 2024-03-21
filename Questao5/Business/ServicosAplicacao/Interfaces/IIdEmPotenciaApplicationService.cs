using Questao5.Business.Abstracoes.ContaCorrente;
using Questao5.Domain.Entidades;

namespace Questao5.Business.ServicosAplicacao.Interfaces
{
    public interface IIdEmPotenciaApplicationService
    {
        Task<RespostaMovimento?> BuscarRespostaPassada(string idRequisicao);
        Task SalvarRespostaComoIdEmPotencial(MovimentoDto movimentoDto, RespostaMovimento respostaMovimento);
    }
}
