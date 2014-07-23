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
using System.Collections.ObjectModel;

namespace PoiEventImplementationTest
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class CityPoiTest : TestInit
    {
        private static ObservableCollection<Tag> HtwTagsCol;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testCtx)
        {
            HtwTagsCol = TagCollectionForStringArray(HtwTags);
        }


        [TestMethod]
        public void TestCityPoiDataIntegrity()
        {
            ObservableCollection<Event> testEvents = CreateTestEvents();
            CityPoi testPoi = new CityPoi(HtwName, HtwTagsCol) { Street = HtwStreet, City = HtwStadt, Location = HtwPos};
            testPoi.overrideEvents(testEvents);
            Assert.AreEqual(testPoi.City, HtwStadt);
            Assert.AreEqual(testPoi.Street, HtwStreet);
            Assert.AreEqual(testPoi.Name, HtwName);
            Assert.AreEqual(testPoi.Location, HtwPos);
            Assert.AreEqual(testPoi.Events, testEvents);
            Assert.IsTrue(TagColEqualsStringArray(testPoi.Tags, HtwTags));
        }
    }
}
