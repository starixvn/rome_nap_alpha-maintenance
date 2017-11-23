namespace Alpha.Maintenance.Repository.Tests.Announcements
{
    using NUnit.Framework;
    using Repository.Criteria.Announcements;

    [TestFixture]
    public class SelectAnnouncementByIdCriteriaTest
    {

        [Test]
        public void GetSettingKey_Always_ReturnsValidSettingKey()
        {
            // Arrange
            var selectAnnouncementByIdCriteria = new SelectAnnouncementByIdCriteria();
            var expectedSettingKey = "Age_Announcements_SelectById";

            // Act
            var actualSettingKey = selectAnnouncementByIdCriteria.GetSettingKey();

            // Assert
            Assert.AreEqual(expectedSettingKey, actualSettingKey);
        }

        [Test]
        public void IsValid_AnnoucementIdLessThanZero_ReturnsFalse()
        {
            // Arrange
            var selectAnnouncementByIdCriteria = new SelectAnnouncementByIdCriteria();
            selectAnnouncementByIdCriteria.Id = -1;

            // Act
            var actualIsValid = selectAnnouncementByIdCriteria.IsValid();

            // Assert
            Assert.IsFalse(actualIsValid);
        }

        [Test]
        public void IsValid_AnnoucementIdGreaterThanOrEqualZero_ReturnsTrue()
        {
            // Arrange
            var selectAnnouncementByIdCriteria = new SelectAnnouncementByIdCriteria();
            selectAnnouncementByIdCriteria.Id = 2;

            // Act
            var actualIsValid = selectAnnouncementByIdCriteria.IsValid();

            // Assert
            Assert.IsTrue(actualIsValid);
        }
    }
}
