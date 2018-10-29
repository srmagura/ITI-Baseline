using Domain;

namespace SampleApp.Application.Dto
{
    public class FooSummaryDto
    {
        public FooId Id { get; set; }
        public string Name { get; set; }
        public int BarCount { get; set; }
        public string SomeInts { get; set; }
    }
}