using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PoiEventNetwork.Data.Exception;
using PoiEventNetwork.Facade;
using PoiEventNetwork.Facade.Interface;
using PoiEventNetwork;
using Microsoft.Practices.Unity;

namespace PoiEventFacadeTest
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class DeletePoiTest : TestInit
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
        public void TestDeletePoi()
        {
            using (IPoiEventFacade facade = Resolver.Resolve<IPoiEventFacade>())
            {
                // create Poi and Check if successfull
                String testPoiName = HtwName + " TestDeletePoi";
                facade.CreateBasePoi(AdminId, testPoiName, HtwTags, HtwPos);
                // TODO: AbsPoi oder BasePoi für actual?
                Assert.IsNotNull(facade.GetPoi(testPoiName));

                // delete Poi and check if successfull
                facade.DeletePoi(AdminId, testPoiName);
                Assert.IsNull(facade.GetPoi(testPoiName));
            }
        }

        [TestMethod]
        [ExpectedException(typeof (AuthorizationException))]
        public void TestDeletePoiWithoutPermission()
        {
            using (IPoiEventFacade facade = Resolver.Resolve<IPoiEventFacade>())
            {
                // create Poi and Check if successfull
                String testPoiName = HtwName + " TestDeletePoiWithoutPermission";
                facade.CreateBasePoi(AdminId, testPoiName, HtwTags, HtwPos);
                // TODO: AbsPoi oder BasePoi für actual?
                Assert.IsNotNull(facade.GetPoi(testPoiName));

                // try delete Poi with diffrent User
                long userId = facade.CreateUser("Default", "User", "TestDeletePoiWithoutPermission@gmail.com");
                facade.DeletePoi(userId, testPoiName);
            }
        }
    }
}
