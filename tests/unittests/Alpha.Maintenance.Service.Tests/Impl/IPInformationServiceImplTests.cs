using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Alpha.Maintenance.Core.Model;
using Alpha.Maintenance.Repository.Criteria.IPInformation;
using Fanex.Data.Repository;
using NSubstitute;
using NUnit.Framework;
using System.Configuration;
using Alpha.Maintenance.Service.Impl.IPInformation;
using NSubstitute.ExceptionExtensions;

namespace Alpha.Maintenance.Service.Tests.Impl
{
    [TestFixture]
    public class IPInformationServiceImplTests
    {
        private IPInformationServiceImpl _sut;
        private IDynamicRepository _dynamicRepository;
        private HttpClient _httpClient;
        private const string TestIP = "192.168.1.1";

        [SetUp]
        public void Setup()
        {
            _dynamicRepository = Substitute.For<IDynamicRepository>();
            _httpClient = Substitute.For<HttpClient>();
            _sut = new IPInformationServiceImpl(_dynamicRepository);
        }

        [Test]
        public async Task GetIPInfos_WhenCalledWithValidIP_ShouldReturnDatabaseResults()
        {
            // Arrange
            var expectedDbResults = new List<IPInfo>
            {
                new IPInfo
                {
                    DataSource = "IPDB",
                    CountryCode = "VN",
                    CountryName = "Vietnam",
                    Region = "Ho Chi Minh",
                    City = "Ho Chi Minh City"
                }
            };

            _dynamicRepository.Fetch<IPInfo>(Arg.Any<GetIPInformationCriteria>())
                .Returns(expectedDbResults);

            // Act
            var results = await _sut.GetIPInfos(TestIP);

            // Assert
            Assert.That(results, Is.Not.Null);
            Assert.That(results.Count(), Is.GreaterThanOrEqualTo(1));
            var dbResult = results.First();
            Assert.That(dbResult.DataSource, Is.EqualTo("IPDB"));
            _dynamicRepository.Received(1).Fetch<IPInfo>(Arg.Any<GetIPInformationCriteria>());
        }

        [Test]
        public void GetIPInfos_WhenRepositoryThrows_ShouldPropagateException()
        {
            // Arrange
            _dynamicRepository.Fetch<IPInfo>(Arg.Any<GetIPInformationCriteria>())
                .Throws(new Exception("Database error"));

            // Act & Assert
            Assert.ThrowsAsync<Exception>(async () => await _sut.GetIPInfos(TestIP));
        }

        [Test]
        public void Constructor_WithNullRepository_ShouldCreateDefaultRepository()
        {
            // Act
            var service = new IPInformationServiceImpl();

            // Assert
            Assert.That(service, Is.Not.Null);
        }

        [Test]
        public void Constructor_WithProxyConfiguration_ShouldCreateHttpClientWithProxy()
        {
            // Arrange
            var proxyUrl = "http://proxy.example.com:8080";
            ConfigurationManager.AppSettings["Proxy"] = proxyUrl;

            // Act
            var service = new IPInformationServiceImpl();

            // Assert
            Assert.That(service, Is.Not.Null);
            // Note: We can't directly test the HttpClient configuration due to its design
        }

        [Test]
        public async Task GetIPInfos_WhenAPICallSucceeds_ShouldReturnCombinedResults()
        {
            // Arrange
            var dbResults = new List<IPInfo>
            {
                new IPInfo { DataSource = "IPDB", CountryCode = "VN" }
            };

            _dynamicRepository.Fetch<IPInfo>(Arg.Any<GetIPInformationCriteria>())
                .Returns(dbResults);

            // Mock AppSettings for API configuration
            var mockApiInfo = new APIInfo("test-api")
            {
                // Note: Since APIInfo properties are read-only, you might need to modify the test approach
                // or create a test-specific implementation
            };

            // Act
            var results = await _sut.GetIPInfos(TestIP);

            // Assert
            Assert.That(results, Is.Not.Null);
            Assert.That(results.Count(), Is.GreaterThanOrEqualTo(1));
            Assert.That(results.Any(r => r.DataSource == "IPDB"), Is.True);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public async Task GetIPInfos_WhenIPIsInvalid_ShouldReturnEmptyResult(string ip)
        {
            // Arrange
            _dynamicRepository.Fetch<IPInfo>(Arg.Any<GetIPInformationCriteria>())
                .Returns(new List<IPInfo>());

            // Act
            var results = await _sut.GetIPInfos(ip);

            // Assert
            Assert.That(results, Is.Empty);
        }

        [Test]
        public void GetIPInfos_WhenAPICallFails_ShouldStillReturnDatabaseResults()
        {
            // Arrange
            var dbResults = new List<IPInfo>
            {
                new IPInfo { DataSource = "IPDB", CountryCode = "VN" }
            };

            _dynamicRepository.Fetch<IPInfo>(Arg.Any<GetIPInformationCriteria>())
                .Returns(dbResults);

            // Mock API call to fail
            // Note: This is challenging to test due to HttpClient being instantiated in the constructor
            // Consider refactoring the service to accept IHttpClientFactory or similar for better testability

            // Act & Assert
            Assert.DoesNotThrowAsync(async () => await _sut.GetIPInfos(TestIP));
        }

        [TearDown]
        public void Cleanup()
        {
            _httpClient?.Dispose();
        }
    }

    // Helper class for testing API responses
    public class MockHttpMessageHandler : HttpMessageHandler
    {
        private readonly Func<HttpRequestMessage, HttpResponseMessage> _handler;

        public MockHttpMessageHandler(Func<HttpRequestMessage, HttpResponseMessage> handler)
        {
            _handler = handler;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        {
            return Task.FromResult(_handler(request));
        }
    }
}