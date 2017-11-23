namespace Alpha.Maintenance.Service.Tests.Impl
{
    using NSubstitute;
    using NUnit.Framework;
    using Repository.Criteria.IPInformation;
    using Repository.Impl;
    using Service.Impl.IPInformation;

    [TestFixture]
    public class IPInformationServiceImplTests
    {
        private IPInformationRepository ipInformationRepository;
        private IPInformationServiceImpl ipInformationService;

        [SetUp]
        public void SetUp()
        {
            ipInformationRepository = Substitute.For<IPInformationRepository>();
            ipInformationService = new IPInformationServiceImpl(ipInformationRepository);
        }

        [Test]
        public void GetIPInfoFromDB_Always_CallsGetIPInfo()
        {
            // Arrange
            var ip = "127.0.0.1";
            var criteria = new GetIPInformationCriteria(ip);

            // Act
            ipInformationService.GetIPInfoFromDB(ip);

            // Assert
            ipInformationRepository.Received().GetIPInfo(
                Arg.Is<GetIPInformationCriteria>(param => param.IP.Equals(criteria.IP)));
        }
    }
}
