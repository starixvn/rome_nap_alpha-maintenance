namespace Alpha.Maintenance.Controller.Test
{
    using System.Collections.Generic;
    using Announcements.Models;
    using Announcements.Specs;
    using Controller;
    using Fanex.NAP.Core.Service.Interface;
    using Fanex.NAP.Core.Web.Context;
    using Fanex.NAP.Core.Web.Session;
    using NSubstitute;
    using NUnit.Framework;
    using Repository.Criteria.Announcements;
    using Request;
    using Service.Interface.Announcement;

    [TestFixture]
    public class AnnouncementControllerTest
    {
        private AnnouncementService announcementService;

        private ICurrentContext currentContext;
        private ICurrentSession currentSession;
        private IUserService userService;
        private AnnouncementController announcementController;
        private string announcementJson;

        [SetUp]
        public void SetUp()
        {
            currentContext = Substitute.For<ICurrentContext>();
            currentSession = Substitute.For<ICurrentSession>();
            userService = Substitute.For<IUserService>();
            announcementService = Substitute.For<AnnouncementService>();
            announcementJson = @"{  
                                   'id':'32',
                                   'code':'17072017-david',
                                   'title':'17072017-david',
                                   'isActive':true,
                                   'announcementType':'1',
                                   'contents':[  
                                      {  
                                         'LanguageKey':'en-US',
                                         'Text':'',
                                         'Url':''
                                      },
                                      {  
                                         'LanguageKey':'vi-VN',
                                         'Text':'',
                                         'Url':''
                                      },
                                      {  
                                         'LanguageKey':'th-TH',
                                         'Text':'',
                                         'Url':''
                                      },
                                      {  
                                         'LanguageKey':'zh-CN',
                                         'Text':'',
                                         'Url':''
                                      },
                                      {  
                                         'LanguageKey':'zh-TW',
                                         'Text':'',
                                         'Url':''
                                      },
                                      {  
                                         'LanguageKey':'ko-KR',
                                         'Text':'',
                                         'Url':''
                                      },
                                      {  
                                         'LanguageKey':'ja-JP',
                                         'Text':'',
                                         'Url':''
                                      }
                                   ],
                                   'environments':[  
                                      'Local'
                                   ],
                                   'fromdate':'Jul 17,2017',
                                   'todate':'Jul 17,2017',
                                   'order':'1',
                                   'systems':[  
                                      'IBCBet'
                                   ],
                                   'specifications':[  

                                   ]
                                }";

            announcementController = new AnnouncementController(
                announcementService,
                currentContext,
                currentSession,
                userService,
                true,
                7,
                true,
                7);
        }

        [Test]
        public void ManageAnnouncements_CallGetAnnouncementsService()
        {
            // Arrange
            var selectAnnouncementsCriteria = new SelectAnnouncementsCriteria();

            // Act
            announcementController.ManageAnnouncements(selectAnnouncementsCriteria);

            // Assert
            announcementService.Received().GetAnnouncements(Arg.Any<SelectAnnouncementsCriteria>());
        }

        [Test]
        public void ComposeMessage_ActionIsCreate_CallGetAnnouncementByIdService()
        {
            // Arrange
            var selectAnnouncementByIdCriteria = new SelectAnnouncementByIdCriteria
            {
                Id = 1
            };

            var specs = new List<ISpecification<IParam>>();
            announcementService.GetOfferedSpecifications().Returns(specs);

            var announcement = new Announcement();
            announcementService.GetAnnouncementById(Arg.Any<SelectAnnouncementByIdCriteria>()).Returns(announcement);

            // Act
            announcementController.ComposeMessage(selectAnnouncementByIdCriteria);

            // Assert
            announcementService.Received().GetAnnouncementById(Arg.Any<SelectAnnouncementByIdCriteria>());
        }

        [Test]
        public void InsertAnnouncement_Always_CallInsertAnnouncementService()
        {
            // Arrange

            var saveAnnouncementDemand = new SaveAnnouncementDemand
            {
                Announcement = announcementJson
            };

            // Act
            announcementController.InsertAnnouncement(saveAnnouncementDemand);

            // Assert
            announcementService.Received().InsertAnnouncement(Arg.Any<InsertAnnouncementsCriteria>());
        }

        [Test]
        public void UpdateAnnouncement_Always_CallUpdateAnnouncementService()
        {
            // Arrange

            var saveAnnouncementDemand = new SaveAnnouncementDemand
            {
                Announcement = announcementJson
            };

            // Act
            announcementController.UpdateAnnouncement(saveAnnouncementDemand);

            // Assert
            announcementService.Received().UpdateAnnouncement(Arg.Any<UpdateAnnouncementsCriteria>());
        }

        [Test]
        public void ComposeMessage_CallGetOfferedSpecificationsService()
        {
            // Arrange
            var selectAnnouncementByIdCriteria = new SelectAnnouncementByIdCriteria
            {
                Id = 0
            };

            var announcement = new Announcement();
            announcementService.GetDefaultAnnouncement().Returns(announcement);

            // Act
            announcementController.ComposeMessage(selectAnnouncementByIdCriteria);

            // Assert
            announcementService.Received().GetOfferedSpecifications();
        }

        [Test]
        public void ComposeMessage_ActionIsCreate_CallGetDefaultAnnouncementService()
        {
            // Arrange
            var selectAnnouncementByIdCriteria = new SelectAnnouncementByIdCriteria
            {
                Id = 0
            };

            var announcement = new Announcement();
            announcementService.GetDefaultAnnouncement().Returns(announcement);

            // Act
            announcementController.ComposeMessage(selectAnnouncementByIdCriteria);

            // Assert
            announcementService.Received().GetDefaultAnnouncement();
        }
    }
}
