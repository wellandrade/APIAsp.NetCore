using FluentValidation;

namespace DevIO.Business.Models.Validacao
{
    public class FornecedorValidacao : AbstractValidator<Fornecedor>
    {
        public FornecedorValidacao()
        {
            RuleFor(f => f.Nome)
                .NotEmpty().WithMessage("O campo {PropertyName} nao foi informado")
                .Length(2, 50).WithMessage("O campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} de caracteres");
        }
    }
}
