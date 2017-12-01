namespace Alpha.Maintenance.Repository.Tests.Criteria.IPInformation
{
    using NUnit.Framework;
    using Repository.Criteria.IPInformation;

    [TestFixture]
    public class GetIPInformationCriteriaTests
    {
        [Test]
        public void GetSettingKey_Always_ReturnsCorrectSettingKey()
        {
            // Arrange
            var criteria = new GetIPInformationCriteria(string.Empty);

            // Act
            var settingKey = criteria.GetSettingKey();

            // Assert
            Assert.AreEqual("Acc_GetIPInfo_Full", settingKey);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void IsValid_IPIsNullOrWhiteSpace_ReturnsFalse(string ip)
        {
            // Arrange
            var criteria = new GetIPInformationCriteria(ip);

            // Act
            var isValid = criteria.IsValid();

            // Assert
            Assert.IsFalse(isValid);
        }

        [Test]
        public void IsValid_ValidCriteria_ReturnsTrue()
        {
            // Arrange
            var criteria = new GetIPInformationCriteria("127.0.0.1");

            // Act
            var isValid = criteria.IsValid();

            // Assert
            Assert.IsTrue(isValid);
        }
    }
}
