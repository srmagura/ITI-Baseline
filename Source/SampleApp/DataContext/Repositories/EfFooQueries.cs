using System.Collections.Generic;
using System.Linq;
using Domain;
using Iti.Baseline.Core.DTOs;
using Iti.Baseline.Core.Repositories;
using Iti.Baseline.Core.UnitOfWorkBase.Interfaces;
using SampleApp.Application.Dto;
using SampleApp.Application.Interfaces;

namespace DataContext.Repositories
{
    public class EfFooQueries : Queries<SampleDataContext>, IFooQueries
    {
        public EfFooQueries(IUnitOfWork uow) : base(uow)
        {
        }

        //

        public FooReferenceDto ReferenceFor(FooId id)
        {
            return Context.Foos
                .Where(p => p.Id == id.Guid)
                .ProjectToDto<FooReferenceDto>();
        }

        public FooSummaryDto SummaryFor(FooId id)
        {
            return Context.Foos
                .Where(p => p.Id == id.Guid)
                .ProjectToDto<FooSummaryDto>();
        }

        public FooJunkDto JunkFor(FooId id)
        {
            return Context.Foos
                .Where(p => p.Id == id.Guid)
                .ProjectToDto<FooJunkDto>();
        }

        public FooDto Get(FooId id)
        {
            return Context.Foos
                .Where(p => p.Id == id.Guid)
                .ProjectToDto<FooDto>();
        }

        public List<FooDto> GetList()
        {
            return Context.Foos
                // .Include(p => p.Bars)
                .OrderByDescending(p => p.DateCreatedUtc)
                .Take(10)
                .ProjectToDtoList<FooDto>();
                ;
        }

    }
}