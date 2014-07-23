using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PoiEventNetwork.Data.Interface;
using PoiEventNetwork.Facade;
using PoiEventNetwork.Data.Context;
using System.Collections.ObjectModel;
using PoiEventNetwork.Data;
using System.Linq;
using PoiEventNetwork;
using Microsoft.Practices.Unity;


namespace PoiEventImplementationTest
{
    [ExcludeFromCodeCoverage]
	public class TestInit
	{
	    public const float       HtwLat     = 52.457735f;
	    public const float       HtwLon     = 13.526187f;

        protected static readonly Coordinate HtwPos = new Coordinate(HtwLat, HtwLon);

        protected static String   HtwStreet  = "Wilhelminenhofstraße 75a";
        protected String          HtwStadt   = "Berlin";
        protected static String   HtwName    = "HTW Berlin";
	    protected static string[] HtwTags    = new[] {"Hochschule", "Technik", "Wirtschaft"};

        protected static ObservableCollection<Tag> TagCollectionForStringArray (String[]sourceArray) {
            ObservableCollection<Tag> output = new ObservableCollection<Tag>();
            foreach (String looper in sourceArray)
            {
                output.Add(new Tag(looper));
            }
            return output;
        }

        protected static ObservableCollection<Event> CreateTestEvents ()
        {
            ObservableCollection<Event> testEvents = new ObservableCollection<Event>();
            testEvents.Add(new Event { Name = "Event1" });
            testEvents.Add(new Event { Name = "Event2" });
            return testEvents;
        }

        protected static bool TagColEqualsStringArray(ObservableCollection<Tag> one, String[] other)
        {
            bool sameContentFound = true;
            foreach (String looper in other)
            {
                if (!one.Contains(new Tag(looper)))
                {
                    sameContentFound = false;
                }
            }
            return sameContentFound;
        }

	}
}
