using Questao5.Domain.Entidades;
using Questao5.Domain.Servicos.Interfaces;
using Questao5.Infrastructure.Repositorios.Interfaces;

namespace Questao5.Domain.Servicos
{
    public class IdEmPotenciaService : IIdEmPotenciaService
    {
        private readonly IIdEmPotenciaRepositorio _idEmPotenciaRepositorio;

        public IdEmPotenciaService(IIdEmPotenciaRepositorio idEmPotenciaRepositorio) {
            _idEmPotenciaRepositorio = idEmPotenciaRepositorio;
        }

        public Task SalvarRequisicao(IdEmPotencia idEmPotencial) =>
            _idEmPotenciaRepositorio.SalvarRequisicao(idEmPotencial);

        public Task<IdEmPotencia?> BuscarRequisicao(string identificadorIdEmPotencial) =>
            _idEmPotenciaRepositorio.BuscarRequisicao(identificadorIdEmPotencial);
    }
}
