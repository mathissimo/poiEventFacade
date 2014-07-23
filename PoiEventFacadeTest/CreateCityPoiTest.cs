using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PoiEventNetwork.Data.Interface;
using PoiEventNetwork.Data.POI;
using PoiEventNetwork.Data;
using PoiEventNetwork.Data.Context;
using PoiEventNetwork.Data.Exception;
using PoiEventNetwork;
using Microsoft.Practices.Unity;
using PoiEventNetwork.Facade;
using PoiEventNetwork.Facade.Interface;

namespace PoiEventFacadeTest
{
	[TestClass]
    [ExcludeFromCodeCoverage]
	public class CreateCityPoiTest : TestInit
	{
        private static long AdminId { get; set; }

	    [ClassInitialize]
	    public static void ClassInitialize(TestContext testCtx)
	    {
            using (IPoiEventFacade facade = Resolver.Resolve<IPoiEventFacade>())
	        {
	            facade.Context.Database.Delete();

	            AdminId = facade.CreateUser("Default", "Admin", "default.admin@gmail.com");
	            facade.SetAdminRole(AdminId);
	        }
	    }

	    [TestMethod]
	    public void TestcreateCityPoi()
	    {
	        using (IPoiEventFacade facade = Resolver.Resolve<IPoiEventFacade>())
	        {
	            facade.CreateCityPoi(AdminId, HtwName + " TestcreateCityPoi", HtwTags, HtwStreet, HtwStadt, HtwPos);
	            // TODO: AbsPoi oder BasePoi für actual?
	            BasePoi actual = facade.GetPoi(HtwName + " TestcreateCityPoi");

	            // TODO: City-Property setzen: {Street = HtwStreet, City = HtwStadt};
                CityPoi expected = facade.Context.CityPois.Create();
                expected.Name = HtwName + " TestcreateCityPoi";
	            expected.InitAddTags(Tag.GetTagsFromLabels(facade.Context, HtwTags));
	            expected.Street = HtwStreet;
	            expected.City = HtwStadt;

	            Assert.AreEqual(expected, actual);
	        }
	    }

	    [TestMethod]
	    [ExpectedException(typeof (AuthorizationException))]
	    public void TestcreatePoiWithoutPermissions()
	    {
	        using (IPoiEventFacade facade = Resolver.Resolve<IPoiEventFacade>())
	        {
	            long userId = facade.CreateUser("Default", "User", "TestcreatePoiWithoutPermissions@gmail.com");
	            facade.CreateCityPoi(userId, HtwName + " TestcreatePoiWithoutPermissions", HtwTags, HtwStreet, HtwStadt,
	                                 HtwPos);
	        }
	    }

	    [TestMethod]
	    [ExpectedException(typeof (ArgumentException))]
	    public void TestCreatePoiWithoutValidUserId()
	    {
	        using (IPoiEventFacade facade = Resolver.Resolve<IPoiEventFacade>())
	        {
	            facade.CreateCityPoi(7373620223786557L, HtwName + " TestCreatePoiWithoutValidUserID", HtwTags, HtwStreet,
	                                 HtwStadt, HtwPos);
	        }
	    }

	    //[TestMethod]
        //[ExpectedException(typeof(ArgumentException))]
        //public void testCreatePoiWithoutNullUserID()
        //{
        //    facade.CreateCityPoi(null, HtwName, HtwTags, HtwStreet, HtwStadt, HtwPos);
        //}

	    [TestMethod]
	    [ExpectedException(typeof (ValidationException))]
	    public void TestLatitudeOutOfRange1()
	    {
	        using (IPoiEventFacade facade = Resolver.Resolve<IPoiEventFacade>())
	        {
	            Coordinate wrongCoord = new Coordinate(-90.1f, HtwLon);
	            facade.CreateCityPoi(AdminId, HtwName + " TestLatitudeOutOfRange1", HtwTags, HtwStreet, HtwStadt,
	                                 wrongCoord);
	        }
	    }

	    [TestMethod]
	    [ExpectedException(typeof (ValidationException))]
	    public void TestLatitudeOutOfRange2()
	    {
	        using (IPoiEventFacade facade = Resolver.Resolve<IPoiEventFacade>())
	        {
	            Coordinate wrongCoord = new Coordinate(+90.1f, HtwLon);
	            facade.CreateCityPoi(AdminId, HtwName + " TestLatitudeOutOfRange2", HtwTags, HtwStreet, HtwStadt,
	                                 wrongCoord);
	        }
	    }

	    [TestMethod]
	    [ExpectedException(typeof (ValidationException))]
	    public void TestLongitudeOutOfRange1()
	    {
	        using (IPoiEventFacade facade = Resolver.Resolve<IPoiEventFacade>())
	        {
	            Coordinate wrongCoord = new Coordinate(HtwLat, -180.1f);
	            facade.CreateCityPoi(AdminId, HtwName + " TestLongitudeOutOfRange1", HtwTags, HtwStreet, HtwStadt,
	                                 wrongCoord);
	        }
	    }

	    [TestMethod]
	    [ExpectedException(typeof (ValidationException))]
	    public void TestLongitudeOutOfRange2()
	    {
	        using (IPoiEventFacade facade = Resolver.Resolve<IPoiEventFacade>())
	        {
	            Coordinate wrongCoord = new Coordinate(HtwLat, 180.1f);
	            facade.CreateCityPoi(AdminId, HtwName + " TestLongitudeOutOfRange2", HtwTags, HtwStreet, HtwStadt,
	                                 wrongCoord);
	        }
	    }

	    //[TestMethod]
        //[ExpectedException(typeof(ArgumentException))]
        //public void testLongitudeNull()
        //{
        //    facade.CreateCityPoi(AdminId, HtwName, HtwTags, HtwStreet, HtwStadt,
        //            HtwLat, null);
        //}

        //[TestMethod]
        //[ExpectedException(typeof(ArgumentException))]
        //public void testLatitudeNull()
        //{
        //    facade.CreateCityPoi(AdminId, HtwName, HtwTags, HtwStreet, HtwStadt,
        //            null, HtwLon);
        //}
	}
}
