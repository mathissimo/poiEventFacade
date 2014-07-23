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
    /// Nachrichten-Klasse: Nachricht besteht aus Titel, Text und zugeordnetem
    /// Autor und Event
    /// </summary>
    public class Message : AbsEntity
    {
        #region Fields

        /// <summary>
        /// The static logger instance for this class.
        /// </summary>
        private static Logger s_Logger;

        #endregion

        #region Properties
        /// <summary>
        /// Titel Nachricht
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Nachrichten-Text
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Autor der Nachricht
        /// </summary>
        public User User { get; set; }
        /// <summary>
        /// Event, dem die Nachricht zugeordnet ist
        /// </summary>
        public Event Event { get; set; }

        #endregion

        #region Methods

        public override void InitDel(IDataContext ctx)
        {
            ctx.Messages.Remove(this);
        }

        #endregion

    }
}
