namespace Alpha.Maintenance.Controller.Tests
{
    using System.Web.Mvc;
    using NSubstitute;
    using NUnit.Framework;
    using Service.Interface.IPInformation;

    [TestFixture]
    public class IPInformationControllerTests
    {
        private IPInformationService ipInformationService;
        private IPInformationController ipInformationController;

        [SetUp]
        public void SetUp()
        {
            ipInformationService = Substitute.For<IPInformationService>();
            ipInformationController = new IPInformationController(ipInformationService);
        }

        [Test]
        public void Index_Always_ReturnsViewResult()
        {
            // Act
            var result = ipInformationController.Index();

            // Assert
            Assert.IsTrue(result is ViewResult);
        }

        [Test]
        public void GetIPInfoFromDB_Always_CallsGetIPInfo()
        {
            // Act
            ipInformationController.GetIPInfo("127.0.0.1");

            // Assert
            ipInformationService.Received().GetIPInfo(Arg.Is("127.0.0.1"));
        }
    }
}
