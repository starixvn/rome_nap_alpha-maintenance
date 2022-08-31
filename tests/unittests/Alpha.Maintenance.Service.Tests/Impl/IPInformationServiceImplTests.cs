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
    }
}
