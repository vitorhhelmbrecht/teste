using FluentValidation.Results;
using Newtonsoft.Json;
using Questao5.Business.Abstracoes.Comum;
using Questao5.Domain.Entidades;

namespace Questao5.Business.Abstracoes.ContaCorrente
{
    public class RespostaMovimento : RespostaBase
    {
        public RespostaMovimento() {
            ErrosDeValidacao = new List<ValidationFailure>();
        }

        public Guid Id { get; set; }
    }
}
