namespace Alpha.Maintenance.Repository.Tests.Announcements
{
    using System;
    using NUnit.Framework;
    using Repository.Criteria.Announcements;

    [TestFixture]
    public class UpdateAnnouncementsCriteriaTest
    {
        public void GetSettingKey_ActionIsValid_ReturnsCorrectSettingKey()
        {
            // Arrange
            var updateAnnouncementsCriteria = new UpdateAnnouncementsCriteria();
            var expectedSettingKey = "Age_Announcements_Update";

            // Act
            var actualSettingKey = updateAnnouncementsCriteria.GetSettingKey();

            // Assert
            Assert.AreEqual(expectedSettingKey, actualSettingKey);
        }

        [Test]
        public void IsValid_AnnoucementIdLessThanZero_ReturnsFalse()
        {
            // Arrange
            var updateAnnouncementsCriteria = new UpdateAnnouncementsCriteria();
            updateAnnouncementsCriteria.Id = -1;

            // Act
            var actualIsValid = updateAnnouncementsCriteria.IsValid();

            // Assert
            Assert.IsFalse(actualIsValid);
        }

        [Test]
        public void IsValid_AnnoucementIdGreaterThanOrEqualZero_ReturnsTrue()
        {
            // Arrange
            var updateAnnouncementsCriteria = new UpdateAnnouncementsCriteria();
            updateAnnouncementsCriteria.Id = 12;

            // Act
            var actualIsValid = updateAnnouncementsCriteria.IsValid();

            // Assert
            Assert.IsTrue(actualIsValid);
        }

        [Test]
        public void IsValid_FromDateGreaterThanToDate_ReturnsFalse()
        {
            // Arrange
            var updateAnnouncementsCriteria = new UpdateAnnouncementsCriteria();
            updateAnnouncementsCriteria.Id = 12;
            updateAnnouncementsCriteria.FromDate = new DateTime(2017, 6, 18);
            updateAnnouncementsCriteria.ToDate = new DateTime(2017, 6, 17);

            // Act
            var actualIsValid = updateAnnouncementsCriteria.IsValid();

            // Assert
            Assert.IsFalse(actualIsValid);
        }

        [Test]
        public void IsValid_FromDateLessThanToDate_ReturnsTrue()
        {
            // Arrange
            var updateAnnouncementsCriteria = new UpdateAnnouncementsCriteria();
            updateAnnouncementsCriteria.Id = 12;
            updateAnnouncementsCriteria.ToDate = new DateTime(2017, 6, 18);
            updateAnnouncementsCriteria.FromDate = new DateTime(2017, 6, 17);

            // Act
            var actualIsValid = updateAnnouncementsCriteria.IsValid();

            // Assert
            Assert.IsTrue(actualIsValid);
        }
    }
}
