using CyberdyneBankAtm.SharedKernel;

namespace CyberdyneBankAtm.Infrastructure.Time;

internal sealed class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}