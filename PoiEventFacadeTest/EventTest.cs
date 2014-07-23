using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PoiEventNetwork.Data;
using PoiEventNetwork;
using PoiEventNetwork.Data.Exception;
using PoiEventNetwork.Data.Interface;
using PoiEventNetwork.Facade;
using Microsoft.Practices.Unity;
using PoiEventNetwork.Facade.Interface;

namespace PoiEventFacadeTest
{
	[TestClass]
    [ExcludeFromCodeCoverage]
	public class EventTest
	{
        protected Coordinate     HtwPos = new Coordinate(23, 24);

	    [ClassInitialize]
		public static void ClassInitialize(TestContext testCtx)
	    {
            using (IDataContext ctx = Resolver.Resolve<IDataContext>())
            {
                ctx.Database.Delete();
            }
		}

        [TestMethod]
        [ExpectedException(typeof(AuthorizationException))]
        public void DeleteWithoutPermissionsTest()
        {
            using (IPoiEventFacade facade = Resolver.Resolve<IPoiEventFacade>())
            {
                long user1Id = facade.CreateUser("Joe", "Doe",
                                                 String.Format("{0}1@gmail.com", MethodBase.GetCurrentMethod().Name));
                facade.SetAdminRole(user1Id);

                long user2Id = facade.CreateUser("Joe", "Doe",
                                                 String.Format("{0}2@gmail.com", MethodBase.GetCurrentMethod().Name));

                string poiname = string.Format("Htw Berlin Event Test {0}", MethodBase.GetCurrentMethod().Name);
                facade.CreateBasePoi(user1Id, poiname, new[] {"HTW", "Berlin", "AI"}, HtwPos);

                long eventId = facade.CreateEvent(user1Id, poiname, "Code Rage", "Hosted by Google.");

                facade.DeleteEvent(user2Id, eventId);
            }
        }

	    [TestMethod]
	    public void DeleteAsAdminTest()
	    {
	        using (IPoiEventFacade facade = Resolver.Resolve<IPoiEventFacade>())
	        {
	            long user1Id = facade.CreateUser("Joe", "Doe",
	                                             String.Format("{0}1@gmail.com", MethodBase.GetCurrentMethod().Name));
	            facade.SetAdminRole(user1Id);

	            string poiname = string.Format("Htw Berlin Event Test {0}", MethodBase.GetCurrentMethod().Name);
	            facade.CreateBasePoi(user1Id, poiname, new[] {"HTW", "Berlin", "AI"}, HtwPos);

	            long eventId = facade.CreateEvent(user1Id, poiname, "Code Rage", "Hosted by Google.");
	            facade.CreateEvent(user1Id, poiname, "Code Rage", "Hosted by Microsoft.");


	            Assert.AreEqual(2, facade.FindEventsForPoi(poiname).Length);
	            facade.DeleteEvent(user1Id, eventId);
	            Assert.AreEqual(1, facade.FindEventsForPoi(poiname).Length);
	        }
	    }

	    [TestMethod]
	    public void CreateDeleteTest()
	    {
	        using (IPoiEventFacade facade = Resolver.Resolve<IPoiEventFacade>())
	        {
	            long user1Id = facade.CreateUser("Joe", "Doe",
	                                             String.Format("{0}1@gmail.com", MethodBase.GetCurrentMethod().Name));
	            facade.SetAdminRole(user1Id);

	            string poiname = string.Format("Htw Berlin Event Test {0}", MethodBase.GetCurrentMethod().Name);
	            facade.CreateBasePoi(user1Id, poiname, new[] {"HTW", "Berlin", "AI"}, HtwPos);

	            long eventId = facade.CreateEvent(user1Id, poiname, "Code Rage", "Hosted by Google.");
	            facade.CreateEvent(user1Id, poiname, "Code Rage", "Hosted by Microsoft.");

	            Event[] eventsonpoi = facade.FindEventsForPoi(poiname);
	            Assert.AreEqual(2, eventsonpoi.Length);

	            Event[] eventsofusr = facade.FindOwnedEvents(user1Id);
	            Assert.AreEqual(2, eventsofusr.Length);

	            facade.DeleteEvent(user1Id, eventId);
	            Assert.AreEqual(1, facade.FindEventsForPoi(poiname).Length);
	        }
	    }

	    [TestMethod]
	    public void CascadeDeleteTest()
	    {
	        using (IPoiEventFacade facade = Resolver.Resolve<IPoiEventFacade>())
	        {
	            long user1Id = facade.CreateUser("Joe", "Doe",
	                                             String.Format("{0}1@gmail.com", MethodBase.GetCurrentMethod().Name));
	            facade.SetAdminRole(user1Id);

	            string poiname = string.Format("Htw Berlin Event Test {0}", MethodBase.GetCurrentMethod().Name);
	            facade.CreateBasePoi(user1Id, poiname, new[] {"HTW", "Berlin", "AI"}, HtwPos);

	            long eventId = facade.CreateEvent(user1Id, poiname, "Code Rage", "Hosted by Google.");

                facade.AddMessage(eventId, user1Id, "New Msg Title", "Important Content.");

	            facade.DeleteEvent(user1Id, eventId);

	            Assert.AreEqual(0, facade.FindOwnedEvents(user1Id).Length);
	        }
	    }

	    [TestMethod]
	    public void FindOwnedEventsTest()
	    {
	        using (IPoiEventFacade facade = Resolver.Resolve<IPoiEventFacade>())
	        {
	            long user1Id = facade.CreateUser("Joe", "Doe",
	                                             String.Format("{0}1@gmail.com", MethodBase.GetCurrentMethod().Name));
	            facade.SetAdminRole(user1Id);

	            string poiname = string.Format("Htw Berlin Event Test {0}", MethodBase.GetCurrentMethod().Name);
	            facade.CreateBasePoi(user1Id, poiname, new[] {"HTW", "Berlin", "AI"}, HtwPos);

	            facade.CreateEvent(user1Id, poiname, "Code Rage", "Hosted by Google.");
	            facade.CreateEvent(user1Id, poiname, "Code Rage", "Hosted by Microsoft.");

	            Event[] events = facade.FindOwnedEvents(user1Id);
	            Assert.AreEqual(2, events.Length);
	        }
	    }

	    [TestMethod]
	    public void SubscribeTest()
	    {
	        using (IPoiEventFacade facade = Resolver.Resolve<IPoiEventFacade>())
	        {
	            long user1Id = facade.CreateUser("Joe", "Doe",
	                                             String.Format("{0}1@gmail.com", MethodBase.GetCurrentMethod().Name));
	            facade.SetAdminRole(user1Id);

	            string poiname = string.Format("Htw Berlin Event Test {0}", MethodBase.GetCurrentMethod().Name);
	            facade.CreateBasePoi(user1Id, poiname, new[] {"HTW", "Berlin", "AI"}, HtwPos);

	            long event1Id = facade.CreateEvent(user1Id, poiname, "Code Rage", "Hosted by Google.");
	            long event2Id = facade.CreateEvent(user1Id, poiname, "Code Rage", "Hosted by Microsoft.");

	            facade.SubscribeToEvent(user1Id, event1Id);
	            Assert.AreEqual(1, facade.FindSubscribedEvents(user1Id).Length);
	            facade.SubscribeToEvent(user1Id, event2Id);
	            Assert.AreEqual(2, facade.FindSubscribedEvents(user1Id).Length);
	        }
	    }

	    [TestMethod]
	    public void AddMessagesTest()
	    {
	        using (IPoiEventFacade facade = Resolver.Resolve<IPoiEventFacade>())
	        {
	            long user1Id = facade.CreateUser("Joe", "Doe",
	                                             String.Format("{0}1@gmail.com", MethodBase.GetCurrentMethod().Name));
	            facade.SetAdminRole(user1Id);

	            string poiname = string.Format("Htw Berlin Event Test {0}", MethodBase.GetCurrentMethod().Name);
	            facade.CreateBasePoi(user1Id, poiname, new[] {"HTW", "Berlin", "AI"}, HtwPos);

	            long event1Id = facade.CreateEvent(user1Id, poiname, "Code Rage", "Hosted by Google.");

	            Message[] msgs = facade.GetMessages(event1Id);

	            Assert.AreEqual(0, msgs.Length);

	            facade.AddMessage(event1Id, user1Id, "Nachricht", "zu empfehlen");
	            Assert.AreEqual(1, facade.GetMessages(event1Id).Length);
	        }
	    }

	    [TestMethod]
	    public void FindMessagesTest()
	    {
	        using (IPoiEventFacade facade = Resolver.Resolve<IPoiEventFacade>())
	        {
	            long user1Id = facade.CreateUser("Joe", "Doe",
	                                             String.Format("{0}1@gmail.com", MethodBase.GetCurrentMethod().Name));
	            facade.SetAdminRole(user1Id);

	            string poiname = string.Format("Htw Berlin Event Test {0}", MethodBase.GetCurrentMethod().Name);
	            facade.CreateBasePoi(user1Id, poiname, new[] {"HTW", "Berlin", "AI"}, HtwPos);

	            long event1Id = facade.CreateEvent(user1Id, poiname, "Code Rage", "Hosted by Google.");

	            facade.AddMessage(event1Id, user1Id, "Nachricht", "zu empfehlen");
	            Assert.AreEqual("Nachricht", facade.GetMessages(event1Id)[0].Name);
	        }
	    }

	    [TestMethod]
	    [ExpectedException(typeof (ArgumentException))]
	    public void DeleteMessagesWithInvalidUserIdTest()
	    {
	        using (IPoiEventFacade facade = Resolver.Resolve<IPoiEventFacade>())
	        {
	            long user1Id = facade.CreateUser("Joe", "Doe",
	                                             String.Format("{0}1@gmail.com", MethodBase.GetCurrentMethod().Name));
	            facade.SetAdminRole(user1Id);

	            string poiname = string.Format("Htw Berlin Event Test {0}", MethodBase.GetCurrentMethod().Name);
	            facade.CreateBasePoi(user1Id, poiname, new[] {"HTW", "Berlin", "AI"}, HtwPos);

	            long event1Id = facade.CreateEvent(user1Id, poiname, "Code Rage", "Hosted by Google.");

	            facade.DeleteEvent(746374645734L, event1Id);
	        }
	    }
	}
}
