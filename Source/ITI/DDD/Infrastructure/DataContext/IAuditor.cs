using Microsoft.EntityFrameworkCore;

namespace ITI.DDD.Infrastructure.DataContext
{
    public interface IAuditor
    {
        public void Process(DbContext context);
    }
}
