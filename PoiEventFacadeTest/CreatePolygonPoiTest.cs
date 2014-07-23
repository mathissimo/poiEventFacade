using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PoiEventNetwork.Data.Interface;
using PoiEventNetwork.Data.POI;
using PoiEventNetwork.Data;
using PoiEventNetwork;
using PoiEventNetwork.Data.Exception;
using System.ComponentModel.DataAnnotations;
using Microsoft.Practices.Unity;
using PoiEventNetwork.Facade;
using PoiEventNetwork.Facade.Interface;

namespace PoiEventFacadeTest

{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class CreatePolygonPoiTest: TestInit
    {
	    static Coordinate Valid1= new Coordinate(30.03f, 50.3f);
	    static Coordinate Valid2= new Coordinate(31.03f, 51.3f);
	    static Coordinate Valid3= new Coordinate(31.03f, 50.3f);

	    Coordinate[] ValidPolygon = new [] {Valid1,Valid2,Valid3};

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
	    public void TestCreatePolygonPoi() 
        {
            using (IPoiEventFacade facade = Resolver.Resolve<IPoiEventFacade>())
            {
                string name = string.Format("{0} {1}", HtwName, MethodBase.GetCurrentMethod().Name);

                facade.CreatePolygonPoi(AdminId, name, HtwTags, ValidPolygon);
                
                var actual = facade.GetPoi(name);

                NgonPoi expected = facade.Context.NgonPois.Create();
                expected.Name = name;
                expected.InitAddTags(Tag.GetTagsFromLabels(facade.Context, HtwTags));

                foreach (var coordinate in ValidPolygon)
                {
                    expected.Polygon.Add(coordinate);
                }

                Assert.AreEqual(expected, actual);
            }
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentException))]
        public void TestCreatePolygonPoiDuplicate()
        {
            using (IPoiEventFacade facade = Resolver.Resolve<IPoiEventFacade>())
            {
                facade.CreatePolygonPoi(AdminId, HtwName + " TestCreatePolygonPoiDuplicate", HtwTags, ValidPolygon);
                facade.CreatePolygonPoi(AdminId, HtwName + " TestCreatePolygonPoiDuplicate", HtwTags, ValidPolygon);
            }
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentException))]
        public void TestCreatePoiDuplicate()
        {
            using (IPoiEventFacade facade = Resolver.Resolve<IPoiEventFacade>())
            {
                facade.CreatePolygonPoi(AdminId, HtwName + " TestCreatePoiDuplicate", HtwTags, ValidPolygon);
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
                facade.CreatePolygonPoi(userId, HtwName + " TestcreatePoiWithoutPermissions", HtwTags, ValidPolygon);
            }
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentException))]
        public void TestCreatePoiWithoutValidUserId()
        {
            using (IPoiEventFacade facade = Resolver.Resolve<IPoiEventFacade>())
            {
                facade.CreatePolygonPoi(7373620223786557L, HtwName + " TestCreatePoiWithoutValidUserID", HtwTags,
                                        ValidPolygon);
            }
        }

        [TestMethod]
        [ExpectedException(typeof (ValidationException))]
        public void TestCreatePoiWithoutInvalidPolygon()
        {
            using (IPoiEventFacade facade = Resolver.Resolve<IPoiEventFacade>())
            {
                Coordinate NotValid = new Coordinate(0.0f, 181.1f);
                Coordinate[] NotValidPolygon = new[] {Valid1, Valid2, NotValid, Valid3};
                facade.CreatePolygonPoi(AdminId, HtwName + " TestCreatePoiWithoutInvalidPolygon", HtwTags, ValidPolygon);
            }
        }
    }
}
