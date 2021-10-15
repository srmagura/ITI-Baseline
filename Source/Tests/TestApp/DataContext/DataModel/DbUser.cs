using ITI.Baseline.ValueObjects;
using ITI.DDD.Infrastructure.DataContext;
using Microsoft.EntityFrameworkCore;
using TestApp.Domain.Enums;

namespace TestApp.DataContext.DataModel
{
    public abstract class DbUser : DbEntity
    {
        public UserType Type { get; set; }
        public EmailAddress? Email { get; set; }

        public static void OnModelCreating(ModelBuilder mb)
        {
            mb.Entity<DbUser>()
                .HasDiscriminator(u => u.Type)
                .HasValue<DbCustomerUser>(UserType.Customer)
                .HasValue<DbOnCallUser>(UserType.OnCall);
        }
    }
}
