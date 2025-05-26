using FluentValidation;

namespace CyberdyneBankAtm.Application.Transactions.Transfer;

public class CreateTransferCommandValidator : AbstractValidator<CreateTransferCommand>
{
    public CreateTransferCommandValidator()
    {
        RuleFor(c => c.AccountId).NotEmpty();
        RuleFor(c => c.RelatedAccountId).NotNull().NotEmpty();
        RuleFor(c => c.Amount).GreaterThan(0);
    }
}