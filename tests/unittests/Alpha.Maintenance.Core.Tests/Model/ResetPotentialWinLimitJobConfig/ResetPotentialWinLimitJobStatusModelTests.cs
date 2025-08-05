using System;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Alpha.Maintenance.Core.Model.ResetPotentialWinLimitJobConfig;
using Alpha.Maintenance.Core.Extension;

namespace Alpha.Maintenance.Core.Tests.Model.ResetPotentialWinLimitJobConfig
{
    [TestFixture]
    public class ResetPotentialWinLimitJobStatusModelTests
    {
        private Fixture _fixture;
        private ResetPotentialWinLimitJobStatusModel _model;

        [SetUp]
        public void Setup()
        {
            _fixture = new Fixture();
            _model = new ResetPotentialWinLimitJobStatusModel();
        }

        [Test]
        public void RunningDate_CanBeSetAndRetrieved()
        {
            // Arrange
            var expectedDate = DateTime.Now;

            // Act
            _model.RunningDate = expectedDate;

            // Assert
            Assert.That(_model.RunningDate, Is.EqualTo(expectedDate));
        }

        [Test]
        public void RunningDate_CanBeSetToNull()
        {
            // Act
            _model.RunningDate = null;

            // Assert
            Assert.That(_model.RunningDate, Is.Null);
        }

        [Test]
        public void LastMaxCustID_CanBeSetAndRetrieved()
        {
            // Arrange
            var expectedId = 12345;

            // Act
            _model.LastMaxCustID = expectedId;

            // Assert
            Assert.That(_model.LastMaxCustID, Is.EqualTo(expectedId));
        }

        [Test]
        public void CompletedTime_CanBeSetAndRetrieved()
        {
            // Arrange
            var expectedDate = DateTime.Now;

            // Act
            _model.CompletedTime = expectedDate;

            // Assert
            Assert.That(_model.CompletedTime, Is.EqualTo(expectedDate));
        }

        [Test]
        public void CompletedTime_CanBeSetToNull()
        {
            // Act
            _model.CompletedTime = null;

            // Assert
            Assert.That(_model.CompletedTime, Is.Null);
        }

        [Test]
        public void DisplayedRunningDate_WhenRunningDateIsNull_ReturnsEmptyString()
        {
            // Arrange
            _model.RunningDate = null;

            // Act
            var result = _model.DisplayedRunningDate;

            // Assert
            Assert.That(result, Is.EqualTo(string.Empty));
        }

        [Test]
        public void DisplayedRunningDate_WhenRunningDateHasValue_ReturnsFormattedDate()
        {
            // Arrange
            var runningDate = new DateTime(2024, 1, 15, 14, 30, 45, 123);
            _model.RunningDate = runningDate;
            var expectedDate = new DateTime(2024, 1, 15, 14, 30, 0).ToLongDate();

            // Act
            var result = _model.DisplayedRunningDate;

            // Assert
            Assert.That(result, Is.EqualTo(expectedDate));
        }

        [Test]
        public void DisplayedRunningDate_RemovesSecondsFromDateTime()
        {
            // Arrange
            var dateWithSeconds = new DateTime(2024, 1, 15, 14, 30, 45, 123);
            _model.RunningDate = dateWithSeconds;
            var expectedDate = new DateTime(2024, 1, 15, 14, 30, 0).ToLongDate();

            // Act
            var result = _model.DisplayedRunningDate;

            // Assert
            Assert.That(result, Is.EqualTo(expectedDate));
        }

        [Test]
        public void DisplayedCompletedTime_WhenCompletedTimeIsNull_ReturnsEmptyString()
        {
            // Arrange
            _model.CompletedTime = null;

            // Act
            var result = _model.DisplayedCompletedTime;

            // Assert
            Assert.That(result, Is.EqualTo(string.Empty));
        }

        [Test]
        public void DisplayedCompletedTime_WhenCompletedTimeHasValue_ReturnsFormattedDate()
        {
            // Arrange
            var completedTime = new DateTime(2024, 1, 15, 16, 45, 30, 456);
            _model.CompletedTime = completedTime;
            var expectedDate = new DateTime(2024, 1, 15, 16, 45, 0).ToLongDate();

            // Act
            var result = _model.DisplayedCompletedTime;

            // Assert
            Assert.That(result, Is.EqualTo(expectedDate));
        }

        [Test]
        public void DisplayedCompletedTime_RemovesSecondsFromDateTime()
        {
            // Arrange
            var dateWithSeconds = new DateTime(2024, 1, 15, 16, 45, 30, 456);
            _model.CompletedTime = dateWithSeconds;
            var expectedDate = new DateTime(2024, 1, 15, 16, 45, 0).ToLongDate();

            // Act
            var result = _model.DisplayedCompletedTime;

            // Assert
            Assert.That(result, Is.EqualTo(expectedDate));
        }

        [Test]
        public void ToString_WhenAllPropertiesAreSet_ReturnsCorrectJsonFormat()
        {
            // Arrange
            var runningDate = new DateTime(2024, 1, 15, 14, 30, 45);
            var completedTime = new DateTime(2024, 1, 15, 16, 45, 30);
            _model.RunningDate = runningDate;
            _model.LastMaxCustID = 12345;
            _model.CompletedTime = completedTime;

            var expected = $"{{\"RunningDate\": \"{runningDate:yyyy-MM-dd HH:mm:ss}\", \"LastMaxCustID\": 12345, \"CompletedTime\": \"{completedTime:yyyy-MM-dd HH:mm:ss}\" }}";

            // Act
            var result = _model.ToString();

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void ToString_WhenRunningDateIsNull_ReturnsCorrectJsonFormat()
        {
            // Arrange
            var completedTime = new DateTime(2024, 1, 15, 16, 45, 30);
            _model.RunningDate = null;
            _model.LastMaxCustID = 12345;
            _model.CompletedTime = completedTime;

            var expected = $"{{\"RunningDate\": null, \"LastMaxCustID\": 12345, \"CompletedTime\": \"{completedTime:yyyy-MM-dd HH:mm:ss}\" }}";

            // Act
            var result = _model.ToString();

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void ToString_WhenCompletedTimeIsNull_ReturnsCorrectJsonFormat()
        {
            // Arrange
            var runningDate = new DateTime(2024, 1, 15, 14, 30, 45);
            _model.RunningDate = runningDate;
            _model.LastMaxCustID = 12345;
            _model.CompletedTime = null;

            var expected = $"{{\"RunningDate\": \"{runningDate:yyyy-MM-dd HH:mm:ss}\", \"LastMaxCustID\": 12345, \"CompletedTime\": null }}";

            // Act
            var result = _model.ToString();

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void ToString_WhenAllPropertiesAreNull_ReturnsCorrectJsonFormat()
        {
            // Arrange
            _model.RunningDate = null;
            _model.LastMaxCustID = 0;
            _model.CompletedTime = null;

            var expected = "{\"RunningDate\": null, \"LastMaxCustID\": 0, \"CompletedTime\": null }";

            // Act
            var result = _model.ToString();

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void DefaultSelectedDate_ReturnsCurrentDateAsLongDate()
        {
            // Arrange
            var expectedDate = DateTime.Now.Date.ToLongDate();

            // Act
            var result = _model.DefaultSelectedDate;

            // Assert
            Assert.That(result, Is.EqualTo(expectedDate));
        }

        [Test]
        public void DisplayedRunningDate_WithDateTimeAtMidnight_ReturnsCorrectFormat()
        {
            // Arrange
            var midnightDate = new DateTime(2024, 1, 15, 0, 0, 0);
            _model.RunningDate = midnightDate;
            var expectedDate = new DateTime(2024, 1, 15, 0, 0, 0).ToLongDate();

            // Act
            var result = _model.DisplayedRunningDate;

            // Assert
            Assert.That(result, Is.EqualTo(expectedDate));
        }

        [Test]
        public void DisplayedRunningDate_WithDateTimeAtEndOfDay_ReturnsCorrectFormat()
        {
            // Arrange
            var endOfDayDate = new DateTime(2024, 1, 15, 23, 59, 59);
            _model.RunningDate = endOfDayDate;
            var expectedDate = new DateTime(2024, 1, 15, 23, 59, 0).ToLongDate();

            // Act
            var result = _model.DisplayedRunningDate;

            // Assert
            Assert.That(result, Is.EqualTo(expectedDate));
        }

        [Test]
        public void DisplayedCompletedTime_WithDateTimeAtMidnight_ReturnsCorrectFormat()
        {
            // Arrange
            var midnightDate = new DateTime(2024, 1, 15, 0, 0, 0);
            _model.CompletedTime = midnightDate;
            var expectedDate = new DateTime(2024, 1, 15, 0, 0, 0).ToLongDate();

            // Act
            var result = _model.DisplayedCompletedTime;

            // Assert
            Assert.That(result, Is.EqualTo(expectedDate));
        }

        [Test]
        public void DisplayedCompletedTime_WithDateTimeAtEndOfDay_ReturnsCorrectFormat()
        {
            // Arrange
            var endOfDayDate = new DateTime(2024, 1, 15, 23, 59, 59);
            _model.CompletedTime = endOfDayDate;
            var expectedDate = new DateTime(2024, 1, 15, 23, 59, 0).ToLongDate();

            // Act
            var result = _model.DisplayedCompletedTime;

            // Assert
            Assert.That(result, Is.EqualTo(expectedDate));
        }

        [Test]
        public void DefaultSelectedDate_ReturnsSameValueOnMultipleCalls()
        {
            // Act
            var result1 = _model.DefaultSelectedDate;
            var result2 = _model.DefaultSelectedDate;

            // Assert
            Assert.That(result1, Is.EqualTo(result2));
        }

        [Test]
        public void DisplayedRunningDate_ReturnsSameValueOnMultipleCalls()
        {
            // Arrange
            var runningDate = new DateTime(2024, 1, 15, 14, 30, 45);
            _model.RunningDate = runningDate;

            // Act
            var result1 = _model.DisplayedRunningDate;
            var result2 = _model.DisplayedRunningDate;

            // Assert
            Assert.That(result1, Is.EqualTo(result2));
        }

        [Test]
        public void DisplayedCompletedTime_ReturnsSameValueOnMultipleCalls()
        {
            // Arrange
            var completedTime = new DateTime(2024, 1, 15, 16, 45, 30);
            _model.CompletedTime = completedTime;

            // Act
            var result1 = _model.DisplayedCompletedTime;
            var result2 = _model.DisplayedCompletedTime;

            // Assert
            Assert.That(result1, Is.EqualTo(result2));
        }

        [Test]
        public void ToString_WithNegativeLastMaxCustID_ReturnsCorrectJsonFormat()
        {
            // Arrange
            var runningDate = new DateTime(2024, 1, 15, 14, 30, 45);
            _model.RunningDate = runningDate;
            _model.LastMaxCustID = -12345;
            _model.CompletedTime = null;

            var expected = $"{{\"RunningDate\": \"{runningDate:yyyy-MM-dd HH:mm:ss}\", \"LastMaxCustID\": -12345, \"CompletedTime\": null }}";

            // Act
            var result = _model.ToString();

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void ToString_WithZeroLastMaxCustID_ReturnsCorrectJsonFormat()
        {
            // Arrange
            var runningDate = new DateTime(2024, 1, 15, 14, 30, 45);
            _model.RunningDate = runningDate;
            _model.LastMaxCustID = 0;
            _model.CompletedTime = null;

            var expected = $"{{\"RunningDate\": \"{runningDate:yyyy-MM-dd HH:mm:ss}\", \"LastMaxCustID\": 0, \"CompletedTime\": null }}";

            // Act
            var result = _model.ToString();

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }
    }
}