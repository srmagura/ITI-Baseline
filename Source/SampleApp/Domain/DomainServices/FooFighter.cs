using System.Collections.Generic;
using Iti.Baseline.Core.Sequences;
using Iti.Baseline.Core.Services;

namespace Domain.DomainServices
{
    public class FooFighter : DomainService, IFooFighter
    {
        private readonly ISequenceResolver _seq;

        public FooFighter(ISequenceResolver seq)
        {
            _seq = seq;
        }

        public Foo Create(string name, List<Bar> bars, List<int> someInts)
        {
            var orderNumber = _seq.GetNextValue("OrderNumber");

            return new Foo(name, bars, someInts, orderNumber);
        }
    }
}