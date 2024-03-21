using FluentValidation;
using Questao5.Business.Abstracoes.ContaCorrente;

namespace Questao5.Business.Abstracoes.Validadores.ContaCorrente
{
    public class MovimentoValidador : AbstractValidator<MovimentoDto>
    {
        public MovimentoValidador() {
            RuleFor(r => r.IdRequisicao)
                .NotEmpty()
                .WithMessage("O identificador da conta não pode estar vazio")
                .WithErrorCode("INVALID_VALUE");

            RuleFor(r => r.NumeroContaCorrente)
                .NotEmpty()
                .WithMessage("O número da conta não pode estar vazio")
                .WithErrorCode("INVALID_VALUE");

            RuleFor(r => r.Valor)
                .GreaterThan(0)
                .WithMessage("O valor da transação deve ser maior que 0")
                .WithErrorCode("INVALID_VALUE");

            var tiposValidos = new List<string>() { "D", "C" };

            RuleFor(r => r.Tipo)
                .Must(r => tiposValidos.Contains(r))
                .WithMessage("O tipo da transação deve ser preenchido ou como 'D' (Débito) ou 'C' (Crédito)")
                .WithErrorCode("INVALID_TYPE");
        }
    }
}
