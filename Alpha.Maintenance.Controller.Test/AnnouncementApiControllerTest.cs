namespace Alpha.Maintenance.Controller.Test
{
    using Controller;
    using NSubstitute;
    using NUnit.Framework;
    using Repository.Criteria.Announcements;
    using Service.Interface.Announcement;

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
            string environment = "Local";
            string system = "789";

            // Act
            announcementApiController.GetAnnouncements(environment, system);

            // Assert
            announcementService.Received().GetAnnouncements(Arg.Any<SelectAnnouncementsCriteria>());
        }
    }
}
