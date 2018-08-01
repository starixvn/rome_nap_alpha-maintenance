namespace Alpha.Maintenance.Repository.Tests.Announcements
{
    using System;
    using NUnit.Framework;
    using Repository.Criteria.Announcements;

    [TestFixture]
    public class InsertAnnouncementsCriteriaTest
    {
        public void GetSettingKey_ActionIsValid_ReturnsCorrectSettingKey()
        {
            // Arrange
            var insertAnnouncementsCriteria = new InsertAnnouncementsCriteria();
            var expectedSettingKey = "Age_Announcements_Insert";

            // Act
            var actualSettingKey = insertAnnouncementsCriteria.GetSettingKey();

            // Assert
            Assert.AreEqual(expectedSettingKey, actualSettingKey);
        }

        [Test]
        public void IsValid_Always_ReturnsTrue()
        {
            // Arrange
            var insertAnnouncementsCriteria = new InsertAnnouncementsCriteria();

            // Act
            var actualIsValid = insertAnnouncementsCriteria.IsValid();

            // Assert
            Assert.IsTrue(actualIsValid);
        }

        [Test]
        public void IsValid_FromDateGreaterThanToDate_ReturnsFalse()
        {
            // Arrange
            var insertAnnouncementsCriteria = new InsertAnnouncementsCriteria();
            insertAnnouncementsCriteria.ReleasedDate = new DateTime(2017, 6, 18);
            

            // Act
            var actualIsValid = insertAnnouncementsCriteria.IsValid();

            // Assert
            Assert.IsTrue(actualIsValid);
        }

        [Test]
        public void IsValid_FromDateLessThanToDate_ReturnsTrue()
        {
            // Arrange
            var insertAnnouncementsCriteria = new InsertAnnouncementsCriteria();
            insertAnnouncementsCriteria.ReleasedDate = new DateTime(2017, 6, 17);

            // Act
            var actualIsValid = insertAnnouncementsCriteria.IsValid();

            // Assert
            Assert.IsTrue(actualIsValid);
        }
    }
}
