using Iti.Baseline.Core.DTOs;

namespace SampleApp.Application.Dto
{
    public class FooJunkDto : IDto
    {
        public FooReferenceDto Ref { get; set; }
        public FooSummaryDto Summary { get; set; }
    }
}