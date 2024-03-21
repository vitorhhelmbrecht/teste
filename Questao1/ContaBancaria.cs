using System.Globalization;

namespace Questao1
{
    class ContaBancaria
    {
        public int Numero { get; private set; }
        public string Titular { get; private set; }
        public double Saldo { get; private set; }

        private const double TaxaSaque = 3.5;

        public ContaBancaria(int numero, string titular, double depositoInicial) {
            Numero = numero;
            Titular = titular;
            Saldo = depositoInicial;
        }

        public ContaBancaria(int numero, string titular) {
            Numero = numero;
            Titular = titular;
            Saldo = 0;
        }

        public void Deposito(double quantia) {
            Saldo += quantia;
        }

        public void Saque(double quantia) {
            Saldo -= quantia;
            Saldo -= TaxaSaque;
        }

        public override string ToString() {
            string stringSaldo = Saldo.ToString("0.00", CultureInfo.InvariantCulture);

            return $"Conta {Numero}, Titular: {Titular}, Saldo: $ {stringSaldo}";
        }
    }
}
