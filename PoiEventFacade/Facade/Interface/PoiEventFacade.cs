using System;
using System.Security.Authentication;
using PoiEventFacade.Data;

namespace PoiEventFacade.Facade.Interface
{
    /// <summary>
    /// Fassade, die als Projektaufgabe implementiert werden muss.
    /// Jede einzelne Methode muss zustandslos und threadsave sein. 
    /// </summary>
    public interface IPoiEventFacade
    {
        /// <summary>
        /// Methode zum Erzeugen eines neuen einfachen Point of Interest (POI).
        /// </summary>
        /// <param name="userId">ID des Nutzers, der den POI anlegt.</param>
        /// <param name="name">Eindeutiger Name des POI.</param>
        /// <param name="tags">Schlagworte, die den POI charakterisieren.</param>
        /// <param name="lat">Geokoordinate in Grad, muss im Bereich zwischen -090 und 090 liegen.</param>
        /// <param name="lon">Geokoordinate in Grad, muss im Bereich zwischen -180 und 180 liegen.</param>
        /// <exception cref="AuthenticationException">Falls der Nutzer kein Admin ist.</exception>
        /// <exception cref="ArgumentException">Falls kein Nutzer mit der ID existiert.</exception>
        /// <exception cref="ArgumentException">Falls Geokoordinaten außerhalb des gültigen Wertebereiches liegen.</exception>
        void CreateSimplePoi(long userId, string name, string[] tags, float lat, float lon);

        /// <summary>
        /// Methode zum Erzeugen eines neuen City Point of Interest (POI).
        /// </summary>
        /// <param name="userId">ID des Nutzers, der den POI anlegt.</param>
        /// <param name="name">eindeutiger Name des POI.</param>
        /// <param name="tags">Schlagworte, die den POI charakterisieren.</param>
        /// <param name="street">Strasse, in dem der POI liegt.</param>
        /// <param name="city">Stadt, in dem der POI liegt.</param>
        /// <param name="lat">Geokoordinate in Grad, muss im Bereich zwischen -090 und 090 liegen.</param>
        /// <param name="lon">Geokoordinate in Grad, muss im Bereich zwischen -180 und 180 liegen.</param>
        /// <exception cref="AuthenticationException">Falls der Nutzer kein Admin ist.</exception>
        /// <exception cref="ArgumentException">Falls kein Nutzer mit der ID existiert.</exception>
        /// <exception cref="ArgumentException">Falls Geokoordinaten außerhalb des gültigen Wertebereiches liegen.</exception>
        void CreateCityPoi(long userId, string name, string[] tags, string street, string city, float lat, float lon);

        /// <summary>
        /// Methode zum Erzeugen eines neuen Polygon Point of Interest (POI).
        /// </summary>
        /// <param name="userId">ID des Nutzers, der den POI anlegt.</param>
        /// <param name="name">eindeutiger Name des POI.</param>
        /// <param name="tags">Schlagworte, die den POI charakterisieren.</param>
        /// <param name="polygon">Geordnete Liste der Geokoordinaten, diese beschreiben eine Fläche. Die einzelnen Koordinaten muessen dabei gueltige Koordinaten sein.</param>
        /// <exception cref="AuthenticationException">Falls der Nutzer kein Admin ist.</exception>
        /// <exception cref="ArgumentException">Falls kein Nutzer mit der ID existiert.</exception>
        /// <exception cref="ArgumentException">Falls Geokoordinaten außerhalb des gültigen Wertebereiches liegen.</exception>
        void CreatePolygonPoi(long userId, string name, string[] tags, Coordinate[] polygon);

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
        /// <param name="tag">Schlagwort, das zur Schlagwortbeschreibung des POI hinzugefuegt werden soll.</param>
        /// <exception cref="ArgumentException">Falls das Schlagwort schon existiert.</exception>
        /// <exception cref="AuthenticationException">Falls der Nutzer kein Admin ist.</exception>
        /// <exception cref="ArgumentException">Falls kein Nutzer mit der ID existiert.</exception>
        void AddPoiTag(long userId, string name, string tag);

        /// <summary>
        /// Methode zum Löschen eines Point of Interest.
        /// </summary>
        /// <param name="userId">ID des Nutzers, der den POI anlegt.</param>
        /// <param name="name">eindeutiger Name des POI.</param>
        /// <param name="tag">Schlagwort, das zur Schlagwortbeschreibung des POI hinzugefuegt werden soll.</param>
        /// <exception cref="ArgumentException">Falls kein solches Schalgwort existiert.</exception>
        /// <exception cref="ArgumentException">Falls kein Nutzer mit der ID existiert.</exception>
        /// <exception cref="ArgumentException">Falls kein Poi mit dem Namen existiert.</exception>
        /// <exception cref="AuthenticationException">Falls der Nutzer kein Admin ist.</exception>
        void DeletePoiTag(long userId, string name, string tag);

        /// <summary>
        /// Gibt alle POIs zurück, die das Schlagwort beinhalten.
	    /// Achten Sie hier auf eine performante Implementierung.
        /// </summary>
        /// <param name="tag">Schlagwort.</param>
        /// <returns>Menge aller Pois, die das Schlagwort beinhalten.</returns>
        Poi[] GetPoiByTag(string tag);

        /// <summary>
        /// Gibt den Poi mit dem angefragenten Namen zurueck. 
        /// </summary>
        /// <param name="name">Name des Pois der zurueckgegeben werden soll.</param>
        /// <returns>Existiert kein Poi mit dem Namen, so wird null zurueckgegeben.</returns>
        Poi GetPoi(string name);

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
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="cityId"></param>
        /// <param name="tag"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        Poi[] GetPoisByCriteria(long userId, long cityId, string tag, string[] query);

    }
}