namespace Alpha.Maintenance.Repository.Test.Announcements
{
    using System;
    using Criteria.Announcements;
    using NUnit.Framework;

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
            insertAnnouncementsCriteria.FromDate = new DateTime(2017, 6, 18);
            insertAnnouncementsCriteria.ToDate = new DateTime(2017, 6, 17);

            // Act
            var actualIsValid = insertAnnouncementsCriteria.IsValid();

            // Assert
            Assert.IsFalse(actualIsValid);
        }

        [Test]
        public void IsValid_FromDateLessThanToDate_ReturnsTrue()
        {
            // Arrange
            var insertAnnouncementsCriteria = new InsertAnnouncementsCriteria();
            insertAnnouncementsCriteria.ToDate = new DateTime(2017, 6, 18);
            insertAnnouncementsCriteria.FromDate = new DateTime(2017, 6, 17);

            // Act
            var actualIsValid = insertAnnouncementsCriteria.IsValid();

            // Assert
            Assert.IsTrue(actualIsValid);
        }
    }
}
