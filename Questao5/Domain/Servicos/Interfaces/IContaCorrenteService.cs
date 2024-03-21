using Questao5.Business.Abstracoes.ContaCorrente;
using Questao5.Domain.Entidades;

namespace Questao5.Domain.Servicos.Interfaces
{
    public interface IContaCorrenteService
    {
        Task<Movimento> RealizarMovimentacao(Movimento movimento);
        Task<ContaCorrente> BuscarContaCorrenteCompleta(int numeroConta);
    }
}
