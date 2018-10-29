using Domain;

namespace SampleApp.Application.Interfaces
{
    public interface IFooRepository
    {
        void Add(Foo foo);
        Foo Get(FooId id);
        void Remove(FooId id);
    }
}