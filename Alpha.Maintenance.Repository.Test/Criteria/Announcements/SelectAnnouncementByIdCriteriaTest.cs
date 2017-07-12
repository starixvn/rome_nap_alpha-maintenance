namespace Alpha.Maintenance.Repository.Test.Announcements
{
    using Criteria.Announcements;
    using NUnit.Framework;

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
        public void IsValid_InvalidAnnoucementId_ReturnsFalse()
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
        public void IsValid_ValidAnnoucementId_ReturnsTrue()
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
