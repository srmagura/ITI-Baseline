using Microsoft.EntityFrameworkCore;

namespace ITI.DDD.Application
{
    public interface IAuditor
    {
        public void Process(DbContext context);
    }
}
