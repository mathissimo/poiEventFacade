using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NLog;
using PoiEventNetwork.Data.Interface;

namespace PoiEventNetwork.Data.Abstract
{
    public abstract class AbsEntity
    {
        #region Fields

        /// <summary>
        /// The static logger instance for this class.
        /// </summary>
        private static Logger s_Logger;

        #endregion

        #region Properties

        [Key]
        public long Id { get; protected set; }

        #endregion

        #region Methods

        public abstract void InitDel(IDataContext ctx);

        #endregion
    }
}
