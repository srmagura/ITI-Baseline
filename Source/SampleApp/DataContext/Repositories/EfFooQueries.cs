using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain;
using Iti.Core.DTOs;
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
                // .Include(p => p.Bars)
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