using Dapper;
using Questao5.Domain.Entidades;
using Questao5.Infrastructure.Repositorios.Comum;
using Questao5.Infrastructure.Repositorios.Interfaces;
using Questao5.Infrastructure.Sqlite;

namespace Questao5.Infrastructure.Repositorios
{
    public class ContaCorrenteRepositorio : RepositorioBase, IContaCorrenteRepositorio
    {
        private readonly ILogger<ContaCorrenteRepositorio> _logger;

        public ContaCorrenteRepositorio(DatabaseConfig databaseConfig, ILogger<ContaCorrenteRepositorio> logger) : base(databaseConfig) {
            _logger = logger;
        }

        public async Task<ContaCorrente> BuscarContaCorrente(int numeroConta) {
            try {
                var query = "SELECT idcontacorrente as id, numero, ativo FROM contacorrente WHERE numero = @numeroConta;";

                _logger.LogDebug("Executando query", query);
                var result = await Connection.QueryFirstOrDefaultAsync<ContaCorrente>(query, CriarParametrosParaBuscaContaCorrente(numeroConta));

                result ??= new ContaCorrente("", -1, 1);

                return result;
            } catch (Exception ex) {
                throw new InvalidOperationException(ex.Message, ex);
            } finally {
                Connection.Close();
            }
        }

        private static object CriarParametrosParaBuscaContaCorrente(int numeroConta) {
            var parametros = new DynamicParameters();

            parametros.Add("@numeroConta", numeroConta);

            return parametros;
        }
    }
}
