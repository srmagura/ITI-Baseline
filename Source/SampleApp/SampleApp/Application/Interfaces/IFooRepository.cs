using Domain;

namespace FooSampleApp.Application.Interfaces
{
    public interface IFooRepository
    {
        void Add(Foo foo);
        Foo Get(FooId id);
    }
}