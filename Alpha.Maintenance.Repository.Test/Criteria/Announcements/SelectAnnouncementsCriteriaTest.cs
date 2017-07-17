namespace Alpha.Maintenance.Repository.Test.Announcements
{
    using System;
    using Criteria.Announcements;
    using NUnit.Framework;

    [TestFixture]
    public class SelectAnnouncementsCriteriaTest
    {
        [Test]
        public void GetSettingKey_ActionIsSelectAlways_ReturnsCorrectSettingKey()
        {
            // Arrange
            var selectAnnouncementsCriteriaTest = new SelectAnnouncementsCriteria();
            var expectedSettingKey = "Age_Announcements_Select";

            // Act
            var actualSettingKey = selectAnnouncementsCriteriaTest.GetSettingKey();

            // Assert
            Assert.AreEqual(expectedSettingKey, actualSettingKey);
        }

        [Test]
        public void IsValid_FromDateGreaterThanToDate_ReturnsFalse()
        {
            // Arrange
            var selectAnnouncementsCriteria = new SelectAnnouncementsCriteria();
            selectAnnouncementsCriteria.FromDate = new DateTime(2017, 6, 18);
            selectAnnouncementsCriteria.ToDate = new DateTime(2017, 6, 17);

            // Act
            var actualIsValid = selectAnnouncementsCriteria.IsValid();

            // Assert
            Assert.IsFalse(actualIsValid);
        }

        [Test]
        public void IsValid_FromDateLessThanToDate_ReturnsTrue()
        {
            // Arrange
            var selectAnnouncementsCriteria = new SelectAnnouncementsCriteria();
            selectAnnouncementsCriteria.ToDate = new DateTime(2017, 6, 18);
            selectAnnouncementsCriteria.FromDate = new DateTime(2017, 6, 17);

            // Act
            var actualIsValid = selectAnnouncementsCriteria.IsValid();

            // Assert
            Assert.IsTrue(actualIsValid);
        }
    }
}
