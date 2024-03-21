using Questao5.Domain.Entidades.Comum;

namespace Questao5.Domain.Entidades
{
    public class ContaCorrente : EntidadeBase
    {
        public string Id { get; private set; }
        public string Titular { get; private set; }
        public int Numero { get; private set; }
        public int Ativo { get; private set; }
        public List<Movimento> Movimentos { get; private set; }

        public bool EstaAtivo { get { return Ativo == 1; } }

        public ContaCorrente() {
            Id = string.Empty;
            Titular = string.Empty;
            ErrosDeValidacao = new();
            Movimentos = new();
        }

        public ContaCorrente(string id, int numero, int ativo) {
            Id = id;
            Numero = numero;
            Ativo = ativo;

            Titular = string.Empty;
            ErrosDeValidacao = new();
            Movimentos = new();
        }

        public void AtualizarMovimentos(List<Movimento> movimentos) {
            Movimentos = movimentos;
        }

        public double CalcularSaldo() {
            if (Movimentos.Count == 0)
                return 0;

            double saldo = 0;
            foreach (var movimento in Movimentos) {
                if (movimento.Tipo == "D")
                    saldo += movimento.Valor;
                else
                    saldo -= movimento.Valor;
            }

            return saldo;
        }
    }
}
