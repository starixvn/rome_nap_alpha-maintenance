namespace Alpha.Maintenance.Repository.Test.Announcements
{
    using Criteria.Announcements;
    using Core.Model;
    using NUnit.Framework;
    using System;

    [TestFixture]
    public class InsertOrUpdateAnnouncementsCriteriaTest
    {
        [Test]
        public void GetSettingKey_ActionIsCreateAlways_ReturnsCorrectSettingKey()
        {
            // Arrange
            var insertOrUpdateAnnouncementsCriteria = new InsertOrUpdateAnnouncementsCriteria();
            insertOrUpdateAnnouncementsCriteria.Action = ActionType.Create;
            var expectedSettingKey = "Age_Announcements_Insert";

            // Act
            var actualSettingKey = insertOrUpdateAnnouncementsCriteria.GetSettingKey();

            // Assert
            Assert.AreEqual(expectedSettingKey, actualSettingKey);
        }

        [Test]
        public void GetSettingKey_ActionIsUpdateAlways_ReturnsCorrectSettingKey()
        {
            // Arrange
            var insertOrUpdateAnnouncementsCriteria = new InsertOrUpdateAnnouncementsCriteria();
            insertOrUpdateAnnouncementsCriteria.Action = ActionType.Update;
            var expectedSettingKey = "Age_Announcements_Update";

            // Act
            var actualSettingKey = insertOrUpdateAnnouncementsCriteria.GetSettingKey();

            // Assert
            Assert.AreEqual(expectedSettingKey, actualSettingKey);
        }

        [Test]
        public void IsValid_InvalidAnnoucementId_ReturnsFalse()
        {
            // Arrange
            var insertOrUpdateAnnouncementsCriteria = new InsertOrUpdateAnnouncementsCriteria();
            insertOrUpdateAnnouncementsCriteria.Id = -1;

            // Act
            var actualIsValid = insertOrUpdateAnnouncementsCriteria.IsValid();

            // Assert
            Assert.IsFalse(actualIsValid);
        }

        [Test]
        public void IsValid_InvalidAnnoucementId_ReturnsTrue()
        {
            // Arrange
            var insertOrUpdateAnnouncementsCriteria = new InsertOrUpdateAnnouncementsCriteria();
            insertOrUpdateAnnouncementsCriteria.Id = 12;

            // Act
            var actualIsValid = insertOrUpdateAnnouncementsCriteria.IsValid();

            // Assert
            Assert.IsTrue(actualIsValid);
        }

        [Test]
        public void IsValid_FromDateGreaterThanToDate_ReturnsFalse()
        {
            // Arrange
            var insertOrUpdateAnnouncementsCriteria = new InsertOrUpdateAnnouncementsCriteria();
            insertOrUpdateAnnouncementsCriteria.Id = 12;
            insertOrUpdateAnnouncementsCriteria.FromDate = new DateTime(2017, 6, 18);
            insertOrUpdateAnnouncementsCriteria.ToDate = new DateTime(2017, 6, 17);

            // Act
            var actualIsValid = insertOrUpdateAnnouncementsCriteria.IsValid();

            // Assert
            Assert.IsFalse(actualIsValid);
        }

        [Test]
        public void IsValid_FromDateLessThanToDate_ReturnsTrue()
        {
            // Arrange
            var insertOrUpdateAnnouncementsCriteria = new InsertOrUpdateAnnouncementsCriteria();
            insertOrUpdateAnnouncementsCriteria.Id = 12;
            insertOrUpdateAnnouncementsCriteria.ToDate = new DateTime(2017, 6, 18);
            insertOrUpdateAnnouncementsCriteria.FromDate = new DateTime(2017, 6, 17);

            // Act
            var actualIsValid = insertOrUpdateAnnouncementsCriteria.IsValid();

            // Assert
            Assert.IsTrue(actualIsValid);
        }
    }
}
