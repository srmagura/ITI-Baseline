namespace ITI.DDD.Core
{
    public interface IDataContext : IDisposable
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        List<IDomainEvent> GetAllDomainEvents();
    }
}
