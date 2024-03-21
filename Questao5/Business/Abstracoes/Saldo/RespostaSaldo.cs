using FluentValidation.Results;
using Questao5.Business.Abstracoes.Comum;

namespace Questao5.Business.Abstracoes.Saldo
{
    public class RespostaSaldo : RespostaBase
    {
        public int NumeroContaCorrente { get; set; }
        public string NomeTitularConta { get; set; }
        public DateTime DataResposta { get; set; }
        public double SaldoAtual { get; set; }

        public RespostaSaldo() {
            ErrosDeValidacao = new List<ValidationFailure>();
            NomeTitularConta = string.Empty;
        }
    }
}
