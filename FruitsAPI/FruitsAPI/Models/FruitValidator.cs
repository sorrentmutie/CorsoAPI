using FluentValidation;

namespace FruitsAPI.Models;

public class FruitValidator : AbstractValidator<FruitModel>
{
    public FruitValidator()
    {
        RuleFor(x => x.name)
            .NotEmpty()
            .WithMessage("Il nome del frutto non può essere vuoto.");

        RuleFor(x => x.name)
            .MaximumLength(100)
            .WithMessage("Il nome del frutto non può superare 100 caratteri.");
    }

}
