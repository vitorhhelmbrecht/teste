using Dapper;
using Questao5.Domain.Entidades;
using Questao5.Infrastructure.Repositorios.Comum;
using Questao5.Infrastructure.Repositorios.Interfaces;
using Questao5.Infrastructure.Sqlite;

namespace Questao5.Infrastructure.Repositorios
{
    public class IdEmPotenciaRepositorio :  RepositorioBase, IIdEmPotenciaRepositorio
    {
        private readonly ILogger<IdEmPotenciaRepositorio> _logger;

        public IdEmPotenciaRepositorio(DatabaseConfig databaseConfig, ILogger<IdEmPotenciaRepositorio> logger) : base(databaseConfig) {
            _logger = logger;
        }

        public async Task SalvarRequisicao(IdEmPotencia idEmPotencia) {
            try {
                var query = "INSERT INTO idempotencia " +
                    "(chave_idempotencia, requisicao, resultado) " +
                    "VALUES (@chave, @requisicao, @resultado)";

                _logger.LogDebug("Executando query", query);
                await Connection.ExecuteAsync(query, CriarParametrosSalvarRequisicao(idEmPotencia));
            } catch (Exception ex) {
                throw new InvalidOperationException(ex.Message, ex);
            } finally {
                Connection.Close();
            }
        }

        public async Task<IdEmPotencia?> BuscarRequisicao(string identificadorIdEmPotencia) {
            try {
                var query = "SELECT requisicao, resultado FROM idempotencia WHERE chave_idempotencia = @chave";

                _logger.LogDebug("Executando query", query);
                var idEmPotencia = await Connection.QueryFirstOrDefaultAsync<IdEmPotencia?>(query, CriarParametrosBuscarRequisicao(identificadorIdEmPotencia));

                return idEmPotencia;
            } catch (Exception ex) {
                throw new InvalidOperationException(ex.Message, ex);
            } finally {
                Connection.Close();
            }
        }

        private static object CriarParametrosSalvarRequisicao(IdEmPotencia idEmPotencia) {
            var parametros = new DynamicParameters();

            parametros.Add("@chave", idEmPotencia.Id);
            parametros.Add("@requisicao", idEmPotencia.Requisicao);
            parametros.Add("@resultado", idEmPotencia.Resultado);

            return parametros;
        }

        private static object CriarParametrosBuscarRequisicao(string identificadorIdEmPotencia) {
            var parametros = new DynamicParameters();

            parametros.Add("@chave", identificadorIdEmPotencia);

            return parametros;
        }
    }
}
