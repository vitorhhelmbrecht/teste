using FluentValidation.Results;

namespace Questao5.Domain.Entidades.Comum
{
    public class EntidadeBase
    {
        public List<ValidationFailure> ErrosDeValidacao { get; set; }
    }
}
