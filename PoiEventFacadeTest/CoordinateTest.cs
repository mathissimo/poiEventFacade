using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PoiEventNetwork.Data;

namespace PoiEventFacadeTest
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class CoordinateTest : TestInit
    {
        [TestMethod]
        [ExpectedException(typeof (ValidationException))]
        public void ConstructorValidationTest()
        {
            Coordinate xy = new Coordinate(-180.1f, 300);
        }

        [TestMethod]
        public void ConstructorValidation2Test()
        {
            Coordinate xy = new Coordinate(-11.1f, 33);
        }
    }
}
