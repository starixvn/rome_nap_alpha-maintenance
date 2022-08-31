namespace Alpha.Maintenance.Repository.Tests.Impl
{
    using System.Data;
    using Core.Model;
    using Database.IPInformation;
    using Fixtures;
    using KellermanSoftware.CompareNetObjects;
    using NSubstitute;
    using NUnit.Framework;
    using Repository.Criteria.IPInformation;
    using Repository.Impl;

    [TestFixture]
    public class IPInformationRepositoryImplTests
    {
        private CompareLogic compareLogic;
        private ObjectDbFixture objectDbFixture;
        private IPInformationRepositoryImpl ipInformationRepository;

        [SetUp]
        public void SetUp()
        {
            compareLogic = new CompareLogic();
            objectDbFixture = new ObjectDbFixture();
            ipInformationRepository = new IPInformationRepositoryImpl();
        }

        [Test]
        public void GetIPInfo_NullDataReturnFromDb_ReturnsNull()
        {
            // Arrange
            var criteria = new GetIPInformationCriteria("127.0.0.1");
            IDataReader dataReader = null;
            objectDbFixture.ObjectDb.ExecuteReader(Arg.Any<GetIPInformationCriteria>()).Returns(dataReader);

            // Act
            var result = ipInformationRepository.GetIPInfo(criteria);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public void GetIPInfo_HasData_ReturnsFromObjectDb()
        {
            // Arrange
            var criteria = new GetIPInformationCriteria("127.0.0.1");
            IDataReader dataReader = IPInformationDatabase.IPInformation.CreateDataReader();
            objectDbFixture.ObjectDb.ExecuteReader(Arg.Any<GetIPInformationCriteria>()).Returns(dataReader);

            var expectedResult = new IPInfo("IPDB", "VN", "VIET NAM", "HO CHI MINH", "HO CHI MINH CITY","#");

            // Act
            var actualResult = ipInformationRepository.GetIPInfo(criteria);

            // Assert
            Assert.IsTrue(compareLogic.Compare(expectedResult, actualResult).AreEqual);
        }
    }
}
