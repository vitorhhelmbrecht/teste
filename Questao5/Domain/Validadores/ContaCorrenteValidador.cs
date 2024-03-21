using FluentAssertions;
using FluentValidation;
using Questao5.Domain.Entidades;

namespace Questao5.Domain.Validadores
{
    public class ContaCorrenteValidador : AbstractValidator<ContaCorrente>
    {
        public ContaCorrenteValidador() {
            RuleFor(r => r.Id)
                .NotEmpty()
                .WithMessage("Apenas contas correntes cadastradas podem realizar essa ação")
                .WithErrorCode("INVALID_ACCOUNT");

            RuleFor(r => r.EstaAtivo)
                .Must(r => r)
                .WithMessage("Apenas contas correntes ativas podem  realizar essa ação")
                .WithErrorCode("INACTIVE_ACCOUNT");
        }
    }
}
