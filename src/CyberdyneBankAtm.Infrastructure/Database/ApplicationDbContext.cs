using CyberdyneBankAtm.Application.Abstractions.Data;
using CyberdyneBankAtm.Domain.Accounts;
using CyberdyneBankAtm.Domain.Exceptions;
using CyberdyneBankAtm.Domain.Transactions;
using CyberdyneBankAtm.Domain.Users;
using CyberdyneBankAtm.SharedKernel;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CyberdyneBankAtm.Infrastructure.Database;

public sealed class ApplicationDbContext(
    DbContextOptions<ApplicationDbContext> options,
    IPublisher publisher,
    ILogger<ApplicationDbContext> logger) : DbContext(options), IApplicationDbContext
{


    public DbSet<User> Users { get; set; }
    public DbSet<Account> Accounts { get; set; }
    public DbSet<Transaction> Transactions { get; set; }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        int result;

        // 1. Save to the database, catching EF Core–specific errors
        try
        {
            result = await base.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            logger.LogError(ex, "A concurrency error occurred while saving changes.");
            throw;
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "A database update error occurred while saving changes.");
            throw;
        }

        // 2. Publish domain events, catching only known cases
        try
        {
            await PublishDomainEventsAsync(cancellationToken);
        }
        catch (OperationCanceledException ex)
        {
            logger.LogError(ex, "Publishing domain events was canceled.");
           
        }
        // If you have a custom exception type for publishing, catch it here:
        catch (DomainEventPublishException ex)
        {
            logger.LogError(ex, "A domain event publish error occurred.");
      
        }

        return result;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        modelBuilder.HasDefaultSchema(Schemas.Default);
    }

    private async Task PublishDomainEventsAsync(CancellationToken cancellationToken)
    {
        var domainEvents = ChangeTracker
            .Entries<Entity>()
            .Select(e => e.Entity)
            .SelectMany(entity =>
            {
                var events = entity.DomainEvents;
                entity.ClearDomainEvents();
                return events;
            })
            .ToList();

        foreach (var domainEvent in domainEvents)
        {
            try
            {
                await publisher.Publish(domainEvent, cancellationToken);
            }
            catch (OperationCanceledException)
            {
                // Already logged above, rethrow to stop processing
                throw;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to publish domain event {EventType}.", domainEvent.GetType().Name);
                throw new DomainEventPublishException(domainEvent.GetType().Name, ex);
            }
        }
    }
}
