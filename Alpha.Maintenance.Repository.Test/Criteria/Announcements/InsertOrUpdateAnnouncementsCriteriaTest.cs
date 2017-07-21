namespace Alpha.Maintenance.Repository.Test.Announcements
{
    using System;
    using Criteria.Announcements;
    using NUnit.Framework;

    [TestFixture]
    public class InsertOrUpdateAnnouncementsCriteriaTest
    {
        [TestCase(ActionType.Create, "Age_Announcements_Insert")]
        [TestCase(ActionType.Update, "Age_Announcements_Update")]
        public void GetSettingKey_ActionIsValid_ReturnsCorrectSettingKey(
            ActionType actionType,
            string expectedSettingKey)
        {
            // Arrange
            var insertOrUpdateAnnouncementsCriteria = new InsertOrUpdateAnnouncementsCriteria();
            insertOrUpdateAnnouncementsCriteria.Action = actionType;

            // Act
            var actualSettingKey = insertOrUpdateAnnouncementsCriteria.GetSettingKey();

            // Assert
            Assert.AreEqual(expectedSettingKey, actualSettingKey);
        }

        [Test]
        public void IsValid_AnnoucementIdLessThanZero_ReturnsFalse()
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
        public void IsValid_AnnoucementIdGreaterThanOrEqualZero_ReturnsTrue()
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
