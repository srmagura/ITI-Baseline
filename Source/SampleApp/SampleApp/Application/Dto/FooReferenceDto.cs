using Domain;
using Iti.Baseline.Core.DTOs;

namespace SampleApp.Application.Dto
{
    public class FooReferenceDto : IDto
    {
        public FooId Id { get; set; }
        public string Name { get; set; }
    }
}