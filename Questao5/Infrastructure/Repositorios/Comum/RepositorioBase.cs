using Microsoft.Data.Sqlite;
using Questao5.Infrastructure.Sqlite;
using System.Data;

namespace Questao5.Infrastructure.Repositorios.Comum
{
    public class RepositorioBase
    {
        public IDbConnection Connection { get; set; }

        public RepositorioBase(DatabaseConfig databaseConfig) {
            Connection = new SqliteConnection(databaseConfig.Name);
        }
    }
}
