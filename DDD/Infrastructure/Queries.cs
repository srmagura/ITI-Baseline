using ITI.DDD.Core;

namespace ITI.DDD.Infrastructure;

public abstract class Queries<TDataContext>
    where TDataContext : IDataContext
{
    private readonly IUnitOfWorkProvider _unitOfWorkProvider;

    protected Queries(IUnitOfWorkProvider unitOfWorkProvider)
    {
        _unitOfWorkProvider = unitOfWorkProvider;
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
