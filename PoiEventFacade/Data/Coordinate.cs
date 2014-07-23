using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NLog;
using PoiEventNetwork.Data.Abstract;
using PoiEventNetwork.Data.Context;
using PoiEventNetwork.Data.Interface;

namespace PoiEventNetwork.Data
{
    /// <summary>
    /// Geo-Koordinate mit Lat/Lon
    /// </summary>
    public class Coordinate : AbsEntity, IEquatable<Coordinate>
	{
		#region Constructor
		/// <summary>
		/// Coordinate mit Lat, Lon anlegen
		/// </summary>
		/// <param name="lat">Latitude (-90°..+90°)</param>
		/// <param name="lon">Longitude (-180°..+180°)</param>
        /// <exception cref="ValidationException">wenn Lat/Lon außerhalb definierten Bereiches</exception>
		public Coordinate(float lat, float lon)
		{
			Lat = lat;
			Lon = lon;

            Validator.ValidateObject(this, new ValidationContext(this), true);
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
        /// Longitude (-180°..+180°)
        /// </summary>
        [Range(-180.0f, 180.0f)]
        public float Lon { get; private set; }
        
        /// <summary>
        /// Latitude (-90°..+90°)
        /// </summary>
        [Range(- 90.0f,  90.0f)]
        public float Lat { get; private set; }

	    #endregion		

		#region Methods

        /// <summary>
        /// Serves as a hash function for a particular type. 
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object"/>.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override int GetHashCode()
	    {
	        unchecked
	        {
	            return (Lon.GetHashCode()*397) ^ Lat.GetHashCode();
	        }
	    }

        public override void InitDel(IDataContext ctx)
        {
            ctx.Coordinates.Remove(this);
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(Coordinate other)
	    {
	        if (ReferenceEquals(null, other)) return false;
	        if (ReferenceEquals(this, other)) return true;
	        return Lon.Equals(other.Lon) && Lat.Equals(other.Lat);
	    }

        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// true if the specified object  is equal to the current object; otherwise, false.
        /// </returns>
        /// <param name="obj">The object to compare with the current object. </param><filterpriority>2</filterpriority>
        public override bool Equals(Object obj)
		{
	        if (ReferenceEquals(null, obj)) return false;
	        if (ReferenceEquals(this, obj)) return true;
	        var other = obj as Coordinate;
	        return other != null && Equals(other);
		}

	    public static bool operator ==(Coordinate left, Coordinate right)
	    {
	        return Equals(left, right);
	    }

	    public static bool operator !=(Coordinate left, Coordinate right)
	    {
	        return !Equals(left, right);
	    }		

		#endregion
	}
}
