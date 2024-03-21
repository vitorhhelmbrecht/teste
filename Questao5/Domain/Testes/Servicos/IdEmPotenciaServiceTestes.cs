using Questao5.Domain.Servicos;
using Questao5.Infrastructure.Repositorios.Interfaces;
using NSubstitute;
using Xunit;
using Questao5.Domain.Entidades;

namespace Questao5.Domain.Testes.Servicos
{
    public class IdEmPotenciaServiceTestes
    {
        private readonly IIdEmPotenciaRepositorio _idEmPotenciaRepositorio;
        private readonly IdEmPotenciaService _idEmPotenciaService;

        public IdEmPotenciaServiceTestes() {
            _idEmPotenciaRepositorio = Substitute.For<IIdEmPotenciaRepositorio>();
            _idEmPotenciaService = new (_idEmPotenciaRepositorio);
        }

        [Fact]
        public async Task DeveriaSalvarRequisicao() {
            IdEmPotencia idEmPotencia = new();

            await _idEmPotenciaService.SalvarRequisicao(idEmPotencia);

            await _idEmPotenciaRepositorio.Received().SalvarRequisicao(idEmPotencia);
        }

        [Fact]
        public async Task DeveriaBuscarRequisicao() {
            string identificador = "123";

            await _idEmPotenciaService.BuscarRequisicao(identificador);

            await _idEmPotenciaRepositorio.Received().BuscarRequisicao(identificador);
        }
    }
}
