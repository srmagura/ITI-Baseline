using System.Collections.Generic;
using Iti.Core.Sequences;
using Iti.Core.Services;

namespace Domain.DomainServices
{
    public class FooFighter : DomainService, IFooFighter
    {
        public Foo Create(string name, List<Bar> bars, List<int> someInts)
        {
            var orderNumber = Sequence.Next("OrderNumber");

            return new Foo(name, bars, someInts, orderNumber);
        }
    }
}