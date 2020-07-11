using Domain;
using Iti.Baseline.Core.DTOs;
using Iti.Baseline.ValueObjects;

namespace SampleApp.Application.Dto
{
    public class ValObjHolderDto : IDto
    {
        public string Name { get; set; }

        public SimpleAddress SimpleAddress { get; set; }
        public SimplePersonName SimplePersonName { get; set; }
        public PhoneNumber PhoneNumber { get; set; }

        public ValueParent ValueParent { get; set; }
    }
}