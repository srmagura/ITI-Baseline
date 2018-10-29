using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain;
using Iti.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using SampleApp.Application.Dto;
using SampleApp.Application.Interfaces;

namespace DataContext.Repositories
{
    public class EfFooQueries : Queries<SampleDataContext>, IFooQueries
    {
        public FooReferenceDto ReferenceFor(FooId id)
        {
            return Context.Foos
                .Where(p => p.Id == id.Guid)
                .ProjectTo<FooReferenceDto>()
                .FirstOrDefault();
        }

        public FooSummaryDto SummaryFor(FooId id)
        {
            return Context.Foos
                .Where(p => p.Id == id.Guid)
                .ProjectTo<FooSummaryDto>()
                .FirstOrDefault();
        }

        public FooJunkDto JunkFor(FooId id)
        {
            return Context.Foos
                .Where(p => p.Id == id.Guid)
                .ProjectTo<FooJunkDto>()
                .FirstOrDefault();
        }

        public FooDto Get(FooId id)
        {
            return Context.Foos
                .Include(p => p.Bars)
                .Where(p => p.Id == id.Guid)
                .ProjectTo<FooDto>()
                .FirstOrDefault();
        }

        public List<FooDto> GetList()
        {
            return Context.Foos
                    .Include(p => p.Bars)
                    .OrderByDescending(p => p.DateCreatedUtc)
                    .Take(10)
                    .ProjectTo<FooDto>()
                    .ToList()
                ;
        }
    }
}