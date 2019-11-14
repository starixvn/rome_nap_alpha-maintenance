namespace Alpha.Maintenance.Repository.Tests.Announcements
{
    using System;
    using NUnit.Framework;
    using Repository.Criteria.Announcements;

    [TestFixture]
    public class SelectAnnouncementsCriteriaTest
    {
        [TestCase(FilterBy.ExpiredDate, "Age_Announcements_Select")]
        public void GetSettingKey_ManageTypeIsValid_ReturnsCorrectSettingKey(
            FilterBy manageType,
            string expectedSettingKey)
        {
            // Arrange
            var selectAnnouncementsCriteriaTest = new SelectAnnouncementsCriteria();
            selectAnnouncementsCriteriaTest.FilterBy = manageType;

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
