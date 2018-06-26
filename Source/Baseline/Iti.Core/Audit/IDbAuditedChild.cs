﻿namespace Iti.Core.Audit
{
    public interface IDbAuditedChild : IDbAudited
    {
        string AuditAggregateName { get; }
        string AuditAggregateId { get; }

        bool HasParent { get; }
    }
}