using Questao5.Domain.Entidades;

namespace Questao5.Infrastructure.Repositorios.Interfaces
{
    public interface IIdEmPotenciaRepositorio
    {
        Task SalvarRequisicao(IdEmPotencia idEmPotencia);
        Task<IdEmPotencia?> BuscarRequisicao(string identificadorIdEmPotencia);
    }
}
