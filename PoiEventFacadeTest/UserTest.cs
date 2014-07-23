using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PoiEventNetwork.Data.Context;
using PoiEventNetwork.Data.Interface;
using PoiEventNetwork.Facade;

using Microsoft.Practices.Unity;
using PoiEventNetwork;
using PoiEventNetwork.Facade.Interface;

namespace PoiEventFacadeTest
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class UserTest : TestInit
    {
        [TestMethod]
        [Complete]
        public void RolesTest()
        {
            using (IPoiEventFacade facade = Resolver.Resolve<IPoiEventFacade>())
            {
                long id = facade.CreateUser("TestRoles", "ober", "TestRoles@htw-berlin.de");

                Assert.IsFalse(facade.HasAdminRole(id));

                facade.SetAdminRole(id);

                Assert.IsTrue(facade.HasAdminRole(id));

                facade.RemAdminRole(id);

                Assert.IsFalse(facade.HasAdminRole(id));                
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void UserExistsTest()
        {
            using (IPoiEventFacade facade = Resolver.Resolve<IPoiEventFacade>())
            {
                long uid1 = facade.CreateUser("admin", "ober", "TestUserExists@htw-berlin.de");
                long uid2 = facade.CreateUser("admin", "ober", "TestUserExists@htw-berlin.de");
            }
        }

        [TestMethod]
        public void DeleteByIdTest()
        {
            using (IPoiEventFacade facade = Resolver.Resolve<IPoiEventFacade>())
            {
                const string email = "TestDeleteById@htw-berlin.de";

                long uid = facade.CreateUser("Joe", "Doe", email);

                Assert.AreEqual(uid, facade.GetUserId(email));

                facade.DeleteUser(uid);

                try
                {
                    Assert.IsNull(facade.GetUserId(email));
                }
                catch (ArgumentException ex)
                {
                    Assert.AreEqual("email", ex.ParamName);
                }
            }
        }

        [TestMethod]
        [Complete]
        public void DeleteByEmailTest()
        {
            using (IPoiEventFacade facade = Resolver.Resolve<IPoiEventFacade>())
            {
                const string email = "TestDeleteByEmail@htw-berlin.de";

                long uid = facade.CreateUser("Joe", "Doe", email);

                Assert.AreEqual(uid, facade.GetUserId(email));

                try
                {
                    facade.GetUserId(email);
                }
                catch (ArgumentException ex)
                {
                    Assert.AreEqual("email", ex.ParamName);
                }
            }
        }

        [TestMethod]
        [Complete]
        [ExpectedException(typeof(ArgumentException))]
        public void DeleteByIllegalEmailTest()
        {
            using (IPoiEventFacade facade = Resolver.Resolve<IPoiEventFacade>())
            {
                facade.DeleteUser("thismailisnotvalid@htw-berlin.de");
            }
        }

        [TestMethod]
        [Complete]
        [ExpectedException(typeof(ArgumentException))]
        public void DeleteByIllegalIdTest()
        {
            using (IPoiEventFacade facade = Resolver.Resolve<IPoiEventFacade>())
            {
                facade.DeleteUser(9999);
            }
        }

        [TestMethod]
        [Complete]
        [ExpectedException(typeof(ArgumentException))]
        public void SetAdminRoleOfNoExistingUserTest()
        {
            using (IPoiEventFacade facade = Resolver.Resolve<IPoiEventFacade>())
            {
                facade.SetAdminRole(9999);
            }
        }

        [TestMethod]
        [Complete]
        [ExpectedException(typeof(ArgumentException))]
        public void HasAdminRoleOfNoExistingUserTest()
        {
            using (IPoiEventFacade facade = Resolver.Resolve<IPoiEventFacade>())
            {
                facade.HasAdminRole(9999);
            }
        }
    }
}
