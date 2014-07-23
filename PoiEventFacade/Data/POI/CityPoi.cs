using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using PoiEventNetwork.Data.Context;
using PoiEventNetwork.Data.Interface;

namespace PoiEventNetwork.Data.POI
{
    /// <summary>
    /// Poi with City-Address
    /// </summary>
    public class CityPoi : NormPoi
    {
        #region Constructor
        /// <summary>
        /// Create empty CityPoi
        /// </summary>
        public CityPoi() : base()
        {

        }

        /// <summary>
        /// Crate citiy Poi with Name and Tags
        /// </summary>
        /// <param name="name">Name (Distinct to class!)</param>
        /// <param name="labels">Tags as ObserveableCollection</param>
        public CityPoi(string name, IEnumerable<Tag> tags)
            : base(name, tags)
        {

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
        /// Steet
        /// </summary>
        public string Street { get; set; }
        /// <summary>
        /// City
        /// </summary>
        public string City { get; set; }

        #endregion

    }
}
