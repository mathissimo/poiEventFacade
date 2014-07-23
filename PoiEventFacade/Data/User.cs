using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using NLog;
using PoiEventNetwork.Data.Abstract;
using PoiEventNetwork.Data.Context;
using PoiEventNetwork.Data.Enums;
using PoiEventNetwork.Data.Exception;
using PoiEventNetwork.Data.Interface;
using PoiEventNetwork.Data.Tools;

namespace PoiEventNetwork.Data
{
    /// <summary>
    /// User-Klasse für Systembenutzer
    /// </summary>
    public class User : AbsEntity
    {
        #region Constructors, Destructor

        /// <summary>
        /// Einen Neuen User anlegen
        /// </summary>
        public User()
        {
            EventsAsGuest = new ObservableCollection<Event>();
            EventsAsOwner = new ObservableCollection<Event>();
            Messages      = new ObservableCollection<Message>();
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
        /// Email-Adresse: Muss eindeutig sein!
        /// </summary>
        public string Mail { get; set; }

        /// <summary>
        /// Vorname
        /// </summary>
        public string FirstName { get; set; }
        /// <summary>
        /// Nachname
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Nutzerrechte (Enumeration)
        /// </summary>
        public Rights Rights { get; set; }

        /// <summary>
        /// Besuchte Events
        /// </summary>
        public virtual ObservableCollection<Event> EventsAsGuest { get; private set; }

        /// <summary>
        /// Angelegte Events
        /// </summary>
        [InverseProperty("Creator")]
        public virtual ObservableCollection<Event> EventsAsOwner { get; private set; }

        /// <summary>
        /// Geschriebene Messages
        /// </summary>
        public virtual ObservableCollection<Message> Messages { get; private set; }

        #endregion

        #region Methods
        /// <summary>
        /// User initail in DB anlegen. Es wird geprüft, ob User schon Existent
        /// </summary>
        /// <param name="ctx"></param>
        /// <exception cref="ArgumentException">Falls Email bereits in DB ist.</exception>
        public void InitAdd(IDataContext ctx)
        {
            var users = from x in ctx.Users
                        where x.Mail.Equals(Mail)
                        select x;

            if (Enumerable.Any(users))
            {
                throw new ArgumentException("A user with this mail already exists.");
            }

            ctx.Users.Add(this);
        }

        /// <summary>
        /// Löschen des Users aus der DB. Und damit verbundener Events und Messages
        /// </summary>
        /// <param name="ctx"></param>
        public override void InitDel(IDataContext ctx)
        {
			foreach (var msg in Messages.ToArray())
			{
				msg.InitDel(ctx);
			}

            foreach (var ev in EventsAsOwner.ToArray())
            {
                ev.InitDel(ctx);
            }

			ctx.Users.Remove(this);
        }

        public void ThrowIfNoAdminRights()
        {
            if (!Rights.HasFlag(Rights.Administer))
                ExceptionHelper.ThrowArgumentException(s_Logger, "", new AuthorizationException(Id));
        }

        #endregion
    }
}
