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
using System.Collections.ObjectModel;
using PoiEventNetwork.Data.POI;

namespace PoiEventImplementationTest
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class EventTest : TestInit
    {
        private static String TestEventName = "TestEvent";
        private static String TestEventDescription = "Description of TestEvent";
        private static ObservableCollection<Message> TestEventMessages = new ObservableCollection<Message>();
        private static ObservableCollection<User> TestEventUsers = new ObservableCollection<User>();
        private static User TestEventCreator = new User() { Mail = "testcreator1@gmail.com" };


        [ClassInitialize]
        public static void ClassInitialize(TestContext testCtx)
        {
            var HtwTagsCol = TagCollectionForStringArray(HtwTags);

            Message TestMessage1 = new Message() {Name="TestMessage_Title1", Text="TestMessage_Test1"};
            Message TestMessage2 = new Message() {Name="TestMessage_Title2", Text="TestMessage_Test2"};
            TestEventMessages.Add(TestMessage1);
            TestEventMessages.Add(TestMessage2);

            User User1 = new User() { Mail = "testUser1@gmail.com" };
            User User2 = new User() { Mail = "testUser2@gmail.com" };
            TestEventUsers.Add(User1);
            TestEventUsers.Add(User2);
        }

        [TestMethod]
        public void TestEventDataIntegrity()
        {
            NormPoi testPoi = new NormPoi() {Name = HtwName, Location = HtwPos};
            Event testEvent = new Event() { 
                Name = TestEventName, 
                Desc = TestEventDescription,
                Location = testPoi,
                Creator = TestEventCreator
                };
            testEvent.overrideMessages(TestEventMessages);
            testEvent.overrideUsers(TestEventUsers);

            Assert.AreEqual(testEvent.Users, TestEventUsers);
            Assert.AreEqual(testEvent.Messages, TestEventMessages);
            Assert.AreEqual(testEvent.Location, testPoi);
            Assert.AreEqual(testEvent.Name, TestEventName);
            Assert.AreEqual(testEvent.Desc, TestEventDescription);
        }
    }
}
