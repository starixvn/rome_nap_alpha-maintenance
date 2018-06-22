namespace Alpha.Maintenance.Service.Tests.Impl
{
    using System.Collections.Generic;
    using System.Linq;
    using Announcements.Models;
    using Announcements.Services;
    using Announcements.Specs;
    using Constants;
    using Core;
    using Fanex.Data.Repository;
    using Interface.Announcement;
    using KellermanSoftware.CompareNetObjects;
    using NSubstitute;
    using NUnit.Framework;
    using Repository.Criteria.Announcements;
    using Repository.Impl;
    using Service.Impl.Announcement;

    [TestFixture]
    public class AnnouncementServiceTest
    {
        private SpecificationFactory specificationFactory;
        private AnnouncementRepository announcementRepository;

        private AnnouncementServiceImpl announcementServiceImpl;
        private BaseAnnouncementService baseAnnouncementService;
        private CompareLogic compareLogic;

        private Announcement announcementSample;
        private IEnumerable<string> languages;
        private bool getAnnouncementsByJson;

        [SetUp]
        public void SetUp()
        {
            specificationFactory = Substitute.For<SpecificationFactory>();
            announcementRepository = Substitute.For<AnnouncementRepository>();
            announcementRepository = Substitute.For<AnnouncementRepository>();
            baseAnnouncementService = Substitute.For<BaseAnnouncementService>();
            compareLogic = new CompareLogic();
            SystemTime.Today = DefaultVariables.CurrentDate;

            languages = new List<string> {
                                            "en-US",
                                            "vi-VN",
                                            "th-TH",
                                            "zh-CN",
                                            "zh-TW",
                                            "ko-KR",
                                            "ja-JP"
                                        };

            getAnnouncementsByJson = false;

            announcementSample = BuildAnnouncementSample();

            announcementServiceImpl = new AnnouncementServiceImpl(
                                                specificationFactory,
                                                announcementRepository,
                                                baseAnnouncementService,
                                                languages,
                                                new List<string>(),
                                                getAnnouncementsByJson);
        }

        [Test]
        public void GetAnnouncements_CallGetAnnouncementsRepository()
        {
            // Arrange
            var selectAnnouncementsCriteria = new SelectAnnouncementsCriteria();

            // Act
            announcementServiceImpl.GetAnnouncements(selectAnnouncementsCriteria);

            // Assert
            announcementRepository.Received().GetAnnouncements(Arg.Any<SelectAnnouncementsCriteria>());
        }

        [Test]
        public void GetAnnouncements_CallGetFactorySpecificationsFunction()
        {
            // Arrange && Act
            announcementServiceImpl.GetOfferedSpecifications();

            // Assert
            baseAnnouncementService.Received().GetFactorySpecifications();
        }

        [Test]
        public void GetAnnouncements_CallGetFiterOfferedSpecsFunction()
        {
            // Arrange 
            var factorySpecs = new List<ISpecification<IParam>>();
            baseAnnouncementService.GetFactorySpecifications().Returns(factorySpecs);

            // Act
            announcementServiceImpl.GetOfferedSpecifications();

            // Assert
            baseAnnouncementService.Received().FiterOfferedSpecs(Arg.Any<IEnumerable<ISpecification<IParam>>>(), Arg.Any<List<string>>());
        }

        [Test]
        public void GetAnnouncementById_CallGetAnnouncementByIdRepository()
        {
            // Arrange
            var selectAnnouncementByIdCriteria = new SelectAnnouncementByIdCriteria();
            announcementRepository.GetAnnouncementById(Arg.Any<SelectAnnouncementByIdCriteria>()).Returns(announcementSample);

            // Act
            announcementServiceImpl.GetAnnouncementById(selectAnnouncementByIdCriteria);

            // Assert
            announcementRepository.Received().GetAnnouncementById(Arg.Any<SelectAnnouncementByIdCriteria>());
        }

        [Test]
        public void GetAnnouncementById_CallCorrectContentFunction()
        {
            // Arrange
            var selectAnnouncementByIdCriteria = new SelectAnnouncementByIdCriteria();
            announcementRepository.GetAnnouncementById(Arg.Any<SelectAnnouncementByIdCriteria>())
                                   .Returns(announcementSample);

            // Act
            announcementServiceImpl.GetAnnouncementById(selectAnnouncementByIdCriteria);

            // Assert
            baseAnnouncementService.Received().CorrectContent(
                Arg.Any<IEnumerable<MessageContent>>(),
                Arg.Any<IEnumerable<string>>());
        }

        [Test]
        public void SaveAnnouncement_CallSaveAnnouncementRepository()
        {
            // Arrange && Act
            var criteria = new InsertAnnouncementsCriteria();
            announcementServiceImpl.SaveAnnouncement(criteria);

            // Assert
            announcementRepository.Received().SaveAnnouncement(Arg.Any<ICriteria>());
        }

        [Test]
        public void GetDefaultAnnouncement_ReturnCorrectAnnoucement()
        {
            // Arrange && Act
            var expectedData = announcementServiceImpl.GetDefaultAnnouncement();

            // Assert
            var result = compareLogic.Compare(expectedData, announcementSample);
            Assert.True(result.AreEqual);
        }

        private Announcement BuildAnnouncementSample()
            => new Announcement
            {   
                ReleasedDate = SystemTime.Today,                
                Environments = Enumerable.Empty<string>(),
                Systems = Enumerable.Empty<string>(),
                Contents = languages.Select(language => new MessageContent
                {
                    LanguageKey = language,
                    Text = string.Empty
                }),
                IsActive = false,
                Specifications = new List<ISpecification<IParam>>()
            };
    }
}
