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
    public class NgonPoiTest : TestInit
    {
        private static ObservableCollection<Tag> HtwTagsCol;
        static Coordinate Valid1 = new Coordinate(30.03f, 50.3f);
        static Coordinate Valid2 = new Coordinate(31.03f, 51.3f);
        static Coordinate Valid3 = new Coordinate(31.03f, 50.3f);

        Coordinate[] ValidPolygon = new[] { Valid1, Valid2, Valid3 };


        [ClassInitialize]
        public static void ClassInitialize(TestContext testCtx)
        {
            HtwTagsCol = TagCollectionForStringArray(HtwTags);
        }


        [TestMethod]
        public void TestNgonPoiDataIntegrity()
        {
            NgonPoi testPoi = new NgonPoi(HtwName,ValidPolygon,HtwTagsCol);
            Assert.AreEqual(testPoi.Name, HtwName);
            Assert.IsTrue(TagColEqualsStringArray(testPoi.Tags, HtwTags));
            for (int i = 0; i < ValidPolygon.Length; i++)
            {
                Assert.AreEqual(ValidPolygon[i], testPoi.Polygon[i]);
            }
        }
    }
}
