using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Diagnostics.CodeAnalysis;
using PoiEventNetwork.Data.Abstract;
using PoiEventNetwork.Data.Interface;
using PoiEventNetwork.Data.POI;

namespace PoiEventNetwork.Data.Context
{
    [ExcludeFromCodeCoverage]
    public class DataContext : DbContext, IDataContext
    {
        public DataContext()
        {
            //Configuration.LazyLoadingEnabled = false;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // No automatic adding of cascade on delete
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            
            modelBuilder.Entity<AbsTagAble>()
                .HasMany(x => x.Tags)
                .WithMany(x => x.Tagged)
                .Map(x =>
                        {
                            x.ToTable("ObjToTag");
                            x.MapLeftKey("ObjId");
                            x.MapRightKey("TagId");
                        }
                    );

            modelBuilder.Entity<Event>()
                .HasMany(x => x.Users)
                .WithMany(x => (ICollection<Event>) x.EventsAsGuest)
                .Map(x =>
                        {
                            x.ToTable("EvtToUsr");
                            x.MapLeftKey("EvtId");
                            x.MapRightKey("UsrId");
                        }
                    );
        }

        #region POI Types

        public DbSet<BasePoi> BasePois { get; set; }
        public DbSet<NormPoi> NormPois { get; set; }
        public DbSet<CityPoi> CityPois { get; set; }
        public DbSet<NgonPoi> NgonPois { get; set; }

        #endregion

        public DbSet<Message>    Messages    { get; set; }

        public DbSet<Coordinate> Coordinates { get; set; }

        public DbSet<Event>      Events      { get; set; }
        
        public DbSet<Tag>        Tags        { get; set; }

        public DbSet<User>       Users       { get; set; }
    }
}
