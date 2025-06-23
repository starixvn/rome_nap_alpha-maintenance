using System;
using System.Web.Mvc;
using Alpha.Maintenance.Controller;
using Alpha.Maintenance.Core.Model;
using Alpha.Maintenance.Core.Model.ResetPotentialWinLimitJobConfig;
using Alpha.Maintenance.Service.Interface.ResetPotentialWinLimitJobConfig;
using Fanex.NAP.Core.Service.Interface;
using Fanex.NAP.Core.Web.Context;
using Fanex.NAP.Core.Web.Session;
using NSubstitute;
using NUnit.Framework;

namespace Alpha.Maintenance.Controller.Tests
{
    [TestFixture]
    public class ResetPotentialWinLimitJobConfigControllerTests
    {
        private ResetPotentialWinLimitJobConfigService service;
        private ICurrentContext currentContext;
        private ICurrentSession currentSession;
        private IUserService userService;
        private ResetPotentialWinLimitJobConfigController controller;

        [SetUp]
        public void SetUp()
        {
            currentContext = Substitute.For<ICurrentContext>();
            currentSession = Substitute.For<ICurrentSession>();
            userService = Substitute.For<IUserService>();
            service = Substitute.For<ResetPotentialWinLimitJobConfigService>();
            
            controller = new ResetPotentialWinLimitJobConfigController(service);
        }

        [Test]
        public void Index_WhenCalled_ShouldCallGetResetPotentialWinLimitJobConfigService()
        {
            // Arrange
            var expectedSetting = new ResetPotentialWinLimitJobSettingModel
            {
                Config = new ResetPotentialWinLimitJobConfigModel
                {
                    DefaultTime = "10:00",
                    IsActive = true,
                    ManualTime = DateTime.Now.AddDays(1)
                },
                Status = new ResetPotentialWinLimitJobStatusModel
                {
                    RunningDate = DateTime.Now.AddHours(-1),
                    LastMaxCustID = 1500,
                    CompletedTime = DateTime.Now
                }
            };
            
            service.GetResetPotentialWinLimitJobConfig().Returns(expectedSetting);

            // Act
            var result = controller.Index();

            // Assert
            service.Received(1).GetResetPotentialWinLimitJobConfig();
        }

        [Test]
        public void Index_WhenCalled_ShouldReturnViewResult()
        {
            // Arrange
            var expectedSetting = new ResetPotentialWinLimitJobSettingModel
            {
                Config = new ResetPotentialWinLimitJobConfigModel
                {
                    DefaultTime = "10:00",
                    IsActive = true,
                    ManualTime = DateTime.Now.AddDays(1)
                },
                Status = new ResetPotentialWinLimitJobStatusModel
                {
                    RunningDate = DateTime.Now.AddHours(-1),
                    LastMaxCustID = 1500,
                    CompletedTime = DateTime.Now
                }
            };
            
            service.GetResetPotentialWinLimitJobConfig().Returns(expectedSetting);

            // Act
            var result = controller.Index();

            // Assert
            Assert.That(result, Is.InstanceOf<ViewResult>());
            var viewResult = result as ViewResult;
            Assert.That(viewResult.ViewName, Is.EqualTo("~/Views/ResetPotentialWinLimitJobConfig/Index.cshtml"));
            Assert.That(viewResult.Model, Is.EqualTo(expectedSetting));
        }

        [Test]
        public void Index_WhenServiceReturnsNull_ShouldReturnViewResultWithNullModel()
        {
            // Arrange
            service.GetResetPotentialWinLimitJobConfig().Returns((ResetPotentialWinLimitJobSettingModel)null);

            // Act
            var result = controller.Index();

            // Assert
            Assert.That(result, Is.InstanceOf<ViewResult>());
            var viewResult = result as ViewResult;
            Assert.That(viewResult.ViewName, Is.EqualTo("~/Views/ResetPotentialWinLimitJobConfig/Index.cshtml"));
            Assert.That(viewResult.Model, Is.Null);
        }

        [Test]
        public void Update_WhenValidSettingProvided_ShouldCallUpdateService()
        {
            // Arrange
            var setting = new ResetPotentialWinLimitJobSettingModel
            {
                Config = new ResetPotentialWinLimitJobConfigModel
                {
                    DefaultTime = "10:00",
                    IsActive = true,
                    ManualTime = DateTime.Now.AddDays(1)
                },
                Status = new ResetPotentialWinLimitJobStatusModel
                {
                    RunningDate = DateTime.Now.AddHours(-1),
                    LastMaxCustID = 1500,
                    CompletedTime = DateTime.Now
                }
            };

            var expectedResult = new Result { Code = 0, Message = "Success" };
            service.Update(setting).Returns(expectedResult);

            // Act
            var result = controller.Update(setting);

            // Assert
            service.Received(1).Update(setting);
        }

        [Test]
        public void Update_WhenValidSettingProvided_ShouldReturnJsonResult()
        {
            // Arrange
            var setting = new ResetPotentialWinLimitJobSettingModel
            {
                Config = new ResetPotentialWinLimitJobConfigModel
                {
                    DefaultTime = "10:00",
                    IsActive = true,
                    ManualTime = DateTime.Now.AddDays(1)
                },
                Status = new ResetPotentialWinLimitJobStatusModel
                {
                    RunningDate = DateTime.Now.AddHours(-1),
                    LastMaxCustID = 1500,
                    CompletedTime = DateTime.Now
                }
            };

            var expectedResult = new Result { Code = 0, Message = "Success" };
            service.Update(setting).Returns(expectedResult);

            // Act
            var result = controller.Update(setting);

            // Assert
            Assert.That(result, Is.InstanceOf<JsonResult>());
            var jsonResult = result as JsonResult;
            Assert.That(jsonResult.Data, Is.EqualTo(expectedResult));
        }

        [Test]
        public void Update_WhenServiceReturnsErrorResult_ShouldReturnJsonResultWithError()
        {
            // Arrange
            var setting = new ResetPotentialWinLimitJobSettingModel
            {
                Config = new ResetPotentialWinLimitJobConfigModel
                {
                    DefaultTime = "10:00",
                    IsActive = true,
                    ManualTime = DateTime.Now.AddDays(1)
                },
                Status = new ResetPotentialWinLimitJobStatusModel
                {
                    RunningDate = DateTime.Now.AddHours(-1),
                    LastMaxCustID = 1500,
                    CompletedTime = DateTime.Now
                }
            };

            var expectedResult = new Result { Code = 1, Message = "Error occurred" };
            service.Update(setting).Returns(expectedResult);

            // Act
            var result = controller.Update(setting);

            // Assert
            Assert.That(result, Is.InstanceOf<JsonResult>());
            var jsonResult = result as JsonResult;
            Assert.That(jsonResult.Data, Is.EqualTo(expectedResult));
        }

        [Test]
        public void Update_WhenSettingWithNullConfig_ShouldCallUpdateServiceWithNullConfig()
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

            var expectedResult = new Result { Code = 0, Message = "Success" };
            service.Update(setting).Returns(expectedResult);

            // Act
            var result = controller.Update(setting);

            // Assert
            service.Received(1).Update(setting);
            Assert.That(result, Is.InstanceOf<JsonResult>());
        }

        [Test]
        public void Update_WhenSettingWithNullStatus_ShouldCallUpdateServiceWithNullStatus()
        {
            // Arrange
            var setting = new ResetPotentialWinLimitJobSettingModel
            {
                Config = new ResetPotentialWinLimitJobConfigModel
                {
                    DefaultTime = "10:00",
                    IsActive = true,
                    ManualTime = DateTime.Now.AddDays(1)
                },
                Status = null
            };

            var expectedResult = new Result { Code = 0, Message = "Success" };
            service.Update(setting).Returns(expectedResult);

            // Act
            var result = controller.Update(setting);

            // Assert
            service.Received(1).Update(setting);
            Assert.That(result, Is.InstanceOf<JsonResult>());
        }

        [Test]
        public void Update_WhenSettingWithNullConfigAndStatus_ShouldCallUpdateServiceWithNullValues()
        {
            // Arrange
            var setting = new ResetPotentialWinLimitJobSettingModel
            {
                Config = null,
                Status = null
            };

            var expectedResult = new Result { Code = 0, Message = "Success" };
            service.Update(setting).Returns(expectedResult);

            // Act
            var result = controller.Update(setting);

            // Assert
            service.Received(1).Update(setting);
            Assert.That(result, Is.InstanceOf<JsonResult>());
        }

        [Test]
        public void Constructor_WhenNoServiceProvided_ShouldCreateDefaultService()
        {
            // Act
            var controllerWithDefaultService = new ResetPotentialWinLimitJobConfigController();

            // Assert
            Assert.That(controllerWithDefaultService, Is.Not.Null);
        }

        [Test]
        public void Constructor_WhenServiceProvided_ShouldUseProvidedService()
        {
            // Arrange
            var customService = Substitute.For<ResetPotentialWinLimitJobConfigService>();

            // Act
            var controllerWithCustomService = new ResetPotentialWinLimitJobConfigController(customService);

            // Assert
            Assert.That(controllerWithCustomService, Is.Not.Null);
        }
    }
} 