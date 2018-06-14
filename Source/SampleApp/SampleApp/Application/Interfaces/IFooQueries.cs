using System.Collections.Generic;
using Domain;
using FooSampleApp.Application.Dto;

namespace FooSampleApp.Application.Interfaces
{
    public interface IFooQueries
    {
        FooReferenceDto ReferenceFor(FooId id);
        FooSummaryDto SummaryFor(FooId id);
        FooJunkDto JunkFor(FooId id);
        FooDto Get(FooId id);
        List<FooDto> GetList();
    }
}