namespace Alpha.Maintenance.Repository.Test.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Alpha.Announcements.Client.Helpers;
    using Alpha.Announcements.Models;
    using Alpha.Announcements.Specs;
    using Criteria.Announcements;
    using Database.Announcements;
    using Fixtures;
    using KellermanSoftware.CompareNetObjects;
    using Newtonsoft.Json;
    using NSubstitute;
    using NUnit.Framework;
    using Repository.Impl;

    [TestFixture]
    public class AnnouncementRepositoryTest
    {
        private ObjectDbFixture objectDbFixture;
        private CompareLogic compareLogic;
        private AnnouncementRepository announcementRepository;
        private Announcement announcementSample;

        [SetUp]
        public void SetUp()
        {
            compareLogic = new CompareLogic();
            objectDbFixture = new ObjectDbFixture();
            announcementRepository = new AnnouncementRepositoryImpl();
            announcementSample = BuildAnnouncement();
        }

        [Test]
        public void GetAnnouncements_CriteriaIsNull_ThrowArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentNullException>(() => announcementRepository.GetAnnouncements(null));
        }

        [Test]
        public void GetAnnouncements_AnnouncementsHasData_ReturnsDataFromObjectDb()
        {
            // Arrange
            var selectAnnouncementsCriteria = new SelectAnnouncementsCriteria();

            var dataSet = AnnouncementsDatabase.Announcements.CreateDataReader();
            objectDbFixture.ObjectDb.ExecuteReader(Arg.Any<SelectAnnouncementsCriteria>()).Returns(dataSet);

            var expectedData = new List<Announcement>()
            {
                announcementSample
            };

            // Act
            var actualData = announcementRepository.GetAnnouncements(selectAnnouncementsCriteria);

            // Assert
            var result = compareLogic.Compare(expectedData, actualData);
            Assert.True(result.AreEqual);
        }

        [Test]
        public void GetAnnouncementById_CriteriaIsNull_ThrowArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentNullException>(() => announcementRepository.GetAnnouncementById(null));
        }

        [Test]
        public void GetAnnouncementById_AnnouncementsHasData_ReturnsDataFromObjectDb()
        {
            // Arrange
            var selectAnnouncementByIdCriteria = new SelectAnnouncementByIdCriteria();

            var dataSet = AnnouncementsDatabase.Announcements.CreateDataReader();
            objectDbFixture.ObjectDb.ExecuteReader(Arg.Any<SelectAnnouncementByIdCriteria>()).Returns(dataSet);

            // Act
            var actualData = announcementRepository.GetAnnouncementById(selectAnnouncementByIdCriteria);

            // Assert
            var result = compareLogic.Compare(announcementSample, actualData);
            Assert.True(result.AreEqual);
        }

        [Test]
        public void InsertorUpdateAnnouncement_CriteriaIsNull_ThrowArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentNullException>(() => announcementRepository.InsertOrUpdateAnnouncement(null));
        }

        private Announcement BuildAnnouncement()
        {
            var specs = @"[  
                           {  
                              'Value':[  
                                 1,
                                 2,
                                 3,
                                 4,
                                 5,
                                 6,
                                 7
                              ],
                              'Id':'E4FE1443-C8D0-4E63-A355-08661859295E',
                              'Name':'Products',
                              'Combinable':true,
                              'TypeName':'Alpha.Announcements.Specs.ProductSpecification, Alpha.Announcements, Version=1.0.0.1, Culture=neutral, PublicKeyToken=null',
                              'ConjunctionOperator':0,
                              'SpecificationOperator':1,
                              'ValueRange':null
                           },
                           {  
                              'Value':[  
                                 'MBC',
                                 ' 123',
                                 ' fdak'
                              ],
                              'Id':'6edc73e0-aa87-4757-96e7-e5ffea0ce06a',
                              'Name':'Sites',
                              'Combinable':true,
                              'TypeName':'Alpha.Announcements.Specs.SiteSpecification, Alpha.Announcements, Version=1.0.0.1, Culture=neutral, PublicKeyToken=null',
                              'ConjunctionOperator':2,
                              'SpecificationOperator':1,
                              'ValueRange':null
                           }
                        ]";

            var contents = @"W3siTGFuZ3VhZ2VLZXkiOiJlbi1VUyIsIlRleHQiOiIgPHA+RGVhciB2YWx1ZWQgYWdlbnRzLDxicj48L3A+ICAgICAgICAgIDxwPiAgICAgICAgV2Ugbm90aWNlZCB0aGF0IHNvbWUgb2YgeW91ciBtZW1iZXJzIGhhdmUgbm90IGJlZW4gY29uZmlndXJlZCBLZW5vIHNldHRpbmdzIHlldC4gUGxlYXNlLCBraW5kbHkgcmV2aWV3IHRoZSBzZXR0aW5nIHBhZ2UgYWdhaW4uICAgICAgPC9wPiAgICAgICAgICAgIDxwPiAgICAgICAgVGhhbmsgeW91ICAgICAgPC9wPiIsIklzRGlzcGxheWluZ0J5VXJsIjpmYWxzZSwiVXJsIjoiaHR0cHM6Ly9hZTAxLmFsaWNkbi5jb20va2YvSFRCMVlwQnJQRlhYWFhjT1hWWFhxNnhYRlhYWHYvQ2hhdWRlLTIwMTctTm91dmVhdS1TZXh5LUZlbW1lcy1kZS1EZW50ZWxsZS1NJmVhY3V0ZTttb2lyZXMtVHJhbnNwYXJlbnRlcy1DdWxvdHRlcy1TYW5zLUNvdXR1cmUtZm9udC1iLVYtYi5qcGcifSx7Ikxhbmd1YWdlS2V5IjoiemgtQ04iLCJUZXh0IjoiICA8cD7lsIrmlaznmoTnlKjmiLfvvIw8L3A+XG4gICAgICA8cD7moLnmja7mlrDmlL/nrZbvvIzmiYDmnInotoXnuqfmgLvku6PvvIzmgLvku6Plkozku6PnkIbpg73lkK/nlKjkuobku6XkuIvkuqflk4HvvJo8L3A+XG4gICAgICB7cHJvZHVjdC1saXN0fVxuICAgICAgPHA+6L+Z5Lqb5Lqn5ZOB55qE5Lya5ZGY54q25oCB5bCG5L+d5oyB5LiN5Y+Y44CCPC9wPlxuICAgICAgPHA+6LCi6LCiPC9wPiIsIklzRGlzcGxheWluZ0J5VXJsIjpmYWxzZSwiVXJsIjoiIn0seyJMYW5ndWFnZUtleSI6InpoLVRXIiwiVGV4dCI6IiAgPHA+5bCK5pWs55qE55So5oi277yMPC9wPlxuICAgICAgPHA+5qC55pOa5paw5pS/562W77yM5omA5pyJ6LaF57Sa57i95Luj77yM57i95Luj5ZKM5Luj55CG6YO95ZWf55So5LqG5Lul5LiL55Si5ZOB77yaPC9wPlxuICAgICAge3Byb2R1Y3QtbGlzdH1cbiAgICAgIDxwPumAmeS6m+eUouWTgeeahOacg+WToeeLgOaFi+Wwh+S/neaMgeS4jeiuiuOAgjwvcD5cbiAgICAgIDxwPuisneisnTwvcD4iLCJJc0Rpc3BsYXlpbmdCeVVybCI6ZmFsc2UsIlVybCI6IiJ9LHsiTGFuZ3VhZ2VLZXkiOiJqYS1KUCIsIlRleHQiOiIgPHA+RGVhciB2YWx1ZWQgY3VzdG9tZXIsPC9wPlxuICAgICAgPHA+QWNjb3JkaW5nIHRvIHRoZSBuZXcgcG9saWN5LCBhbGwgU3VwZXJzLCBNYXN0ZXJzIGFuZCBBZ2VudHMgaGF2ZSBiZWVuIGVuYWJsZWQgdGhlIGZvbGxvd2luZyBwcm9kdWN0czo8L3A+XG4gICAgICB7cHJvZHVjdC1saXN0fVxuICAgICAgPHA+VGhlc2UgcHJvZHVjdHMnIHN0YXR1cyBvbiBNZW1iZXJzIGFyZSBrZXB0IGludGFjdC48L3A+XG4gICAgICA8cD5UaGFuayB5b3UuPC9wPiIsIklzRGlzcGxheWluZ0J5VXJsIjpmYWxzZSwiVXJsIjoiIn0seyJMYW5ndWFnZUtleSI6ImtvLUtSIiwiVGV4dCI6IiA8cD5EZWFyIHZhbHVlZCBjdXN0b21lciw8L3A+XG4gICAgICA8cD5BY2NvcmRpbmcgdG8gdGhlIG5ldyBwb2xpY3ksIGFsbCBTdXBlcnMsIE1hc3RlcnMgYW5kIEFnZW50cyBoYXZlIGJlZW4gZW5hYmxlZCB0aGUgZm9sbG93aW5nIHByb2R1Y3RzOjwvcD5cbiAgICAgIHtwcm9kdWN0LWxpc3R9XG4gICAgICA8cD5UaGVzZSBwcm9kdWN0cycgc3RhdHVzIG9uIE1lbWJlcnMgYXJlIGtlcHQgaW50YWN0LjwvcD5cbiAgICAgIDxwPlRoYW5rIHlvdS48L3A+IiwiSXNEaXNwbGF5aW5nQnlVcmwiOmZhbHNlLCJVcmwiOiIifSx7Ikxhbmd1YWdlS2V5IjoidGgtVEgiLCJUZXh0IjoiIDxwPuC5gOC4o+C4teC4ouC4meC5gOC4reC5gOC4iOC4meC4leC5jOC4l+C4uOC4geC4l+C5iOC4suC4mTwvcD5cbiAgICAgIDxwPuC4leC4suC4oeC4meC5guC4ouC4muC4suC4ouC5g+C4q+C4oeC5iCDguIvguLjguJvguYDguJvguK3guKPguYwg4Lih4Liy4Liq4LmA4LiV4Lit4Lij4LmMIOC5geC4peC4sCDguYDguK3guYDguIjguJnguJXguYwg4LiX4Lix4LmJ4LiH4Lir4Lih4LiU4LmE4LiU4LmJ4Lij4Lix4Lia4LiB4Liy4Lij4LmA4Lib4Li04LiU4LmD4LiK4LmJ4LiH4Liy4LiZ4Lic4Lil4Li04LiV4Lig4Lix4LiT4LiR4LmM4LiV4LmI4Lit4LmE4Lib4LiZ4Li14LmJOjwvcD5cbiAgICAgIHtwcm9kdWN0LWxpc3R9XG4gICAgICA8cD7guKrguJbguLLguJnguLDguILguK3guIfguJzguKXguLTguJXguKDguLHguJPguJHguYzguYDguKvguKXguYjguLLguJnguLXguYnguYPguJnguKrguKHguLLguIrguLTguIHguIjguLDguJbguLnguIHguYDguIHguYfguJrguYTguKfguYnguYDguKvguKHguLfguK3guJnguYDguJTguLTguKE8L3A+XG4gICAgICA8cD7guILguK3guJrguITguLjguJPguITguYjguLA8L3A+IiwiSXNEaXNwbGF5aW5nQnlVcmwiOmZhbHNlLCJVcmwiOiIifSx7Ikxhbmd1YWdlS2V5IjoidmktVk4iLCJUZXh0IjoiIDxwPkvDrW5oIGfhu61pIHF1w70ga2jDoWNoIGjDoG5nLDwvcD5cbiAgICAgIDxwPkThu7FhIHRoZW8gcXV5IMSR4buLbmggbeG7m2kgY+G7p2EgY8O0bmcgdHksIHThuqV0IGPhuqMgU3VwZXIsIE1hc3RlciB2w6AgQWdlbnQgxJHhu4F1IMSRxrDhu6NjIGvDrWNoIGhv4bqhdCA4IHPhuqNuIHBo4bqpbSBkxrDhu5tpIMSRw6J5OjwvcD5cbiAgICAgIHtwcm9kdWN0LWxpc3R9XG4gICAgICA8cD5UcuG6oW5nIHRow6FpIHPhuqNuIHBo4bqpbSBj4bunYSBuZ8aw4budaSBjaMahaSBz4bq9IGtow7RuZyB0aGF5IMSR4buVaS48L3A+XG4gICAgICA8cD5UcsOibiB0cuG7jW5nLjwvcD4iLCJJc0Rpc3BsYXlpbmdCeVVybCI6ZmFsc2UsIlVybCI6IiJ9XQ==";

            var encodedContents = Convert.FromBase64String(contents);
            var plainText = Encoding.UTF8.GetString(encodedContents);

            var environments = "Local, UAT, Staging";
            var systems = "IBCBet, 789";

            var announcement = new Announcement
            {
                Id = 1,
                Code = "07032017",
                Title = "07032017 - Example",
                IsActive = false,
                AnnouncementType = AnnouncementType.HeaderPopup,
                CreatedDate = new DateTime(2017, 6, 3),
                FromDate = new DateTime(2017, 6, 3),
                ToDate = new DateTime(2017, 7, 20),
                Ordinal = 4,
                Environments = environments.Split(",".ToCharArray(),
                                        StringSplitOptions.RemoveEmptyEntries)
                                        .Select(system => system.Trim()),
                Systems = systems.Split(",".ToCharArray(),
                                        StringSplitOptions.RemoveEmptyEntries)
                                        .Select(system => system.Trim()),
                Contents = JsonConvert.DeserializeObject<IEnumerable<MessageContent>>(plainText),
                Specifications = JsonConvert.DeserializeObject<IEnumerable<ISpecification<IParam>>>(
                           specs,
                            new SpecificationConverter())
            };

            return announcement;
        }
    }
}
