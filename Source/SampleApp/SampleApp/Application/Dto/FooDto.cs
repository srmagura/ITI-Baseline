using System.Collections.Generic;
using Domain;
using Iti.Core.DTOs;
using Iti.ValueObjects;

namespace SampleApp.Application.Dto
{
    public class FooDto : IDto
    {
        public FooId Id { get; set; }
        public string Name { get; set; }
        public decimal SomeMoney { get; set; }
        public SimpleAddress SimpleAddress { get; set; }
        public SimplePersonName SimplePersonName { get; set; }
        public PhoneNumber PhoneNumber { get; set; }
        public long SomeNumber { get; set; }

        public List<BarDto> Bars { get; set; }

        public string SomeInts { get; set; }
    }
}