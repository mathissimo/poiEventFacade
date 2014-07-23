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


namespace PoiEventFacadeTest
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
	}
}
