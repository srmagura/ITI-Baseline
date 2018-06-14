using System.Collections.Generic;

namespace Domain.DomainServices
{
    public interface IFooFighter
    {
        Foo Create(string name, List<Bar> bars, List<int> someInts);
    }
}