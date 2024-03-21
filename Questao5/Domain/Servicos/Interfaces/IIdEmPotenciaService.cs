using Questao5.Domain.Entidades;

namespace Questao5.Domain.Servicos.Interfaces
{
    public interface IIdEmPotenciaService
    {
        Task SalvarRequisicao(IdEmPotencia idEmPotencia);
        Task<IdEmPotencia?> BuscarRequisicao(string identificadorIdEmPotencia);
    }
}
