using Domain;
using Iti.Core.DTOs;
using Iti.ValueObjects;

namespace SampleApp.Application.Dto
{
    public class ValObjHolderDto : IDto
    {
        public string Name { get; set; }

        public Address Address { get; set; }
        public PersonName PersonName { get; set; }
        public PhoneNumber PhoneNumber { get; set; }

        public ValueParent ValueParent { get; set; }
    }
}