using System.Collections.Generic;
using Domain;
using FooSampleApp.Application.Dto;
using Iti.ValueObjects;

namespace FooSampleApp.Application.Interfaces
{
    public interface IFooAppService
    {
        FooReferenceDto ReferenceFor(FooId id);
        FooSummaryDto SummaryFor(FooId id);
        FooJunkDto JunkFor(FooId id);
        FooDto Get(FooId id);
        List<FooDto> GetList();
        FooId CreateFoo(string name, List<Bar> bars);
        void SetName(FooId id, string newName);
        void RemoveBar(FooId id, string name);
        void AddBar(FooId id, string name);
        void SetAllBarNames(FooId id, string name);
        void SetAddress(FooId id, Address address);
    }
}