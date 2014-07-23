using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoiEventFacade
{
	public class Coordinate : IEquatable<Coordinate>
	{
		#region Constructor
		
		public Coordinate(float lat, float lon)
		{
			m_Lon = lat;
			m_Lat = lon;
		}
		
		#endregion
		
		#region Methods

	    public bool Equals(Coordinate other)
	    {
	        if (ReferenceEquals(null, other)) return false;
	        if (ReferenceEquals(this, other)) return true;
	        return m_Lon.Equals(other.m_Lon) && m_Lat.Equals(other.m_Lat);
	    }

	    public override int GetHashCode()
	    {
	        unchecked
	        {
	            return (Lon.GetHashCode()*397) ^ Lat.GetHashCode();
	        }
	    }

	    public static bool operator ==(Coordinate left, Coordinate right)
	    {
	        return Equals(left, right);
	    }

	    public static bool operator !=(Coordinate left, Coordinate right)
	    {
	        return !Equals(left, right);
	    }

	    public override bool Equals(Object obj)
		{
	        if (ReferenceEquals(null, obj)) return false;
	        if (ReferenceEquals(this, obj)) return true;
	        var other = obj as Coordinate;
	        return other != null && Equals(other);
		}
		
		#endregion
		
		#region Fields

	    private readonly float m_Lon;
	    private readonly float m_Lat;
		
		#endregion

	    #region Properties

	    public float Lon
	    {
	        get { return m_Lon; }
	    }

	    public float Lat
	    {
	        get { return m_Lat; }
	    }

	    #endregion
	}
}
