using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Security.Authentication;
using NLog;
using PoiEventNetwork.Data.Context;
using PoiEventNetwork.Data.Interface;

namespace PoiEventNetwork.Data.POI
{
    /// <summary>
    /// Normal Poi with Events
    /// </summary>
    public class NormPoi : BasePoi
    {
        #region Constructor
        /// <summary>
        /// Create empty Normal Poi
        /// </summary>
        public NormPoi()
        {
            Events = new ObservableCollection<Event>();
        }
        /// <summary>
        /// Create Normal Poi
        /// </summary>
        /// <param name="name">Name (distinct to instance!)</param>
        /// <param name="tags">Tags as ObserveableCollection</param>
        public NormPoi(string name, IEnumerable<Tag> tags) : base(name, tags)
        {
            Events = new ObservableCollection<Event>();
        }

        #endregion

        #region Fields

        /// <summary>
        /// The static logger instance for this class.
        /// </summary>
        private static Logger s_Logger = NLog.LogManager.GetCurrentClassLogger();

        #endregion

        #region Properties
        /// <summary>
        /// Location
        /// </summary>
        public Coordinate Location { get; set; }
        /// <summary>
        /// Events connected with Poi
        /// </summary>
        public virtual ObservableCollection<Event> Events { get; protected set; }

        #endregion

        #region Methods

        /// <summary>
        /// Set Event-List manually. E.g. for testing.
        /// </summary>
        /// <param name="NewEvents">Overriding Event-List</param>
        public void overrideEvents(ObservableCollection<Event> NewEvents)
        {
            Events = NewEvents;
        }

        public override void InitDel(IDataContext ctx)
        {
            base.InitDel(ctx);

            foreach (var item in Events)
            {
                item.InitDel(ctx);
            }

            ctx.NormPois.Remove(this);
        }

        #endregion

    }
}
