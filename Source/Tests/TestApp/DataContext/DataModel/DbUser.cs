using ITI.Baseline.ValueObjects;
using ITI.DDD.Infrastructure.DataContext;
using TestApp.Domain.Enums;

namespace TestApp.DataContext.DataModel
{
    public abstract class DbUser : DbEntity
    {
        public UserType Type { get; set; }
        public EmailAddress? Email { get; set; }
    }
}
