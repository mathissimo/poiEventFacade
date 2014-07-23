using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PoiEventNetwork.Data.Interface;
using PoiEventNetwork.Data.POI;
using PoiEventNetwork.Data;
using PoiEventNetwork;
using Microsoft.Practices.Unity;
using PoiEventNetwork.Data.Exception;
using System.ComponentModel.DataAnnotations;
using PoiEventNetwork.Facade;
using PoiEventNetwork.Facade.Interface;


namespace PoiEventFacadeTest
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class CreateSimpleBasePoiTest : TestInit
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
	    public void TestCreateBasePoi() 
        {
            using (IPoiEventFacade facade = Resolver.Resolve<IPoiEventFacade>())
            {
                string name = string.Format("{0} {1}", HtwName, MethodBase.GetCurrentMethod().Name);

                facade.CreateBasePoi(AdminId, name, HtwTags, HtwPos);
                // TODO: AbsPoi oder BasePoi für actual?
                BasePoi actual = facade.GetPoi(name);

                CityPoi expected = facade.Context.CityPois.Create();
                expected.Name = name;
                expected.InitAddTags(Tag.GetTagsFromLabels(facade.Context, HtwTags));

                Assert.AreEqual(expected, actual);
            }
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentException))]
        public void TestCreateBasePoiDuplicate()
        {
            using (IPoiEventFacade facade = Resolver.Resolve<IPoiEventFacade>())
            {
                facade.CreateBasePoi(AdminId, HtwName + " TestCreateBasePoiDuplicate", HtwTags, HtwPos);
                facade.CreateBasePoi(AdminId, HtwName + " TestCreateBasePoiDuplicate", HtwTags, HtwPos);
            }
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentException))]
        public void TestCreatePoiDuplicate()
        {
            using (IPoiEventFacade facade = Resolver.Resolve<IPoiEventFacade>())
            {
                facade.CreateBasePoi(AdminId, HtwName + " TestCreatePoiDuplicate", HtwTags, HtwPos);
                facade.CreateCityPoi(AdminId, HtwName + " TestCreatePoiDuplicate", HtwTags, HtwStreet, HtwStadt, HtwPos);
            }
        }

        [TestMethod]
        [ExpectedException(typeof (AuthorizationException))]
        public void TestcreatePoiWithoutPermissions()
        {
            using (IPoiEventFacade facade = Resolver.Resolve<IPoiEventFacade>())
            {
                long userId = facade.CreateUser("Default", "User", "TestcreatePoiWithoutPermissions@gmail.com");
                facade.CreateBasePoi(userId, HtwName + " TestcreatePoiWithoutPermissions", HtwTags, HtwPos);
            }
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentException))]
        public void TestCreatePoiWithoutValidUserId()
        {
            using (IPoiEventFacade facade = Resolver.Resolve<IPoiEventFacade>())
            {
                facade.CreateBasePoi(7373620223786557L, HtwName + " TestCreatePoiWithoutValidUserID", HtwTags, HtwPos);
            }
        }

        [TestMethod]
        [ExpectedException(typeof (ValidationException))]
        public void TestLatitudeOutOfRange1()
        {
            using (IPoiEventFacade facade = Resolver.Resolve<IPoiEventFacade>())
            {
                Coordinate wrongCoord = new Coordinate(-90.1f, HtwLon);
                facade.CreateBasePoi(AdminId, HtwName + " TestLatitudeOutOfRange1", HtwTags, wrongCoord);
            }
        }

        [TestMethod]
        [ExpectedException(typeof (ValidationException))]
        public void TestLatitudeOutOfRange2()
        {
            using (IPoiEventFacade facade = Resolver.Resolve<IPoiEventFacade>())
            {
                Coordinate wrongCoord = new Coordinate(+90.1f, HtwLon);
                facade.CreateBasePoi(AdminId, HtwName + " TestLatitudeOutOfRange2", HtwTags, wrongCoord);
            }
        }

        [TestMethod]
        [ExpectedException(typeof (ValidationException))]
        public void TestLongitudeOutOfRange1()
        {
            using (IPoiEventFacade facade = Resolver.Resolve<IPoiEventFacade>())
            {
                Coordinate wrongCoord = new Coordinate(HtwLat, -180.1f);
                facade.CreateBasePoi(AdminId, HtwName + " TestLongitudeOutOfRange1", HtwTags, wrongCoord);
            }
        }

        [TestMethod]
        [ExpectedException(typeof (ValidationException))]
        public void TestLongitudeOutOfRange2()
        {
            using (IPoiEventFacade facade = Resolver.Resolve<IPoiEventFacade>())
            {
                Coordinate wrongCoord = new Coordinate(HtwLat, 180.1f);
                facade.CreateBasePoi(AdminId, HtwName + " TestLongitudeOutOfRange2", HtwTags, wrongCoord);
            }
        }
    }
}
