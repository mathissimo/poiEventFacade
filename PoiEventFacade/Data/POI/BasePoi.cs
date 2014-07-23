using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Security.Authentication;
using NLog;
using PoiEventNetwork.Data.Abstract;
using PoiEventNetwork.Data.Context;
using PoiEventNetwork.Data.Exception;
using PoiEventNetwork.Data.Interface;
using PoiEventNetwork.Data.Tools;

namespace PoiEventNetwork.Data.POI
{
    /// <summary>
    /// Einfachster instanzierbarer POI. Ergänzt den Namen.
    /// </summary>
    public abstract class BasePoi : AbsTagAble
    {
        #region Constructor, Destructor
        /// <summary>
        /// Create empty BasePoi
        /// </summary>
        public BasePoi() : base()
        {
        }
        /// <summary>
        /// Create BasePoi with name and tags
        /// </summary>  
        /// <param name="name">Name: Distinct Value for class!</param>
        /// <param name="tags">Poi-Tags</param>
        public BasePoi(string name, IEnumerable<Tag> tags) : base(tags)
        {
            Name = name;
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
        /// Name: Distinct Value for class!
        /// </summary>
        [Required]
        public string Name { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Initialize and add Poi to DB
        /// </summary>
        /// <param name="ctx">DB-Context</param>
        /// <param name="user">Poi Owner</param>
        /// <exception cref="ArgumentException">Falls der Nutzer nicht in DB gefunden wird.</exception>
        /// <exception cref="AuthorizationException">Falls der Nutzer kein Admin ist.</exception>
        public void InitAdd(IDataContext ctx, User user)
        {
            if (user == null)
                ExceptionHelper.ThrowArgumentException(s_Logger, "", new ArgumentNullException("user"));

            user.ThrowIfNoAdminRights();

            if (Enumerable.Any(ctx.BasePois, item => item.Name.Equals(Name)))
            {
                ExceptionHelper.ThrowArgumentException(s_Logger, "", new ArgumentException("A point of interest with this name already exists."));
            }

            ctx.BasePois.Add(this);
        }

        /// <summary>
        /// Deletes Poi and adjactant Tags
        /// </summary>
        /// <param name="ctx"></param>
        public override void InitDel(IDataContext ctx)
        {
            InitDelTags(Tags);
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(BasePoi other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            bool equals = string.Equals(Name, other.Name);

            return equals;
        }

        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// true if the specified object  is equal to the current object; otherwise, false.
        /// </returns>
        /// <param name="obj">The object to compare with the current object. </param><filterpriority>2</filterpriority>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;

            BasePoi poi = obj as BasePoi;
            return poi != null && Equals((BasePoi) obj);
        }

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
                return ((Name != null ? Name.GetHashCode() : 0)*397) ^ (Tags != null ? Tags.GetHashCode() : 0);
            }
        }

        #region Operators

        public static bool operator ==(BasePoi left, BasePoi right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(BasePoi left, BasePoi right)
        {
            return !Equals(left, right);
        }

        #endregion

        #endregion
    }
}
