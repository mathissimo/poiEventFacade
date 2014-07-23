using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PoiEventFacadeTest
{
    public class TestUserInit : TestInit
    {
        protected long AdminId { get; set; }
        protected long UserId { get; set; }

        [TestInitialize]
        public void TestInitialize()
        {
            AdminId = MyPoiEvent.CreateUser("admin", "ober", "admin@htw-berlin.de");
            MyPoiEvent.SetAdminRole(AdminId);
            UserId = MyPoiEvent.CreateUser("user", "normal", "user@htw-berlin.de");
            //@Before
            //public void setUp(){
            //    adminId = poiEvent.createUser("admin", "ober", "admin@htw-berlin.de");
            //    poiEvent.setAdminRole(adminId);
            //    userId = poiEvent.createUser("user", "normal", "user@htw-berlin.de");
            //}
        }

        [TestCleanup]
        public void TestCleanup()
        {
            try
            {
                MyPoiEvent.DeleteUser("admin@htw-berlin.de");
                MyPoiEvent.DeleteUser("user@htw-berlin.de");
            }
            catch (Exception)
            {

            }

            //@After 
            //public void tearDown(){
            //    poiEvent.deleteUser("admin@htw-berlin.de");
            //    poiEvent.deleteUser("user@htw-berlin.de");
            //}
        }
    }
}
