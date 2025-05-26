using CyberdyneBankAtm.Domain.Transactions;
using FluentValidation;

namespace CyberdyneBankAtm.Application.Transactions.Create
{
    public class CreateTransactionCommandValidator : AbstractValidator<CreateTransactionCommand>
    {
        public CreateTransactionCommandValidator()
        {
            RuleFor(c => c.AccountId).NotEmpty();
            RuleFor(c => c.Amount).GreaterThan(0);
            RuleFor(c => c.Type)
                .Must(type => type == TransactionType.Deposit || type == TransactionType.Withdraw)
                .WithMessage("Type must be either Deposit or Withdraw.");
            RuleFor(c => c.Description).NotEmpty().MaximumLength(255);
        }
    }
}
