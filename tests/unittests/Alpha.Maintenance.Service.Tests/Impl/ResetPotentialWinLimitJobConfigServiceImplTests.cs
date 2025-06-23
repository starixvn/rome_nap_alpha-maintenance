using System;
using Alpha.Maintenance.Core.Model;
using Alpha.Maintenance.Core.Model.ResetPotentialWinLimitJobConfig;
using Alpha.Maintenance.Repository.Criteria.ResetPotentialWinLimitJob;
using Alpha.Maintenance.Service.Impl.ResetPotentialWinLimitJobConfig;
using Fanex.Data.Repository;
using NSubstitute;
using NUnit.Framework;
using Newtonsoft.Json;

namespace Alpha.Maintenance.Service.Tests.Impl
{
    [TestFixture]
    public class ResetPotentialWinLimitJobConfigServiceImplTests
    {
        private ResetPotentialWinLimitJobConfigServiceImpl _sut;
        private IDynamicRepository _repository;

        [SetUp]
        public void Setup()
        {
            _repository = Substitute.For<IDynamicRepository>();
            _sut = new ResetPotentialWinLimitJobConfigServiceImpl(_repository);
        }

        [Test]
        public void Constructor_WhenRepositoryIsNull_ShouldCreateNewDynamicRepository()
        {
            // Act
            var service = new ResetPotentialWinLimitJobConfigServiceImpl();

            // Assert
            Assert.That(service, Is.Not.Null);
        }

        [Test]
        public void Constructor_WhenRepositoryIsProvided_ShouldUseProvidedRepository()
        {
            // Act
            var service = new ResetPotentialWinLimitJobConfigServiceImpl(_repository);

            // Assert
            Assert.That(service, Is.Not.Null);
        }

        [Test]
        public void GetResetPotentialWinLimitJobConfig_WhenValidData_ShouldReturnDeserializedSettingModel()
        {
            // Arrange
            var configModel = new ResetPotentialWinLimitJobConfigModel
            {
                DefaultTime = "14:30",
                IsActive = true,
                ManualTime = DateTime.Now.AddDays(1)
            };

            var statusModel = new ResetPotentialWinLimitJobStatusModel
            {
                RunningDate = DateTime.Now.AddHours(-1),
                LastMaxCustID = 1000,
                CompletedTime = DateTime.Now
            };

            var configJson = JsonConvert.SerializeObject(configModel);
            var statusJson = JsonConvert.SerializeObject(statusModel);

            var configCriteria = new GetResetPotentialWinLimitJobSettingCriteria("ResetPotentialWinLimitJob_Config");
            var statusCriteria = new GetResetPotentialWinLimitJobSettingCriteria("ResetPotentialWinLimitJob_Status");

            _repository.When(x => x.Execute(configCriteria)).Do(x => configCriteria.VValue = configJson);
            _repository.When(x => x.Execute(statusCriteria)).Do(x => statusCriteria.VValue = statusJson);

            // Act
            var result = _sut.GetResetPotentialWinLimitJobConfig();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Config, Is.Not.Null);
            Assert.That(result.Status, Is.Not.Null);
            Assert.That(result.Config.DefaultTime, Is.EqualTo("14:30"));
            Assert.That(result.Config.IsActive, Is.True);
            Assert.That(result.Config.ManualTime, Is.EqualTo(configModel.ManualTime));
            Assert.That(result.Status.RunningDate, Is.EqualTo(statusModel.RunningDate));
            Assert.That(result.Status.LastMaxCustID, Is.EqualTo(1000));
            Assert.That(result.Status.CompletedTime, Is.EqualTo(statusModel.CompletedTime));

            _repository.Received(1).Execute(Arg.Is<GetResetPotentialWinLimitJobSettingCriteria>(c => c.VName == "ResetPotentialWinLimitJob_Config"));
            _repository.Received(1).Execute(Arg.Is<GetResetPotentialWinLimitJobSettingCriteria>(c => c.VName == "ResetPotentialWinLimitJob_Status"));
        }

        [Test]
        public void GetResetPotentialWinLimitJobConfig_WhenNullJsonData_ShouldReturnNullModels()
        {
            // Arrange
            var configCriteria = new GetResetPotentialWinLimitJobSettingCriteria("ResetPotentialWinLimitJob_Config");
            var statusCriteria = new GetResetPotentialWinLimitJobSettingCriteria("ResetPotentialWinLimitJob_Status");

            _repository.When(x => x.Execute(configCriteria)).Do(x => configCriteria.VValue = null);
            _repository.When(x => x.Execute(statusCriteria)).Do(x => statusCriteria.VValue = null);

            // Act
            var result = _sut.GetResetPotentialWinLimitJobConfig();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Config, Is.Null);
            Assert.That(result.Status, Is.Null);

            _repository.Received(1).Execute(Arg.Is<GetResetPotentialWinLimitJobSettingCriteria>(c => c.VName == "ResetPotentialWinLimitJob_Config"));
            _repository.Received(1).Execute(Arg.Is<GetResetPotentialWinLimitJobSettingCriteria>(c => c.VName == "ResetPotentialWinLimitJob_Status"));
        }

        [Test]
        public void GetResetPotentialWinLimitJobConfig_WhenEmptyJsonData_ShouldReturnNullModels()
        {
            // Arrange
            var configCriteria = new GetResetPotentialWinLimitJobSettingCriteria("ResetPotentialWinLimitJob_Config");
            var statusCriteria = new GetResetPotentialWinLimitJobSettingCriteria("ResetPotentialWinLimitJob_Status");

            _repository.When(x => x.Execute(configCriteria)).Do(x => configCriteria.VValue = "");
            _repository.When(x => x.Execute(statusCriteria)).Do(x => statusCriteria.VValue = "");

            // Act
            var result = _sut.GetResetPotentialWinLimitJobConfig();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Config, Is.Null);
            Assert.That(result.Status, Is.Null);

            _repository.Received(1).Execute(Arg.Is<GetResetPotentialWinLimitJobSettingCriteria>(c => c.VName == "ResetPotentialWinLimitJob_Config"));
            _repository.Received(1).Execute(Arg.Is<GetResetPotentialWinLimitJobSettingCriteria>(c => c.VName == "ResetPotentialWinLimitJob_Status"));
        }

        [Test]
        public void GetResetPotentialWinLimitJobConfig_WhenInvalidJsonData_ShouldThrowJsonException()
        {
            // Arrange
            var configCriteria = new GetResetPotentialWinLimitJobSettingCriteria("ResetPotentialWinLimitJob_Config");
            var statusCriteria = new GetResetPotentialWinLimitJobSettingCriteria("ResetPotentialWinLimitJob_Status");

            _repository.When(x => x.Execute(configCriteria)).Do(x => configCriteria.VValue = "invalid json");
            _repository.When(x => x.Execute(statusCriteria)).Do(x => statusCriteria.VValue = "invalid json");

            // Act & Assert
            Assert.Throws<JsonReaderException>(() => _sut.GetResetPotentialWinLimitJobConfig());

            _repository.Received(1).Execute(Arg.Is<GetResetPotentialWinLimitJobSettingCriteria>(c => c.VName == "ResetPotentialWinLimitJob_Config"));
        }

        [Test]
        public void Update_WhenValidSetting_ShouldReturnSuccessResult()
        {
            // Arrange
            var setting = new ResetPotentialWinLimitJobSettingModel
            {
                Config = new ResetPotentialWinLimitJobConfigModel
                {
                    DefaultTime = "15:45",
                    IsActive = false,
                    ManualTime = DateTime.Now.AddDays(2)
                },
                Status = new ResetPotentialWinLimitJobStatusModel
                {
                    RunningDate = DateTime.Now.AddHours(-2),
                    LastMaxCustID = 2000,
                    CompletedTime = DateTime.Now.AddMinutes(-30)
                }
            };

            // Act
            var result = _sut.Update(setting);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Code, Is.EqualTo(0));
            Assert.That(result.Message, Is.EqualTo("Success"));

            _repository.Received(1).Execute(Arg.Is<UpdateResetPotentialWinLimitJobConfigCriteria>(c => 
                c.VName == "ResetPotentialWinLimitJob_Config" && 
                c.VValue.Contains("\"DefaultTime\": \"15:45\"") &&
                c.VValue.Contains("\"IsActive\": false")));

            _repository.Received(1).Execute(Arg.Is<UpdateResetPotentialWinLimitJobConfigCriteria>(c => 
                c.VName == "ResetPotentialWinLimitJob_Status" && 
                c.VValue.Contains("\"LastMaxCustID\": 2000")));
        }

        [Test]
        public void Update_WhenSettingWithNullConfig_ShouldReturnSuccessResult()
        {
            // Arrange
            var setting = new ResetPotentialWinLimitJobSettingModel
            {
                Config = null,
                Status = new ResetPotentialWinLimitJobStatusModel
                {
                    RunningDate = DateTime.Now.AddHours(-1),
                    LastMaxCustID = 1500,
                    CompletedTime = DateTime.Now
                }
            };

            // Act
            var result = _sut.Update(setting);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Code, Is.EqualTo(0));
            Assert.That(result.Message, Is.EqualTo("Success"));

            _repository.Received(1).Execute(Arg.Is<UpdateResetPotentialWinLimitJobConfigCriteria>(c => 
                c.VName == "ResetPotentialWinLimitJob_Config" && 
                c.VValue == "null"));

            _repository.Received(1).Execute(Arg.Is<UpdateResetPotentialWinLimitJobConfigCriteria>(c => 
                c.VName == "ResetPotentialWinLimitJob_Status"));
        }

        [Test]
        public void Update_WhenSettingWithNullStatus_ShouldReturnSuccessResult()
        {
            // Arrange
            var setting = new ResetPotentialWinLimitJobSettingModel
            {
                Config = new ResetPotentialWinLimitJobConfigModel
                {
                    DefaultTime = "10:00",
                    IsActive = true,
                    ManualTime = null
                },
                Status = null
            };

            // Act
            var result = _sut.Update(setting);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Code, Is.EqualTo(0));
            Assert.That(result.Message, Is.EqualTo("Success"));

            _repository.Received(1).Execute(Arg.Is<UpdateResetPotentialWinLimitJobConfigCriteria>(c => 
                c.VName == "ResetPotentialWinLimitJob_Config"));

            _repository.Received(1).Execute(Arg.Is<UpdateResetPotentialWinLimitJobConfigCriteria>(c => 
                c.VName == "ResetPotentialWinLimitJob_Status" && 
                c.VValue == "null"));
        }

        [Test]
        public void Update_WhenSettingWithNullConfigAndStatus_ShouldReturnSuccessResult()
        {
            // Arrange
            var setting = new ResetPotentialWinLimitJobSettingModel
            {
                Config = null,
                Status = null
            };

            // Act
            var result = _sut.Update(setting);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Code, Is.EqualTo(0));
            Assert.That(result.Message, Is.EqualTo("Success"));

            _repository.Received(1).Execute(Arg.Is<UpdateResetPotentialWinLimitJobConfigCriteria>(c => 
                c.VName == "ResetPotentialWinLimitJob_Config" && 
                c.VValue == "null"));

            _repository.Received(1).Execute(Arg.Is<UpdateResetPotentialWinLimitJobConfigCriteria>(c => 
                c.VName == "ResetPotentialWinLimitJob_Status" && 
                c.VValue == "null"));
        }

        [Test]
        public void Update_WhenRepositoryThrowsException_ShouldPropagateException()
        {
            // Arrange
            var setting = new ResetPotentialWinLimitJobSettingModel
            {
                Config = new ResetPotentialWinLimitJobConfigModel
                {
                    DefaultTime = "12:00",
                    IsActive = true,
                    ManualTime = null
                },
                Status = new ResetPotentialWinLimitJobStatusModel
                {
                    RunningDate = null,
                    LastMaxCustID = 0,
                    CompletedTime = null
                }
            };

            _repository.When(x => x.Execute(Arg.Any<UpdateResetPotentialWinLimitJobConfigCriteria>()))
                .Throw(new InvalidOperationException("Database connection failed"));

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => _sut.Update(setting));

            _repository.Received(1).Execute(Arg.Is<UpdateResetPotentialWinLimitJobConfigCriteria>(c => 
                c.VName == "ResetPotentialWinLimitJob_Config"));
        }

        [Test]
        public void GetResetPotentialWinLimitJobConfig_WhenConfigHasManualTime_ShouldDeserializeCorrectly()
        {
            // Arrange
            var configModel = new ResetPotentialWinLimitJobConfigModel
            {
                DefaultTime = "09:15",
                IsActive = false,
                ManualTime = new DateTime(2024, 1, 15, 14, 30, 0)
            };

            var statusModel = new ResetPotentialWinLimitJobStatusModel
            {
                RunningDate = new DateTime(2024, 1, 14, 10, 0, 0),
                LastMaxCustID = 500,
                CompletedTime = new DateTime(2024, 1, 14, 11, 30, 0)
            };

            var configJson = JsonConvert.SerializeObject(configModel);
            var statusJson = JsonConvert.SerializeObject(statusModel);

            var configCriteria = new GetResetPotentialWinLimitJobSettingCriteria("ResetPotentialWinLimitJob_Config");
            var statusCriteria = new GetResetPotentialWinLimitJobSettingCriteria("ResetPotentialWinLimitJob_Status");

            _repository.When(x => x.Execute(configCriteria)).Do(x => configCriteria.VValue = configJson);
            _repository.When(x => x.Execute(statusCriteria)).Do(x => statusCriteria.VValue = statusJson);

            // Act
            var result = _sut.GetResetPotentialWinLimitJobConfig();

            // Assert
            Assert.That(result.Config.ManualTime, Is.EqualTo(new DateTime(2024, 1, 15, 14, 30, 0)));
            Assert.That(result.Status.RunningDate, Is.EqualTo(new DateTime(2024, 1, 14, 10, 0, 0)));
            Assert.That(result.Status.CompletedTime, Is.EqualTo(new DateTime(2024, 1, 14, 11, 30, 0)));
        }

        [Test]
        public void Update_WhenConfigToStringContainsManualTime_ShouldSerializeCorrectly()
        {
            // Arrange
            var setting = new ResetPotentialWinLimitJobSettingModel
            {
                Config = new ResetPotentialWinLimitJobConfigModel
                {
                    DefaultTime = "16:20",
                    IsActive = true,
                    ManualTime = new DateTime(2024, 2, 20, 18, 45, 0)
                },
                Status = new ResetPotentialWinLimitJobStatusModel
                {
                    RunningDate = new DateTime(2024, 2, 19, 9, 15, 0),
                    LastMaxCustID = 3000,
                    CompletedTime = new DateTime(2024, 2, 19, 17, 30, 0)
                }
            };

            // Act
            var result = _sut.Update(setting);

            // Assert
            Assert.That(result.Code, Is.EqualTo(0));

            _repository.Received(1).Execute(Arg.Is<UpdateResetPotentialWinLimitJobConfigCriteria>(c => 
                c.VName == "ResetPotentialWinLimitJob_Config" && 
                c.VValue.Contains("2024-02-20 18:45:00")));

            _repository.Received(1).Execute(Arg.Is<UpdateResetPotentialWinLimitJobConfigCriteria>(c => 
                c.VName == "ResetPotentialWinLimitJob_Status" && 
                c.VValue.Contains("2024-02-19 09:15:00") &&
                c.VValue.Contains("2024-02-19 17:30:00")));
        }
    }
} 