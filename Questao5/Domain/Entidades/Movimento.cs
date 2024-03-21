using Questao5.Domain.Entidades.Comum;
using System.Security.Cryptography;
using System.Text;

namespace Questao5.Domain.Entidades
{
    public class Movimento : EntidadeBase
    {
        public Guid Id { get; private set; }
        public int NumeroContaCorrente { get; private set; }
        public string Data { get; private set; }
        public string Tipo { get; private set; }
        public double Valor { get; private set; }

        public string IdContaCorrente { get; private set; }

        public Movimento() {
            Id = Guid.Empty;
            NumeroContaCorrente = -1;
            Tipo = string.Empty;
            Valor = -1;
            Data = string.Empty;
            IdContaCorrente = string.Empty;

            ErrosDeValidacao = new();
        }

        public Movimento(int numeroContaCorrente, string tipoMovimento, double valor) {
            NumeroContaCorrente = numeroContaCorrente;
            Tipo = tipoMovimento;
            Valor = valor;
            Data = DateTime.UtcNow.ToString();
            IdContaCorrente = string.Empty;

            ErrosDeValidacao = new();

            Id = GenerateNewId();
        }

        private Guid GenerateNewId() {
            string uniqueValue = NumeroContaCorrente.ToString() + Data + Valor.ToString();

            using MD5 md5 = MD5.Create();
            byte[] hash = md5.ComputeHash(Encoding.UTF8.GetBytes(uniqueValue));
            
            return new Guid(hash);
        }

        public void AtualizarValoresDaContaCorrente(ContaCorrente contaCorrente) {
            IdContaCorrente = contaCorrente.Id;
        }
    }
}
