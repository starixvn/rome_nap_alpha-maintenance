namespace Alpha.Maintenance.Controller.Tests
{
    using Controller;
    using NSubstitute;
    using NUnit.Framework;
    using Repository.Criteria.Announcements;
    using Service.Interface.Announcement;
    using System;

    [TestFixture]
    public class AnnouncementApiControllerTest
    {
        private AnnouncementService announcementService;

        private AnnouncementApiController announcementApiController;

        [SetUp]
        public void SetUp()
        {
            announcementService = Substitute.For<AnnouncementService>();
            announcementApiController = new AnnouncementApiController(announcementService);
        }

        [Test]
        public void ManageAnnouncements_CallGetAnnouncementsService()
        {
            // Arrange
            var request = new AnnouncementHistoryRequest
            {
                Environment = "Local",
                FromDate = DateTime.Now.AddDays(-1),
                System = "Nova88",
                ToDate = DateTime.Now
            };

            // Act
            var act = announcementApiController.GetAnnouncementsHistory(request);
                     
            // Expected   
            var expected = announcementService.Received().GetAnnouncements(Arg.Any<SelectAnnouncementsCriteria>());

            // Assert
            Assert.AreEqual(expected, act);
        }
    }
}
