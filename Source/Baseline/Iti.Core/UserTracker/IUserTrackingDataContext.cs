using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Iti.Core.UserTracker
{
    public interface IUserTrackingDataContext : IDisposable
    {
        DbSet<UserTrack> UserTracks { get; }
        DatabaseFacade Database { get; }
        int SaveChanges();
    }
}