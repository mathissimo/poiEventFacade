using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using NLog;
using PoiEventNetwork.Data.Abstract;
using PoiEventNetwork.Data.Context;
using PoiEventNetwork.Data.Interface;

namespace PoiEventNetwork.Data
{
    /// <summary>
    /// Tag-Klasse: Erlaubt das Zuweisen von Schlüsselwörtern an Pois und Events
    /// </summary>
    public class Tag : AbsEntity
    {        
        #region Constructor

        /// <summary>
        /// Leeres Tag anlegen
        /// </summary>
        public Tag()
        {
            Tagged = new ObservableCollection<AbsTagAble>();
        }

        /// <summary>
        /// Tag mit Text anlegen
        /// </summary>
        /// <param name="text">Labeltext</param>
        public Tag(string text) : this()
        {
            Text = text;
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
        /// Labeltext
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Verknüpfung zu Pois + Events mit Tags (ORM)
        /// </summary>
        public virtual ObservableCollection<AbsTagAble> Tagged { get; private set; }

        #endregion

        #region Methods

        public override void InitDel(IDataContext ctx)
        {
            Tagged.Clear();

            ctx.Tags.Remove(this);
        }

        /// <summary>
        /// Returns a collection of tags which were retrived from the database, or created if not existing.
        /// </summary>
        /// <param name="labels"></param>
        /// <returns>A Collection of tags.</returns>
        public static IEnumerable<Tag> GetTagsFromLabels(IDataContext ctx, IEnumerable<string> labels)
        {
            foreach (var label in labels)
            {
                var tag =
                    (
                        from t in ctx.Tags
                        where t.Text.Equals(label)
                        select t
                    ).FirstOrDefault();

                if (tag == null)
                {
                    tag = ctx.Tags.Create();
                    tag.Text = label;
                }

                yield return tag;
            }
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
                int hashCode = Id.GetHashCode();
                hashCode = (hashCode*397) ^ (Text != null ? Text.GetHashCode() : 0);
				
				// TODO: Reintegrate the tagged hashs                
				//hashCode = Tagged.Aggregate(hashCode, (current, obj) => (current * 397) ^ obj.GetHashCode());

                return hashCode;
            }
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(Tag other)
        {
            if (ReferenceEquals(null, other)) return false;
            return ReferenceEquals(this, other) || string.Equals(Text, other.Text);
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
            var other = obj as Tag;
            return other != null && Equals(other);
        }

        public static bool operator ==(Tag left, Tag right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Tag left, Tag right)
        {
            return !Equals(left, right);
        }

        #endregion
    }
}
