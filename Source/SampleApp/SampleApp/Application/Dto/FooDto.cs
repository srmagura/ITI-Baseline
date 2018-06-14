using System.Collections.Generic;
using Domain;
using Iti.ValueObjects;

namespace FooSampleApp.Application.Dto
{
    public class FooDto
    {
        public FooId Id { get; set; }
        public string Name { get; set; }
        public decimal SomeMoney { get; set; }
        public Address Address { get; set; }
        public long SomeNumber { get; set; }

        public List<BarDto> Bars { get; set; }

        public List<int> SomeInts { get; set; }
    }
}