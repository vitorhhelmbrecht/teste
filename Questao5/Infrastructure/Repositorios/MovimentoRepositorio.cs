using Dapper;
using Questao5.Domain.Entidades;
using Questao5.Infrastructure.Repositorios.Comum;
using Questao5.Infrastructure.Repositorios.Interfaces;
using Questao5.Infrastructure.Sqlite;

namespace Questao5.Infrastructure.Repositorios
{
    public class MovimentoRepositorio : RepositorioBase, IMovimentoRepositorio
    {
        private readonly ILogger<MovimentoRepositorio> _logger;

        public MovimentoRepositorio(DatabaseConfig databaseConfig, ILogger<MovimentoRepositorio> logger) : base(databaseConfig) {
            _logger = logger;
        }

        public async Task<Movimento> AdicionarMovimento(Movimento movimento) {
            try {
                var query = "INSERT INTO movimento " +
                    "(idmovimento, idcontacorrente, datamovimento, tipomovimento, valor) " +
                    "VALUES (@id, @contaCorrente, @data, @tipo, @valor);";

                _logger.LogDebug("Executando query", query);
                await Connection.ExecuteAsync(query, CriarParametrosDeCriacaoDeMovimento(movimento));

                return movimento;
            } catch (Exception ex) {
                throw new InvalidOperationException(ex.Message, ex);
            } finally {
                Connection.Close();
            }
        }

        public async Task<IEnumerable<Movimento>> BuscarMovimentosPorContaCorrente(string idContaCorrente) {
            try {
                var query = "SELECT tipomovimento as Tipo, valor FROM movimento WHERE idcontacorrente = @contaCorrente;";

                _logger.LogDebug("Executando query", query);
                var movimentos = await Connection.QueryAsync<Movimento>(query, CriarParametrosBuscaMovimentosPorContaCorrente(idContaCorrente));

                movimentos ??= new List<Movimento>();

                return movimentos;
            } catch (Exception ex) {
                throw new InvalidOperationException(ex.Message, ex);
            } finally {
                Connection.Close();
            }
        }

        private static object CriarParametrosDeCriacaoDeMovimento(Movimento movimento) {
            var parametros = new DynamicParameters();

            parametros.Add("@id", movimento.Id);
            parametros.Add("@contaCorrente", movimento.IdContaCorrente);
            parametros.Add("@data", movimento.Data);
            parametros.Add("@tipo", movimento.Tipo);
            parametros.Add("@valor", movimento.Valor);

            return parametros;
        }

        private static object CriarParametrosBuscaMovimentosPorContaCorrente(string idContaCorrente) {
            var parametros = new DynamicParameters();

            parametros.Add("@contaCorrente", idContaCorrente);

            return parametros;
        }
    }
}
