using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using NLog;
using PoiEventNetwork.Data.Context;
using PoiEventNetwork.Data.Interface;

namespace PoiEventNetwork.Data.POI
{
    /// <summary>
    /// Poi with polygon as geofence
    /// </summary>
    public class NgonPoi : BasePoi, IEquatable<NgonPoi>
    {
        #region Constructor
        /// <summary>
        /// Empty Polygon Poi
        /// </summary>
        public NgonPoi() : base()
        {
            Polygon = new ObservableCollection<Coordinate>();
        }
        /// <summary>
        /// Polygon Poi with name, geofence, tags
        /// </summary>
        /// <param name="name">Name (distinct to class!)</param>
        /// <param name="coordinates">Geofence as ObserveableCollection of Coordinates</param>
        /// <param name="tags">Tags as ObservableCollection of Strings</param>
        public NgonPoi(string name, IEnumerable<Coordinate> coordinates, IEnumerable<Tag> tags)
            : base(name, tags)
        {
            Polygon = new ObservableCollection<Coordinate>(coordinates);
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
        /// Geofence
        /// </summary>
        public virtual ObservableCollection<Coordinate> Polygon { get; private set; }

        #endregion

        #region Methods

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public bool Equals(NgonPoi other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return base.Equals(other) && Equals(Polygon, other.Polygon);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (base.GetHashCode() * 397) ^ (Polygon != null ? Polygon.GetHashCode() : 0);
            }
        }

        public static bool operator ==(NgonPoi left, NgonPoi right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(NgonPoi left, NgonPoi right)
        {
            return !Equals(left, right);
        }

        #endregion


    }
}
