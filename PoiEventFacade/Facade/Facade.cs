using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Authentication;
using Microsoft.Practices.Unity;
using PoiEventNetwork.Data;
using PoiEventNetwork.Data.Enums;
using PoiEventNetwork.Data.Exception;
using PoiEventNetwork.Data.Interface;
using PoiEventNetwork.Data.POI;
using PoiEventNetwork.Data.Tools;
using PoiEventNetwork.Facade.Interface;
using NLog;

namespace PoiEventNetwork.Facade
{
    /// <summary>
    /// </summary>
    public class Facade : IPoiEventFacade, IDisposable
    {
        #region Constructor

        /// <summary>
        /// 
        /// </summary>
        public Facade()
        {
            Context = Resolver.Resolve<IDataContext>();
        }

        #endregion

        private static Logger s_Logger = LogManager.GetLogger("Facade");

        #region Properties

        /// <summary>
        ///     The context which manages the connection to the database and the entity states.
        /// </summary>
        public IDataContext Context { get; protected set; }

        #endregion

        #region Methods

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            Context.Dispose();
        }

        /// <summary>
        ///     Methode zum Erzeugen eines neuen einfachen Point of Interest (POI).
        /// </summary>
        /// <param name="userId">ID des Nutzers, der den POI anlegt.</param>
        /// <param name="name">Eindeutiger Name des POI.</param>
        /// <param name="labels">Schlagworte, die den POI charakterisieren.</param>
        /// <param name="pos">Geokoordinate in Grad.</param>
        /// <exception cref="AuthenticationException">Falls der Nutzer kein Admin ist.</exception>
        /// <exception cref="ArgumentException">Falls kein Nutzer mit der ID existiert.</exception>
        /// <exception cref="ArgumentException">Falls Geokoordinaten außerhalb des gültigen Wertebereiches liegen.</exception>
        public void CreateBasePoi(long userId, string name, IEnumerable<string> labels, Coordinate pos)
        {
            User user = GenericSearcher.GetFirstOrDefaultByCriteria(Context.Users, user1 => user1.Id == userId);

            IEnumerable<Tag> tags = Tag.GetTagsFromLabels(Context, labels);

            var poi = new NormPoi {Name = name};


            poi.InitAddTags(tags.ToArray());

            poi.InitAdd(Context, user);

            Context.SaveChanges();
        }

        /// <summary>
        ///     Methode zum Erzeugen eines neuen City Point of Interest (POI).
        /// </summary>
        /// <param name="userId">ID des Nutzers, der den POI anlegt.</param>
        /// <param name="name">eindeutiger Name des POI.</param>
        /// <param name="labels">Schlagworte, die den POI charakterisieren.</param>
        /// <param name="street">Strasse, in dem der POI liegt.</param>
        /// <param name="city">Stadt, in dem der POI liegt.</param>
        /// <param name="pos">Geokoordinate in Grad.</param>
        /// <exception cref="AuthenticationException">Falls der Nutzer kein Admin ist.</exception>
        /// <exception cref="ArgumentException">Falls kein Nutzer mit der ID existiert.</exception>
        /// <exception cref="ArgumentException">Falls Geokoordinaten außerhalb des gültigen Wertebereiches liegen.</exception>
        public void CreateCityPoi(long userId, string name, IEnumerable<string> labels, string street, string city,
                                  Coordinate pos)
        {
            User user = GenericSearcher.GetFirstOrDefaultByCriteria(Context.Users, user1 => user1.Id == userId);

            IEnumerable<Tag> tags = Tag.GetTagsFromLabels(Context, labels);
            
            if (pos == null)
                ExceptionHelper.ThrowArgumentException(s_Logger, "", new ArgumentNullException("pos"));

            var poi = new CityPoi(name, tags) {Street = street, City = city};

            poi.InitAdd(Context, user);

            Context.SaveChanges();
        }

        /// <summary>
        ///     Methode zum Erzeugen eines neuen Polygon Point of Interest (POI).
        /// </summary>
        /// <param name="userId">ID des Nutzers, der den POI anlegt.</param>
        /// <param name="name">eindeutiger Name des POI.</param>
        /// <param name="labels">Schlagworte, die den POI charakterisieren.</param>
        /// <param name="polygon">Geordnete Liste der Geokoordinaten, diese beschreiben eine Fläche. Die einzelnen Koordinaten muessen dabei gueltige Koordinaten sein.</param>
        /// <exception cref="AuthenticationException">Falls der Nutzer kein Admin ist.</exception>
        /// <exception cref="ArgumentException">Falls kein Nutzer mit der ID existiert.</exception>
        /// <exception cref="ArgumentException">Falls Geokoordinaten außerhalb des gültigen Wertebereiches liegen.</exception>
        public void CreatePolygonPoi(long userId, string name, IEnumerable<string> labels, Coordinate[] polygon)
        {
            User user = GenericSearcher.GetFirstOrDefaultByCriteria(Context.Users, user1 => user1.Id == userId);

            IEnumerable<Tag> tags = Tag.GetTagsFromLabels(Context, labels);

            var poi = new NgonPoi(name, polygon, tags);

            poi.InitAdd(Context, user);

            Context.SaveChanges();
        }

        /// <summary>
        ///     Methode zum Löschen eines Point of Interest.
        /// </summary>
        /// <param name="userId">ID des Nutzers, der den POI anlegt.</param>
        /// <param name="name">Eindeutige Name des Poi der gelöscht werden soll.</param>
        /// <exception cref="AuthenticationException">Falls der Nutzer kein Admin ist.</exception>
        /// <exception cref="ArgumentException">Falls kein Nutzer mit der ID existiert.</exception>
        public void DeletePoi(long userId, string name)
        {
            User usr = GenericSearcher.GetFirstOrDefaultByCriteria(Context.Users, user => user.Id == userId);
            NormPoi poi = GenericSearcher.GetFirstOrDefaultByCriteria(Context.NormPois, basePoi => basePoi.Name == name);

            usr.ThrowIfNoAdminRights();

            poi.InitDel(Context);

            Context.SaveChanges();
        }

        /// <summary>
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="name"></param>
        /// <param name="label"></param>
        public void AddPoiTag(long userId, string name, string label)
        {
            User usr = GenericSearcher.GetFirstOrDefaultByCriteria(Context.Users, user => user.Id == userId);
            NormPoi poi = GenericSearcher.GetFirstOrDefaultByCriteria(Context.NormPois, bpoi => bpoi.Name == name);

            Tag tag = Tag.GetTagsFromLabels(Context, new[] {label}).Single();

            usr.ThrowIfNoAdminRights();

            if (!poi.Tags.Contains(tag))
                poi.Tags.Add(tag);

            Context.SaveChanges();
        }

        /// <summary>
        ///     Methode zum Löschen eines Point of Interest.
        /// </summary>
        /// <param name="userId">ID des Nutzers, der den POI anlegt.</param>
        /// <param name="name">eindeutiger Name des POI.</param>
        /// <param name="label">Schlagwort, das zur Schlagwortbeschreibung des POI hinzugefuegt werden soll.</param>
        /// <exception cref="ArgumentException">Falls kein solches Schalgwort existiert.</exception>
        /// <exception cref="ArgumentException">Falls kein Nutzer mit der ID existiert.</exception>
        /// <exception cref="ArgumentException">Falls kein Poi mit dem Namen existiert.</exception>
        /// <exception cref="AuthenticationException">Falls der Nutzer kein Admin ist.</exception>
        public void DeletePoiTag(long userId, string name, string label)
        {
            User usr = GenericSearcher.GetFirstOrDefaultByCriteria(Context.Users, user => user.Id == userId);
            NormPoi poi = GenericSearcher.GetFirstOrDefaultByCriteria(Context.NormPois, basePoi => basePoi.Name == name);
            Tag tag = GenericSearcher.GetFirstOrDefaultByCriteria(Context.Tags, tag1 => tag1.Text == label);

            usr.ThrowIfNoAdminRights();

            poi.InitDelTags(new[] {tag});

            Context.SaveChanges();
        }

        /// <summary>
        ///     Gibt alle POIs zurück, die das Schlagwort beinhalten.
        ///     Achten Sie hier auf eine performante Implementierung.
        /// </summary>
        /// <param name="label">Schlagwort.</param>
        /// <returns>Menge aller Pois, die das Schlagwort beinhalten.</returns>
        /// <remarks>
        ///     Performance considerations:
        ///     Any tag entity keeps a observable collection of all objects it tagged.
        ///     This collection is virtual; Meaning it is lazy loading. Here the final load is executed at the call of ToArray().
        ///     The actual query to the database is binary (compiled) sql, executing at maximum speed.
        /// </remarks>
        public IEnumerable<BasePoi> GetPoiByTag(string label)
        {
            Tag tag = GenericSearcher.GetFirstOrDefaultByCriteria(Context.Tags, tag1 => tag1.Text.Equals(label));

            return tag.Tagged.ToArray().Cast<BasePoi>();
        }

        /// <summary>
        ///     Gibt den Poi mit dem angefragenten Namen zurueck.
        /// </summary>
        /// <param name="name">Name des Pois der zurueckgegeben werden soll.</param>
        /// <returns>Existiert kein Poi mit dem Namen, so wird null zurueckgegeben.</returns>
        public BasePoi GetPoi(string name)
        {
            return GenericSearcher.TryGetFirstOrDefaultByCriteria(Context.BasePois, basePoi => basePoi.Name.Equals(name));
        }

        /// <summary>
        ///     Beim Erzeugen soll intern auch das Anlegedatum (siehe date Attribut in Event) gespeichert werden.
        /// </summary>
        /// <param name="userId">Id des Nutzers, der den Event anlegt.</param>
        /// <param name="poiName">Name des Poi, zu dem der Event hinzugefügt wird.</param>
        /// <param name="title">Titel des Events.</param>
        /// <param name="description">Textuelle beschreibung des Events.</param>
        /// <returns>Id, die den Event eindeutig identifiziert.</returns>
        /// <exception cref="ArgumentException">Falls kein User mit der id existiert.</exception>
        public long CreateEvent(long userId, string poiName, string title, string description)
        {
            User user = GenericSearcher.GetFirstOrDefaultByCriteria(Context.Users, user1 => user1.Id == userId);
            NormPoi bpoi = GenericSearcher.GetFirstOrDefaultByCriteria(Context.NormPois, bpoi1 => bpoi1.Name == poiName);

            var evnt = Context.Events.Create();

            evnt.Creator = user;
            evnt.Date = DateTime.Now;
            evnt.Desc = description;
            evnt.Name = title;
            evnt.Location = bpoi;
                

            user.EventsAsOwner.Add(evnt);

            Context.Events.Add(evnt);

            if (Context.SaveChanges() == 0)
                ExceptionHelper.ThrowArgumentException(s_Logger, "", new EntityException("Failed to save the event."));

            return evnt.Id;
        }

        /// <summary>
        /// </summary>
        /// <param name="userId">
        ///     Id des Nutzer, der den Event loeschen will. Ist dies kein
        ///     Adminstrator oder nicht der Nutzer, der den Event angelegt hat, so wird eine
        ///     AuthorizationException geworfen.
        ///     Wird ein Event geloescht, so werden auch alle Nachrichten
        ///     zu dem Event geloescht.
        /// </param>
        /// <param name="eventId">Id des Events.</param>
        /// <exception cref="ArgumentException">Falls kein Event mit dem Namen existiert.</exception>
        /// <exception cref="ArgumentException">Falls kein user mit der id existiert.</exception>
        public void DeleteEvent(long userId, long eventId)
        {
            User user = GenericSearcher.GetFirstOrDefaultByCriteria(Context.Users, user1 => user1.Id == userId);
            Event evnt = GenericSearcher.GetFirstOrDefaultByCriteria(Context.Events, evnt1 => evnt1.Id == eventId);

            if (evnt.Creator.Id != user.Id)
                user.ThrowIfNoAdminRights();

            evnt.InitDel(Context);

            Context.SaveChanges();
        }

        /// <summary>
        ///     Retrieval Methode zum Finden von allen Events zu den Pois.
        /// </summary>
        /// <param name="poiName">name des pois.</param>
        /// <returns>Alle Events, die zum Poi hinzugefuegt worden sind.</returns>
        /// <exception cref="ArgumentException">falls kein Poi mit dem Namen existiert.</exception>
        public Event[] FindEventsForPoi(string poiName)
        {
            NormPoi poi = GenericSearcher.GetFirstOrDefaultByCriteria(Context.NormPois,
                                                                      basePoi => basePoi.Name == poiName);

            return poi.Events.ToArray();
        }

        /// <summary>
        ///     Nutzer koennen sich zu Events anmelden.
        /// </summary>
        /// <param name="userId">Nutzer Id.</param>
        /// <param name="eventId">Event Id.</param>
        /// <exception cref="ArgumentException">Falls kein Event mit dem Namen existiert.</exception>
        /// <exception cref="ArgumentException">Falls kein user mit der id existiert.</exception>
        public void SubscribeToEvent(long userId, long eventId)
        {
            User usr = GenericSearcher.GetFirstOrDefaultByCriteria(Context.Users, user => user.Id == userId);
            Event evt = GenericSearcher.GetFirstOrDefaultByCriteria(Context.Events, evnt => evnt.Id == eventId);

            evt.Users.Add(usr);

            Context.SaveChanges();
        }

        /// <summary>
        ///     Finde alle Events fuer die sich der Nutzer mit der userId registriert hat.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>Menge der Events.</returns>
        /// <exception cref="ArgumentException">Falls kein user mit der id existiert.</exception>
        public Event[] FindSubscribedEvents(long userId)
        {
            User user = GenericSearcher.GetFirstOrDefaultByCriteria(Context.Users, user1 => user1.Id == userId);

            return user.EventsAsGuest.ToArray();
        }

        /// <summary>
        ///     Finde alle Events, die der Nutzer angelegt hat.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>Menge der Events.</returns>
        /// <exception cref="ArgumentException">Falls kein user mit der id existiert.</exception>
        public Event[] FindOwnedEvents(long userId)
        {
            User user = GenericSearcher.GetFirstOrDefaultByCriteria(Context.Users, user1 => user1.Id == userId);

            return user.EventsAsOwner.ToArray();
        }

        /// <summary>
        ///     Fuege eine Nachricht zu einem Event hinzu.
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="userId"></param>
        /// <param name="title"></param>
        /// <param name="content"></param>
        /// <exception cref="ArgumentException">Falls kein Event mit dem Namen existiert.</exception>
        /// <exception cref="ArgumentException">Falls kein user mit der id existiert.</exception>
        public void AddMessage(long eventId, long userId, string title, string content)
        {
            Event evnt = GenericSearcher.GetFirstOrDefaultByCriteria(Context.Events, @event => @event.Id == eventId);
            User user = GenericSearcher.GetFirstOrDefaultByCriteria(Context.Users, user1 => user1.Id == userId);


            var msg = Context.Messages.Create();
            msg.Event = evnt;
            msg.User = user;
            msg.Name = title;
            msg.Text = content;

            evnt.Messages.Add(msg);

            Context.SaveChanges();
        }

        /// <summary>
        ///     Retrieval Methode zum Finden aller Nachrichten zu einem Event.
        /// </summary>
        /// <param name="eventId"></param>
        /// <returns>Sortiert nach dem Anlegedatum der Nachricht (je jünger die Nachricht, desto spaeter steht sie in der Liste.)</returns>
        public Message[] GetMessages(long eventId)
        {
            return
                GenericSearcher.GetFirstOrDefaultByCriteria(Context.Events, @event => @event.Id == eventId)
                               .Messages.ToArray();
        }

        /// <summary>
        ///     Erzeugt einen Nutzer.
        /// </summary>
        /// <param name="lastName"></param>
        /// <param name="firstName"></param>
        /// <param name="email"></param>
        /// <returns>Id des Nutzers.</returns>
        /// <exception cref="ArgumentException">Falls ein Nutzer mit der E-Mail bereits existiert.</exception>
        public long CreateUser(string lastName, string firstName, string email)
        {
            User user = Context.Users.Create();

            user.FirstName = firstName;
            user.LastName = lastName;
            user.Rights = Rights.Registered;
            user.Mail = email;

            user.InitAdd(Context);

            Context.SaveChanges();

            return user.Id;
        }

        /// <summary>
        ///     Fuegt zu einem Nutzer die admin Rechte hinzu.
        /// </summary>
        /// <param name="userId"></param>
        /// <exception cref="ArgumentException">Falls kein user mit der id existiert.</exception>
        public void SetAdminRole(long userId)
        {
            User usr = GenericSearcher.TryGetFirstOrDefaultByCriteria(Context.Users, user => user.Id == userId);

            if (usr == null)
                ExceptionHelper.ThrowArgumentException(s_Logger, "", new ArgumentException("No user exists for the given parameter.", "userId"));

            usr.Rights |= Rights.Administer;

            Context.SaveChanges();
        }

        public bool HasAdminRole(long userId)
        {
            User usr = GenericSearcher.TryGetFirstOrDefaultByCriteria(Context.Users, user => user.Id == userId);

            if (usr == null)
                ExceptionHelper.ThrowArgumentException(s_Logger, "", new ArgumentException("No user exists for the given parameter.", "userId"));

            return usr.Rights.HasFlag(Rights.Administer);
        }

        /// <summary>Loescht die Admin Rolle eines Nutzers. Falls der Nutzer keine Adminrolle hat, passiert nichts.</summary>
        /// <param name="userId"></param>
        /// <exception cref="ArgumentException">Falls kein user mit der id existiert.</exception>
        public void RemAdminRole(long userId)
        {
            User usr = GenericSearcher.TryGetFirstOrDefaultByCriteria(Context.Users, user => user.Id == userId);

            if (usr == null)
                ExceptionHelper.ThrowArgumentException(s_Logger, "", new ArgumentException("No user exists for the given parameter.", "userId"));

            usr.Rights &= (~Rights.Administer);

            Context.SaveChanges();
        }

        /// <summary>
        ///     Gibt die Nutzer-Id zu der Email zurück.
        /// </summary>
        /// <param name="email"></param>
        /// <returns>id des Nutzers mit der Email. Falls kein user mit der email Adresse vorhanden ist, wird null zurueckgegeben.</returns>
        public long GetUserId(string email)
        {
            User usr = GenericSearcher.TryGetFirstOrDefaultByCriteria(Context.Users, user => user.Mail == email);

            if (usr == null)
                ExceptionHelper.ThrowArgumentException(s_Logger, "", new ArgumentException("No user exists for the given parameter.", "email"));

            return usr.Id;
        }

        /// <summary>
        ///     Methode zum Loeschen des Nutzers. Dabei sollen alle Pois, Events und Messages geloescht werden, die der Nutzer angelegt hat oder die an den entsprechenden Pois oder Events haengen.
        /// </summary>
        /// <param name="userId"></param>
        /// <exception cref="ArgumentException">Falls kein user mit der id existiert.</exception>
        public void DeleteUser(long userId)
        {
            User user = GenericSearcher.TryGetFirstOrDefaultByCriteria(Context.Users, user1 => user1.Id == userId);

            if (user == null)
                ExceptionHelper.ThrowArgumentException(s_Logger, "", new ArgumentException("No user exists for the given parameter.", "userId"));

            user.InitDel(Context);

            Context.SaveChanges();
        }

        /// <summary>
        ///     Methode zum Loeschen des Nutzers. Dabei sollen alle Pois, Events und Messages geloescht werden, die der Nutzer angelegt hat oder die an den entsprechenden Pois oder Events haengen.
        /// </summary>
        /// <param name="email"></param>
        /// <exception cref="ArgumentException">Falls kein user mit der id existiert.</exception>
        public void DeleteUser(string email)
        {
            User user = GenericSearcher.TryGetFirstOrDefaultByCriteria(Context.Users, user1 => user1.Mail == email);

            if (user == null)
                ExceptionHelper.ThrowArgumentException(s_Logger, "", new ArgumentException("No user exists for the given parameter.", "email"));

            user.InitDel(Context);

            Context.SaveChanges();
        }

        #endregion
    }
}
