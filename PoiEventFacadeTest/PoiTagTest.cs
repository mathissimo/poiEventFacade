using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PoiEventNetwork.Data.Interface;
using PoiEventNetwork.Data.POI;
using PoiEventNetwork.Data;
using PoiEventNetwork.Data.Context;
using PoiEventNetwork.Data.Exception;
using PoiEventNetwork;
using System.ComponentModel.DataAnnotations;
using Microsoft.Practices.Unity;
using PoiEventNetwork.Facade;
using PoiEventNetwork.Facade.Interface;

namespace PoiEventFacadeTest
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class PoiTagTest : TestInit
    {
	    private static String UasString = "university of applied science";
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
	    public void TestAddTags()
        {
            using (IPoiEventFacade facade = Resolver.Resolve<IPoiEventFacade>())
            {
                String testLabel = string.Format("{0} {1}", UasString, MethodBase.GetCurrentMethod().Name);
                String testPoiName = string.Format("{0} {1}", HtwName, MethodBase.GetCurrentMethod().Name);
                Tag testLabelAsTag = new Tag(testLabel);

                facade.CreateBasePoi(AdminId, testPoiName, HtwTags, HtwPos);
                facade.AddPoiTag(AdminId, testPoiName, testLabel);
                BasePoi poi = facade.GetPoi(testPoiName);

            }
        }

        [TestMethod]
        [ExpectedException(typeof(AuthorizationException))]
        public void TestAddTagWithoutPermissions()
        {
            using (IPoiEventFacade facade = Resolver.Resolve<IPoiEventFacade>())
            {
                String testLabel = string.Format("{0} {1}", UasString, MethodBase.GetCurrentMethod().Name);
                String testPoiName = string.Format("{0} {1}", HtwName, MethodBase.GetCurrentMethod().Name);
                long userId = facade.CreateUser("Default", "User", "TestAddTagWithoutPermissions@gmail.com");

                facade.CreateBasePoi(AdminId, testPoiName, HtwTags, HtwPos);

                facade.AddPoiTag(userId, testPoiName, testLabel);
            }
        }

        [TestMethod]
        public void TestDeletePoiTags()
        {
            using (IPoiEventFacade facade = Resolver.Resolve<IPoiEventFacade>())
            {
                // create Tag and check if successsfull
                String testLabel = string.Format("{0} {1}", UasString, MethodBase.GetCurrentMethod().Name);
                String testPoiName = string.Format("{0} {1}", HtwName, MethodBase.GetCurrentMethod().Name);
                Tag testLabelAsTag = new Tag(testLabel);

                facade.CreateBasePoi(AdminId, testPoiName, HtwTags, HtwPos);
                facade.AddPoiTag(AdminId, testPoiName, testLabel);
                BasePoi poi = facade.GetPoi(testPoiName);

                Assert.IsTrue(poi.Tags.Contains(testLabelAsTag));

                // delete Tag and check if successfull
                facade.DeletePoiTag(AdminId, testPoiName, testLabel);
                BasePoi poi2 = facade.GetPoi(testPoiName);

                Assert.IsFalse(poi2.Tags.Contains(testLabelAsTag));
            }
        }

        [TestMethod]
        [ExpectedException(typeof (AuthorizationException))]
        public void TestDeleteTagsWithoutPermissions()
        {
            using (IPoiEventFacade facade = Resolver.Resolve<IPoiEventFacade>())
            {
                // create Poi + Tag
                String testLabel = string.Format("{0} {1}", UasString, MethodBase.GetCurrentMethod().Name);
                String testPoiName = string.Format("{0} {1}", HtwName, MethodBase.GetCurrentMethod().Name);
                Tag testLabelAsTag = new Tag(testLabel);

                facade.CreateBasePoi(AdminId, testPoiName, HtwTags, HtwPos);
                facade.AddPoiTag(AdminId, testPoiName, testLabel);
                BasePoi poi = facade.GetPoi(testPoiName);

                Assert.IsTrue(poi.Tags.Contains(testLabelAsTag));

                // try to delete without rights
                long userId = facade.CreateUser("Default", "User", "TestDeleteTagsWithoutPermissions@gmail.com");

                facade.DeletePoiTag(userId, testPoiName, testLabel);
            }
        }

        [TestMethod]
        public void TestGetByTags()
        {
            using (IPoiEventFacade facade = Resolver.Resolve<IPoiEventFacade>())
            {
                facade.CreateBasePoi(AdminId, "AAA", new[] {"Bildung", "Hausbau"}, HtwPos);
                facade.CreateBasePoi(AdminId, "BBB", new[] {"Bildung", "Mauer"}, HtwPos);
                facade.CreateBasePoi(AdminId, "CCC", new[] {"Spiegel", "Welt"}, HtwPos);

                int a = facade.GetPoiByTag("Bildung").Count();
                int b = facade.GetPoiByTag("Spiegel").Count();

                Assert.IsTrue(2 == a);
                Assert.IsTrue(1 == b);
            }
        }
    }
}
