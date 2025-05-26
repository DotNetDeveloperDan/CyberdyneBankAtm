using CyberdyneBankAtm.Application.Abstractions.Data;
using CyberdyneBankAtm.Domain.Accounts;
using CyberdyneBankAtm.Domain.Transactions;
using CyberdyneBankAtm.Domain.Users;
using CyberdyneBankAtm.SharedKernel;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CyberdyneBankAtm.Infrastructure.Database.Data;

public sealed class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IPublisher publisher)
    : DbContext(options), IApplicationDbContext
{
    public DbSet<User> Users { get; set; }

    public DbSet<Account> Accounts { get; set; }
    public DbSet<Transaction> Transactions { get; set; }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // When should you publish domain events?
        //
        // 1. BEFORE calling SaveChangesAsync
        //     - domain events are part of the same transaction
        //     - immediate consistency
        // 2. AFTER calling SaveChangesAsync
        //     - domain events are a separate transaction
        //     - eventual consistency
        //     - handlers can fail

        var result = await base.SaveChangesAsync(cancellationToken);

        await PublishDomainEventsAsync();

        return result;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        modelBuilder.HasDefaultSchema(Schemas.Default);
    }

    /// <summary>
    ///     Publishes all domain events that have been raised by entities tracked by the current DbContext.
    ///     This method performs the following steps:
    ///     1. Iterates over all tracked entities of type <see cref="Entity" /> in the current DbContext.
    ///     2. For each entity, retrieves its list of domain events via the <see cref="Entity.DomainEvents" /> property.
    ///     3. Clears the domain events from the entity to prevent duplicate publication.
    ///     4. Collects all domain events from all entities into a single list.
    ///     5. Publishes each domain event using the injected <see cref="IPublisher" /> instance.
    ///     This method is typically called after saving changes to the database, ensuring that domain events
    ///     are only published if the transaction succeeds.
    /// </summary>
    private async Task PublishDomainEventsAsync()
    {
        var domainEvents = ChangeTracker
            .Entries<Entity>()
            .Select(entry => entry.Entity)
            .SelectMany(entity =>
            {
                var domainEvents = entity.DomainEvents;

                entity.ClearDomainEvents();

                return domainEvents;
            })
            .ToList();

        foreach (var domainEvent in domainEvents) await publisher.Publish(domainEvent);
    }
}