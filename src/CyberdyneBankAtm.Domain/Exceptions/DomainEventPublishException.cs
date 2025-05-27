namespace CyberdyneBankAtm.Domain.Exceptions;

/// <summary>
///     Thrown when publishing a domain event fails.
/// </summary>
public class DomainEventPublishException : Exception
{
    public DomainEventPublishException(string eventType)
        : base($"An error occurred publishing domain event '{eventType}'.")
    {
        EventType = eventType;
    }

    public DomainEventPublishException(string eventType, string message)
        : base(message)
    {
        EventType = eventType;
    }

    public DomainEventPublishException(string eventType, Exception innerException)
        : base($"An error occurred publishing domain event '{eventType}'. See inner exception for details.",
            innerException)
    {
        EventType = eventType;
    }

    public DomainEventPublishException(string eventType, string message, Exception innerException)
        : base(message, innerException)
    {
        EventType = eventType;
    }

    /// <summary>
    ///     The CLR type name of the domain event that failed to publish.
    /// </summary>
    public string EventType { get; }
}