using Questao5.Business.Abstracoes.ContaCorrente;
using Questao5.Business.Abstracoes.Saldo;

namespace Questao5.Business.ServicosAplicacao.Interfaces
{
    public interface IContaCorrenteApplicationService
    {
        Task<RespostaMovimento> RealizarMovimentacao(MovimentoDto movimento);
        Task<RespostaSaldo> BuscarSaldo(int numeroConta);
    }
}
