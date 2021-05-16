using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace ITI.DDD.Domain.ValueObjects
{
    [Owned]
    public abstract record ValueObject
    {
        public bool? HasValue { get; private init; } = true;
    }
}