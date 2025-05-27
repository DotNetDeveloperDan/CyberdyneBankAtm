using CyberdyneBankAtm.Domain.Accounts;
using CyberdyneBankAtm.Domain.Users;
using CyberdyneBankAtm.Infrastructure.Database;

namespace CyberdyneBankAtm.Api.Utilities
{
    public static class DbInitializer
    {
        public static void Seed(ApplicationDbContext context)
        {
            var newUserId = -1;
            // Only seed if Users table is empty
            if (!context.Users.Any())
            {
                var user = new User { FirstName = "John", LastName = "Connor", Email = "john.connor@sky.net" };
                context.Users.Add(user);
                // Add more seed data here
                context.SaveChanges();

                newUserId = user.Id;
            }

            // Repeat for other entities if needed
            if (!context.Accounts.Any())
            {
                context.Accounts.Add(new Account { Balance = 5000000, CreatedOn = DateTime.UtcNow,UserId = newUserId });
                context.Accounts.Add(new Account { Balance = 3, CreatedOn = DateTime.UtcNow, UserId = newUserId });
                context.Accounts.Add(new Account { Balance = 1445, CreatedOn = DateTime.UtcNow, UserId = newUserId });
                context.SaveChanges();
            }
        }
    }

}
