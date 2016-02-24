using System.Web.Mvc;
using BoardGames.Models;
using BoardGames.Web.Controllers;
using BoardGames.Web.Tests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BoardGames.Web.Tests.Controllers
{
    [TestClass]
    public class RoomControllerTest
    {
        [TestMethod]
        public void TestDetailsView()
        {
            var controller = new RoomController(new RepositoryMock<Room>());
            var result = controller.Index() as ViewResult;
            Assert.AreEqual("Index", result.ViewName);

        }
    }
}
