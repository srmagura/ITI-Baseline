using Domain;
using Iti.Core.DTOs;

namespace SampleApp.Application.Dto
{
    public class FooSummaryDto : IDto
    {
        public FooId Id { get; set; }
        public string Name { get; set; }
        public int BarCount { get; set; }
        public string SomeInts { get; set; }
    }
}