using System;
using System.Collections.Generic;
using System.Security.Authentication;
using PoiEventNetwork.Data;
using PoiEventNetwork.Data.Interface;
using PoiEventNetwork.Data.POI;

namespace PoiEventNetwork.Facade.Interface
{
    /// <summary>
    /// Fassade, die als Projektaufgabe implementiert werden muss.
    /// Jede einzelne Methode muss zustandslos und threadsave sein. 
    /// </summary>
    public interface IPoiEventFacade : IDisposable
    {
        IDataContext Context { get; }

        /// <summary>
        /// Methode zum Erzeugen eines neuen einfachen Point of Interest (POI).
        /// </summary>
        /// <param name="userId">ID des Nutzers, der den POI anlegt.</param>
        /// <param name="name">Eindeutiger Name des POI.</param>
        /// <param name="tags">Schlagworte, die den POI charakterisieren.</param>
        /// <param name="pos">Geokoordinate in Grad.</param>
        /// <exception cref="AuthenticationException">Falls der Nutzer kein Admin ist.</exception>
        /// <exception cref="ArgumentException">Falls kein Nutzer mit der ID existiert.</exception>
        /// <exception cref="ArgumentException">Falls Geokoordinaten außerhalb des gültigen Wertebereiches liegen.</exception>
        void CreateBasePoi(long userId, string name, IEnumerable<string> labels, Coordinate pos);

        /// <summary>
        /// Methode zum Erzeugen eines neuen City Point of Interest (POI).
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
        void CreateCityPoi(long userId, string name, IEnumerable<string> labels, string street, string city, Coordinate pos);

        /// <summary>
        /// Methode zum Erzeugen eines neuen Polygon Point of Interest (POI).
        /// </summary>
        /// <param name="userId">ID des Nutzers, der den POI anlegt.</param>
        /// <param name="name">eindeutiger Name des POI.</param>
        /// <param name="labels">Schlagworte, die den POI charakterisieren.</param>
        /// <param name="polygon">Geordnete Liste der Geokoordinaten, diese beschreiben eine Fläche. Die einzelnen Koordinaten muessen dabei gueltige Koordinaten sein.</param>
        /// <exception cref="AuthenticationException">Falls der Nutzer kein Admin ist.</exception>
        /// <exception cref="ArgumentException">Falls kein Nutzer mit der ID existiert.</exception>
        /// <exception cref="ArgumentException">Falls Geokoordinaten außerhalb des gültigen Wertebereiches liegen.</exception>
        void CreatePolygonPoi(long userId, string name, IEnumerable<string> labels, Coordinate[] polygon);

        /// <summary>
        /// Methode zum Löschen eines Point of Interest.
        /// </summary>
        /// <param name="userId">ID des Nutzers, der den POI anlegt.</param>
        /// <param name="name">Eindeutige Name des Poi der gelöscht werden soll.</param>
        /// <exception cref="AuthenticationException">Falls der Nutzer kein Admin ist.</exception>
        /// <exception cref="ArgumentException">Falls kein Nutzer mit der ID existiert.</exception>
        void DeletePoi(long userId, string name);

        /// <summary>
        /// Methode zum Hinzufügen eines Point of Interest.
        /// </summary>
        /// <param name="userId">ID des Nutzers, der den POI anlegt.</param>
        /// <param name="name">eindeutiger Name des POI.</param>
        /// <param name="label">Schlagwort, das zur Schlagwortbeschreibung des POI hinzugefuegt werden soll.</param>
        /// <exception cref="ArgumentException">Falls das Schlagwort schon existiert.</exception>
        /// <exception cref="AuthenticationException">Falls der Nutzer kein Admin ist.</exception>
        /// <exception cref="ArgumentException">Falls kein Nutzer mit der ID existiert.</exception>
        void AddPoiTag(long userId, string name, string label);

        /// <summary>
        /// Methode zum Löschen eines Point of Interest.
        /// </summary>
        /// <param name="userId">ID des Nutzers, der den POI anlegt.</param>
        /// <param name="name">eindeutiger Name des POI.</param>
        /// <param name="label">Schlagwort, das zur Schlagwortbeschreibung des POI hinzugefuegt werden soll.</param>
        /// <exception cref="ArgumentException">Falls kein solches Schalgwort existiert.</exception>
        /// <exception cref="ArgumentException">Falls kein Nutzer mit der ID existiert.</exception>
        /// <exception cref="ArgumentException">Falls kein Poi mit dem Namen existiert.</exception>
        /// <exception cref="AuthenticationException">Falls der Nutzer kein Admin ist.</exception>
        void DeletePoiTag(long userId, string name, string label);

        /// <summary>
        /// Gibt alle POIs zurück, die das Schlagwort beinhalten.
	    /// Achten Sie hier auf eine performante Implementierung.
        /// </summary>
        /// <param name="label">Schlagwort.</param>
        /// <returns>Menge aller Pois, die das Schlagwort beinhalten.</returns>
        IEnumerable<BasePoi> GetPoiByTag(string label);

        /// <summary>
        /// Gibt den Poi mit dem angefragenten Namen zurueck. 
        /// </summary>
        /// <param name="name">Name des Pois der zurueckgegeben werden soll.</param>
        /// <returns>Existiert kein Poi mit dem Namen, so wird null zurueckgegeben.</returns>
        BasePoi GetPoi(string name);

        /// <summary>
        /// Beim Erzeugen soll intern auch das Anlegedatum (siehe date Attribut in Event) gespeichert werden.
        /// </summary>
        /// <param name="userId">Id des Nutzers, der den Event anlegt.</param>
        /// <param name="poiName">Name des Poi, zu dem der Event hinzugefügt wird.</param>
        /// <param name="title">Titel des Events.</param>
        /// <param name="description">Textuelle beschreibung des Events.</param>
        /// <returns>Id, die den Event eindeutig identifiziert.</returns>
        /// <exception cref="ArgumentException">Falls kein User mit der id existiert.</exception>
        long CreateEvent(long userId, string poiName, string title, string description);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId">
        /// Id des Nutzer, der den Event loeschen will. Ist dies kein 
	    /// Adminstrator oder nicht der Nutzer, der den Event angelegt hat, so wird eine 
	    /// AuthorizationException geworfen.  
	    /// Wird ein Event geloescht, so werden auch alle Nachrichten
	    /// zu dem Event geloescht.
	    /// </param>
        /// <param name="eventId">Id des Events.</param>
        /// <exception cref="ArgumentException">Falls kein Event mit dem Namen existiert.</exception>
        /// <exception cref="ArgumentException">Falls kein user mit der id existiert.</exception>
        void DeleteEvent(long userId, long eventId);

        /// <summary>
        /// Retrieval Methode zum Finden von allen Events zu den Pois.
        /// </summary>
        /// <param name="poiName">name des pois.</param>
        /// <returns>Alle Events, die zum Poi hinzugefuegt worden sind.</returns>
        /// <exception cref="ArgumentException">falls kein Poi mit dem Namen existiert.</exception>
        Event[] FindEventsForPoi(string poiName);

        /// <summary>
        /// Nutzer koennen sich zu Events anmelden. 
        /// </summary>
        /// <param name="userId">Nutzer Id.</param>
        /// <param name="eventId">Event Id.</param>
        /// <exception cref="ArgumentException">Falls kein Event mit dem Namen existiert.</exception>
        /// <exception cref="ArgumentException">Falls kein user mit der id existiert.</exception>
        void SubscribeToEvent(long userId, long eventId);

        /// <summary>
        /// Finde alle Events fuer die sich der Nutzer mit der userId registriert hat.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>Menge der Events.</returns>
        /// <exception cref="ArgumentException">Falls kein user mit der id existiert.</exception>
        Event[] FindSubscribedEvents(long userId);

        /// <summary>
        /// Finde alle Events, die der Nutzer angelegt hat.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>Menge der Events.</returns>
        /// <exception cref="ArgumentException">Falls kein user mit der id existiert.</exception>
        Event[] FindOwnedEvents(long userId);

        /// <summary>
        /// Fuege eine Nachricht zu einem Event hinzu.
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="userId"></param>
        /// <param name="title"></param>
        /// <param name="content"></param>
        /// <exception cref="ArgumentException">Falls kein Event mit dem Namen existiert.</exception>
        /// <exception cref="ArgumentException">Falls kein user mit der id existiert.</exception>
        void AddMessage(long eventId, long userId, string title, string content);

        /// <summary>
        /// Retrieval Methode zum Finden aller Nachrichten zu einem Event.
        /// </summary>
        /// <param name="eventId"></param>
        /// <returns>Sortiert nach dem Anlegedatum der Nachricht (je jünger die Nachricht, desto spaeter steht sie in der Liste.)</returns>
        Message[] GetMessages(long eventId);

        /// <summary>
        /// Erzeugt einen Nutzer.
        /// </summary>
        /// <param name="lastName"></param>
        /// <param name="firstName"></param>
        /// <param name="email"></param>
        /// <returns>Id des Nutzers.</returns>
        /// <exception cref="ArgumentException">Falls ein Nutzer mit der E-Mail bereits existiert.</exception>
        long CreateUser(string lastName, string firstName, string email);

        /// <summary>
        /// Fuegt zu einem Nutzer die admin Rechte hinzu.
        /// </summary>
        /// <param name="userId"></param>
        /// <exception cref="ArgumentException">Falls kein user mit der id existiert.</exception>
        void SetAdminRole(long userId);

        /// <summary>
        /// Erfragt zu einem Nutzer die admin Rechte.
        /// </summary>
        /// <param name="userId"></param>
        /// <exception cref="ArgumentException">Falls kein user mit der id existiert.</exception>
        /// <returns>True wenn Admin.</returns>
        bool HasAdminRole(long userId);

        /// <summary>Loescht die Admin Rolle eines Nutzers. Falls der Nutzer keine Adminrolle hat, passiert nichts.</summary>
        /// <param name="userId"></param>
        /// <exception cref="ArgumentException">Falls kein user mit der id existiert.</exception>
        void RemAdminRole(long userId);

        /// <summary>
        /// Gibt die Nutzer-Id zu der Email zurück.
        /// </summary>
        /// <param name="email"></param>
        /// <returns>Id des Nutzers mit der Email.</returns>
        long GetUserId(string email);

        /// <summary>
        /// Methode zum Loeschen des Nutzers. Dabei sollen alle Pois, Events und Messages geloescht werden, die der Nutzer angelegt hat oder die an den entsprechenden Pois oder Events haengen.
        /// </summary>
        /// <param name="userId"></param>
        /// <exception cref="ArgumentException">Falls kein user mit der id existiert.</exception>
        void DeleteUser(long userId);

        /// <summary>
        /// Methode zum Loeschen des Nutzers. Dabei sollen alle Pois, Events und Messages geloescht werden, die der Nutzer angelegt hat oder die an den entsprechenden Pois oder Events haengen.
        /// </summary>
        /// <param name="email"></param>
        /// <exception cref="ArgumentException">Falls kein user mit der id existiert.</exception>
        void DeleteUser(string email);
    }
}