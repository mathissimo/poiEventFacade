using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using NLog;
using PoiEventNetwork.Data.Abstract;
using PoiEventNetwork.Data.Context;
using PoiEventNetwork.Data.Interface;
using PoiEventNetwork.Data.POI;

namespace PoiEventNetwork.Data
{
    /// <summary>
    /// Event-Klasse sammelt Event-Daten und Messages. Der Event istan einen Poi gebunden.
    /// </summary>
    public class Event : AbsEntity
    {
        #region Constructor
        /// <summary>
        /// Leeren Event anlegen
        /// </summary>
        public Event()
        {
            Users = new ObservableCollection<User>();
            Messages = new ObservableCollection<Message>();
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
        /// Titel Event
        /// </summary>
        public string   Name     { get; set; }
        /// <summary>
        /// Beschreibung Event
        /// </summary>
        public string   Desc     { get; set; }
        /// <summary>
        /// Datum Event
        /// </summary>
        public DateTime Date     { get; set; }

        /// <summary>
        /// Poi-Referenz (zwingend!)
        /// </summary>
        [Required]
        public NormPoi  Location { get; set; }

        /// <summary>
        /// Event-Owner-ID für ORM!
        /// </summary>
        [ForeignKey("Creator")]
        public long CreatorId { get; set; }

        /// <summary>
        /// Event-Owner (zwingend!)
        /// </summary>
        [Required]
        public virtual User Creator { get; set; }
        /// <summary>
        /// Besucher-Liste
        /// </summary>
        public virtual ObservableCollection<User>    Users      { get; private set; }
        /// <summary>
        /// Nachrichten-Liste
        /// </summary>
        public virtual ObservableCollection<Message> Messages   { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// Set Messages-List manually. E.g. for testing.
        /// </summary>
        /// <param name="NewMessages">Overriding Messages-List</param>
        public void overrideMessages(ObservableCollection<Message> NewMessages)
        {
            Messages = NewMessages;
        }

        public void overrideUsers (ObservableCollection<User> NewUsers)
        {
            Users = NewUsers;
        }

        public override void InitDel(IDataContext ctx)
        {
            foreach (var user in Users.ToArray())
            {
                user.InitDel(ctx);
            }

            foreach (var msg in Messages.ToArray())
            {
                msg.InitDel(ctx);
            }

            ctx.Events.Remove(this);
        }

        #endregion
    }
}
