using System;
using System.Collections.Generic;
using System.Text;
using TestApp.Domain.Identities;

namespace TestApp.Application.Dto
{
    public class LtcPharmacyDto
    {
        public LtcPharmacyId Id { get; set; }
        public string? Name { get; set; }
    }
}
