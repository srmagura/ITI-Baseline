using ITI.DDD.Core;
using ITI.DDD.Infrastructure.DataMapping;

namespace ITI.DDD.Infrastructure;

public abstract class Repository<TDataContext>
    where TDataContext : IDataContext
{
    private readonly IUnitOfWorkProvider _unitOfWorkProvider;
    protected IDbEntityMapper DbMapper { get; }

    protected Repository(IUnitOfWorkProvider unitOfWorkProvider, IDbEntityMapper dbMapper)
    {
        _unitOfWorkProvider = unitOfWorkProvider;
        DbMapper = dbMapper;
    }

    protected TDataContext Context
    {
        get
        {
            if (_unitOfWorkProvider.Current == null)
            {
                throw new NotSupportedException(
                    $"Attempted to use {GetType().Name} outside of a unit of work."
                );
            }

            return _unitOfWorkProvider.Current.GetDataContext<TDataContext>();
        }
    }
}
