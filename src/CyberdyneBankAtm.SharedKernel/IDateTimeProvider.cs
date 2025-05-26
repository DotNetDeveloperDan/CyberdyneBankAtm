namespace CyberdyneBankAtm.SharedKernel;

public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
}