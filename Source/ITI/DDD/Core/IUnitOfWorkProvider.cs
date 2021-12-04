namespace ITI.DDD.Core;

public interface IUnitOfWorkProvider
{
    IUnitOfWork Begin();
    IUnitOfWork? Current { get; }
}
