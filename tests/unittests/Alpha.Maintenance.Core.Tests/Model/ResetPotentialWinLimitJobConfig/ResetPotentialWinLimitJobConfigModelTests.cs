using System;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Alpha.Maintenance.Core.Model.ResetPotentialWinLimitJobConfig;
using Alpha.Maintenance.Core.Extension;

namespace Alpha.Maintenance.Core.Tests.Model.ResetPotentialWinLimitJobConfig
{
    [TestFixture]
    public class ResetPotentialWinLimitJobConfigModelTests
    {
        private Fixture _fixture;
        private ResetPotentialWinLimitJobConfigModel _model;

        [SetUp]
        public void Setup()
        {
            _fixture = new Fixture();
            _model = new ResetPotentialWinLimitJobConfigModel();
        }

        [Test]
        public void DefaultTime_CanBeSetAndRetrieved()
        {
            // Arrange
            var expectedTime = "14:30";

            // Act
            _model.DefaultTime = expectedTime;

            // Assert
            Assert.That(_model.DefaultTime, Is.EqualTo(expectedTime));
        }

        [Test]
        public void IsActive_CanBeSetAndRetrieved()
        {
            // Arrange
            var expectedValue = true;

            // Act
            _model.IsActive = expectedValue;

            // Assert
            Assert.That(_model.IsActive, Is.EqualTo(expectedValue));
        }

        [Test]
        public void ManualTime_CanBeSetAndRetrieved()
        {
            // Arrange
            var expectedTime = DateTime.Now.AddDays(1);

            // Act
            _model.ManualTime = expectedTime;

            // Assert
            Assert.That(_model.ManualTime, Is.EqualTo(expectedTime));
        }

        [Test]
        public void ManualTime_CanBeSetToNull()
        {
            // Act
            _model.ManualTime = null;

            // Assert
            Assert.That(_model.ManualTime, Is.Null);
        }

        [Test]
        public void DefaultManualTime_ReturnsCurrentDateAsLongDate()
        {
            // Arrange
            var expectedDate = DateTime.Now.Date.ToLongDate();

            // Act
            var result = _model.DefaultManualTime;

            // Assert
            Assert.That(result, Is.EqualTo(expectedDate));
        }

        [Test]
        public void HasManualTime_WhenManualTimeIsNull_ReturnsFalse()
        {
            // Arrange
            _model.ManualTime = null;

            // Act
            var result = _model.HasManualTime;

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void HasManualTime_WhenManualTimeIsInPast_ReturnsFalse()
        {
            // Arrange
            _model.ManualTime = DateTime.Now.AddDays(-1);

            // Act
            var result = _model.HasManualTime;

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void HasManualTime_WhenManualTimeIsInFuture_ReturnsTrue()
        {
            // Arrange
            _model.ManualTime = DateTime.Now.AddDays(1);

            // Act
            var result = _model.HasManualTime;

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void HasManualTime_WhenManualTimeIsCurrentTime_ReturnsTrue()
        {
            // Arrange
            _model.ManualTime = DateTime.Now;

            // Act
            var result = _model.HasManualTime;

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void DisplayedManualTime_WhenManualTimeIsNull_ReturnsEmptyString()
        {
            // Arrange
            _model.ManualTime = null;

            // Act
            var result = _model.DisplayedManualTime;

            // Assert
            Assert.That(result, Is.EqualTo(string.Empty));
        }

        [Test]
        public void DisplayedManualTime_WhenManualTimeIsInPast_ReturnsEmptyString()
        {
            // Arrange
            _model.ManualTime = DateTime.Now.AddDays(-1);

            // Act
            var result = _model.DisplayedManualTime;

            // Assert
            Assert.That(result, Is.EqualTo(string.Empty));
        }

        [Test]
        public void DisplayedManualTime_WhenManualTimeIsInFuture_ReturnsFormattedDate()
        {
            // Arrange
            var futureDate = DateTime.Now.AddDays(1);
            _model.ManualTime = futureDate;
            var expectedDate = new DateTime(futureDate.Year, futureDate.Month, futureDate.Day,
                                          futureDate.Hour, futureDate.Minute, 0).ToLongDate();

            // Act
            var result = _model.DisplayedManualTime;

            // Assert
            Assert.That(result, Is.EqualTo(expectedDate));
        }

        [Test]
        public void DisplayedManualTime_WhenManualTimeIsCurrentTime_ReturnsFormattedDate()
        {
            // Arrange
            var currentTime = DateTime.Now;
            _model.ManualTime = currentTime;
            var expectedDate = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day,
                                          currentTime.Hour, currentTime.Minute, 0).ToLongDate();

            // Act
            var result = _model.DisplayedManualTime;

            // Assert
            Assert.That(result, Is.EqualTo(expectedDate));
        }

        [Test]
        public void DisplayedManualTime_RemovesSecondsFromDateTime()
        {
            // Arrange
            var dateWithSeconds = new DateTime(2024, 1, 15, 14, 30, 45, 123);
            _model.ManualTime = dateWithSeconds;
            var expectedDate = new DateTime(2024, 1, 15, 14, 30, 0).ToLongDate();

            // Act
            var result = _model.DisplayedManualTime;

            // Assert
            Assert.That(result, Is.EqualTo(expectedDate));
        }

        [Test]
        public void ToString_WhenAllPropertiesAreSet_ReturnsCorrectJsonFormat()
        {
            // Arrange
            _model.DefaultTime = "14:30";
            _model.IsActive = true;
            _model.ManualTime = new DateTime(2024, 1, 15, 14, 30, 0);

            // Act
            var result = _model.ToString();

            // Assert
            var expected = "{ \"DefaultTime\": \"14:30\", \"IsActive\": true, \"ManualTime\": \"2024-01-15 14:30:00\" }";
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void ToString_WhenManualTimeIsNull_ReturnsCorrectJsonFormat()
        {
            // Arrange
            _model.DefaultTime = "14:30";
            _model.IsActive = false;
            _model.ManualTime = null;

            // Act
            var result = _model.ToString();

            // Assert
            var expected = "{ \"DefaultTime\": \"14:30\", \"IsActive\": false, \"ManualTime\": null }";
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void ToString_WhenDefaultTimeIsNull_ReturnsCorrectJsonFormat()
        {
            // Arrange
            _model.DefaultTime = null;
            _model.IsActive = true;
            _model.ManualTime = new DateTime(2024, 1, 15, 14, 30, 0);

            // Act
            var result = _model.ToString();

            // Assert
            var expected = "{ \"DefaultTime\": \"\", \"IsActive\": true, \"ManualTime\": \"2024-01-15 14:30:00\" }";
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void ToString_WhenAllPropertiesAreNull_ReturnsCorrectJsonFormat()
        {
            // Arrange
            _model.DefaultTime = null;
            _model.IsActive = false;
            _model.ManualTime = null;

            // Act
            var result = _model.ToString();

            // Assert
            var expected = "{ \"DefaultTime\": \"\", \"IsActive\": false, \"ManualTime\": null }";
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void HasManualTime_WithDateTimeAtMidnight_ReturnsTrue()
        {
            // Arrange
            var midnight = DateTime.Today;
            _model.ManualTime = midnight;

            // Act
            var result = _model.HasManualTime;

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void HasManualTime_WithDateTimeAtEndOfDay_ReturnsTrue()
        {
            // Arrange
            var endOfDay = DateTime.Today.AddDays(1).AddTicks(-1);
            _model.ManualTime = endOfDay;

            // Act
            var result = _model.HasManualTime;

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void DisplayedManualTime_WithDateTimeAtMidnight_ReturnsCorrectFormat()
        {
            // Arrange
            var midnight = DateTime.Today;
            _model.ManualTime = midnight;
            var expectedDate = new DateTime(midnight.Year, midnight.Month, midnight.Day, 0, 0, 0).ToLongDate();

            // Act
            var result = _model.DisplayedManualTime;

            // Assert
            Assert.That(result, Is.EqualTo(expectedDate));
        }

        [Test]
        public void DisplayedManualTime_WithDateTimeAtEndOfDay_ReturnsCorrectFormat()
        {
            // Arrange
            var endOfDay = DateTime.Today.AddDays(1).AddTicks(-1);
            _model.ManualTime = endOfDay;
            var expectedDate = new DateTime(endOfDay.Year, endOfDay.Month, endOfDay.Day,
                                          endOfDay.Hour, endOfDay.Minute, 0).ToLongDate();

            // Act
            var result = _model.DisplayedManualTime;

            // Assert
            Assert.That(result, Is.EqualTo(expectedDate));
        }

        [Test]
        public void DefaultManualTime_ReturnsSameValueOnMultipleCalls()
        {
            // Act
            var result1 = _model.DefaultManualTime;
            var result2 = _model.DefaultManualTime;

            // Assert
            Assert.That(result1, Is.EqualTo(result2));
        }

        [Test]
        public void DisplayedManualTime_ReturnsSameValueOnMultipleCalls()
        {
            // Arrange
            _model.ManualTime = DateTime.Now.AddDays(1);

            // Act
            var result1 = _model.DisplayedManualTime;
            var result2 = _model.DisplayedManualTime;

            // Assert
            Assert.That(result1, Is.EqualTo(result2));
        }
    }
}