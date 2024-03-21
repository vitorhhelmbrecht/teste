
using Questao5.Domain.Entidades;

namespace Questao5.Infrastructure.Repositorios.Interfaces
{
    public interface IMovimentoRepositorio
    {
        Task<Movimento> AdicionarMovimento(Movimento movimento);
        Task<IEnumerable<Movimento>> BuscarMovimentosPorContaCorrente(string idContaCorrente);
    }
}
