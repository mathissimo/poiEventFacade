using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using PoiEventNetwork.Data.Abstract;
using PoiEventNetwork.Data.POI;

namespace PoiEventNetwork.Data.Interface
{
    public interface IDataContext : IObjectContextAdapter, IDisposable
    {
        DbSet<BasePoi> BasePois { get; set; }
        DbSet<NormPoi> NormPois { get; set; }
        DbSet<CityPoi> CityPois { get; set; }
        DbSet<NgonPoi> NgonPois { get; set; }

        DbSet<Message> Messages { get; set; }

        DbSet<Coordinate> Coordinates { get; set; }

        DbSet<Event> Events { get; set; }

        DbSet<Tag> Tags { get; set; }

        DbSet<User> Users { get; set; }

        int SaveChanges();

        Database Database { get; }
    }
}