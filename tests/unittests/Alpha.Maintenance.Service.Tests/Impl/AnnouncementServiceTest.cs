using System;
using System.Collections.Generic;
using System.Linq;
using Alpha.Announcements.Models;
using Alpha.Announcements.Services;
using Alpha.Announcements.Specs;
using Alpha.Maintenance.Core;
using Alpha.Maintenance.Repository.Criteria.Announcements;
using Alpha.Maintenance.Repository.DTOs;
using Alpha.Maintenance.Service.Impl.Announcement;
using Alpha.Maintenance.Service.Interface.Announcement;
using Fanex.Data.Repository;
using NSubstitute;
using NUnit.Framework;

[TestFixture]
public class AnnouncementServiceImplTests
{
    private AnnouncementServiceImpl _sut;
    private SpecificationFactory _specificationFactory;
    private IDynamicRepository _dynamicRepository;
    private BaseAnnouncementService _baseAnnouncementService;
    private IEnumerable<string> _languages;
    private IEnumerable<string> _excludedSpecifications;
    private bool _getAnnouncementsByJson;

    [SetUp]
    public void Setup()
    {
        // Initialize mocks
        _specificationFactory = Substitute.For<SpecificationFactory>();
        _dynamicRepository = Substitute.For<IDynamicRepository>();
        _baseAnnouncementService = Substitute.For<BaseAnnouncementService>();

        // Setup test data
        _languages = new[] { "en-US", "vi-VN" };
        _excludedSpecifications = new List<string>();
        _getAnnouncementsByJson = false;

        // Create system under test
        _sut = new AnnouncementServiceImpl(
            _specificationFactory,
            _dynamicRepository,
            _baseAnnouncementService,
            _languages,
            _excludedSpecifications,
            _getAnnouncementsByJson
        );
    }

    [Test]
    public void GetOfferedSpecifications_ShouldReturnFilteredSpecs()
    {
        // Arrange
        var factorySpecs = new List<ISpecification<IParam>>();
        _baseAnnouncementService.GetFactorySpecifications().Returns(factorySpecs);

        // Act
        var result = _sut.GetOfferedSpecifications();

        // Assert
        _baseAnnouncementService.Received(1).GetFactorySpecifications();
        _baseAnnouncementService.Received(1).FiterOfferedSpecs(factorySpecs, _excludedSpecifications);
    }

    [Test]
    public void GetAnnouncements_WhenUsingDatabase_ShouldFetchAndMapAnnouncements()
    {
        // Arrange
        var criteria = new SelectAnnouncementsCriteria();
        var dtoList = new List<AnnouncementDTO> { CreateSampleAnnouncementDTO() };
        _dynamicRepository.Fetch<AnnouncementDTO>(criteria).Returns(dtoList);

        // Act
        var result = _sut.GetAnnouncements(criteria).ToList();

        // Assert
        Assert.That(result, Has.Count.EqualTo(1));
        _dynamicRepository.Received(1).Fetch<AnnouncementDTO>(criteria);
        _baseAnnouncementService.Received(1).FiterOfferedSpecs(
            Arg.Any<IEnumerable<ISpecification<IParam>>>(),
            _excludedSpecifications
        );
    }

    [Test]
    public void GetAnnouncementById_ShouldReturnMappedAnnouncement()
    {
        // Arrange
        var criteria = new SelectAnnouncementByIdCriteria { Id = 1 };
        var dto = CreateSampleAnnouncementDTO();
        _dynamicRepository.Get<AnnouncementDTO>(criteria).Returns(dto);

        // Act
        var result = _sut.GetAnnouncementById(criteria);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Id, Is.EqualTo(dto.Id));
        _dynamicRepository.Received(1).Get<AnnouncementDTO>(criteria);
        _baseAnnouncementService.Received(1).CorrectContent(
            Arg.Any<IEnumerable<MessageContent>>(),
            _languages
        );
    }

    [Test]
    public void SaveAnnouncement_ShouldReturnSuccessResult()
    {
        // Arrange
        var criteria = new SaveAnnouncementsCriteria();
        criteria.ErrCode = 0;

        // Act
        var result = _sut.SaveAnnouncement(criteria);

        // Assert
        Assert.That(result.Code, Is.EqualTo(0));
        Assert.That(result.Message, Is.EqualTo(AnnouncementResources.SaveSuccessfully));
        _dynamicRepository.Received(1).Execute(criteria);
    }

    [Test]
    public void SaveAnnouncement_WhenError_ShouldReturnErrorResult()
    {
        // Arrange
        var criteria = new SaveAnnouncementsCriteria();
        criteria.ErrCode = 1;
        criteria.ErrMsg = "Test error";

        // Act
        var result = _sut.SaveAnnouncement(criteria);

        // Assert
        Assert.That(result.Code, Is.EqualTo(1));
        Assert.That(result.Message, Is.EqualTo("Test error"));
        _dynamicRepository.Received(1).Execute(criteria);
    }

    [Test]
    public void GetDefaultAnnouncement_ShouldReturnCorrectDefaultValues()
    {
        // Act
        var result = _sut.GetDefaultAnnouncement();

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.ReleasedDate, Is.EqualTo(SystemTime.Today));
        Assert.That(result.ExpiredDate, Is.EqualTo(SystemTime.Today));
        Assert.That(result.IsActive, Is.False);
        Assert.That(result.Contents.Count(), Is.EqualTo(_languages.Count()));
        Assert.That(result.Specifications, Is.Empty);
    }

    private AnnouncementDTO CreateSampleAnnouncementDTO()
    {
        return new AnnouncementDTO
        {
            Id = 1,
            Code = "TEST001",
            Title = "Test Announcement",
            AnnouncementType = "1",
            FromDate = DateTime.Today,
            ToDate = DateTime.Today.AddDays(7),
            IsActive = true,
            Contents = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes("[]")),
            Specifications = "[]",
            Environments = "Dev,Prod",
            Systems = "System1,System2",
            CreatedDate = DateTime.Today,
            CreatorId = 1,
            CreatorName = "TestUser"
        };
    }

    [Test]
    public void Constructor_WithDefaultParameters_ShouldCreateInstance()
    {
        // Act
        var service = new AnnouncementServiceImpl();

        // Assert
        Assert.That(service, Is.Not.Null);
    }

    [Test]
    public void GetAnnouncements_WithJsonEnabled_ShouldReadFromJsonFile()
    {
        // Arrange
        _getAnnouncementsByJson = true;
        _sut = new AnnouncementServiceImpl(
            _specificationFactory,
            _dynamicRepository,
            _baseAnnouncementService,
            _languages,
            _excludedSpecifications,
            _getAnnouncementsByJson
        );

        // Act & Assert
        Assert.Throws<System.IO.FileNotFoundException>(() =>
            _sut.GetAnnouncements(new SelectAnnouncementsCriteria())
        );
    }
}