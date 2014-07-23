using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoiEventFacade
{

    public class Poi : IEquatable<Poi>
    {
        public bool Equals(Poi other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(Name, other.Name) && Equals(Tags, other.Tags);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Poi) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Name != null ? Name.GetHashCode() : 0)*397) ^ (Tags != null ? Tags.GetHashCode() : 0);
            }
        }

        public static bool operator ==(Poi left, Poi right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Poi left, Poi right)
        {
            return !Equals(left, right);
        }

        public string           Name { get; set; }
        public HashSet<string>  Tags { get; set; } 
    }
}
