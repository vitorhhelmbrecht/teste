using Questao5.Domain.Entidades;

namespace Questao5.Infrastructure.Repositorios.Interfaces
{
    public interface IContaCorrenteRepositorio
    {
        Task<ContaCorrente> BuscarContaCorrente(int numeroConta);
    }
}
