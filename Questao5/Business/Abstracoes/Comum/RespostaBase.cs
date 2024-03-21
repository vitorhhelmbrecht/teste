using FluentValidation.Results;
using System.Text.Json.Serialization;

namespace Questao5.Business.Abstracoes.Comum
{
    public class RespostaBase
    {
        [JsonIgnore]
        public List<ValidationFailure> ErrosDeValidacao { get; set; }
    }
}
