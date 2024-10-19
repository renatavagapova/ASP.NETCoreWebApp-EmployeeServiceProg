using EmployeeServiceProg.Controllers;
using EmployeeServiceProg.Models.Dto;
using EmployeeServiceProg.Models.Requests;
using EmployeeServiceProg.Models;
using EmployeeServiceProg.Services.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.Results;
using Microsoft.Net.Http.Headers;

namespace EmployeeServiceTests
{
    public class AutenticateControllerTests
    {
        private readonly AuthenticateController _authenticateController;
        private readonly Mock<IAuthenticateService> _mockAuthenticateService;
        private readonly Mock<IValidator<AuthenticationRequest>> _mockAuthenticationRequestValidator;
        private readonly Mock<IHttpContextAccessor> _mockHttpContextAccessor;

        public AutenticateControllerTests()
        {
            _mockAuthenticateService = new Mock<IAuthenticateService>();
            _mockAuthenticationRequestValidator = new Mock<IValidator<AuthenticationRequest>>();
            _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();

            // Настройка контроллера с mock-объектами
            _authenticateController = new AuthenticateController(
                _mockAuthenticateService.Object,
                _mockAuthenticationRequestValidator.Object
            );

            // Установка HttpContext для тестов
            _authenticateController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
        }

        [Fact]
        public void Login_ShouldReturnBadRequest_WhenValidationFails()
        {
            // [1] Подготовка mock-объектов
            var authenticationRequest = new AuthenticationRequest { Login = "", Password = "" }; // Некорректные данные
            _mockAuthenticationRequestValidator
                .Setup(validator => validator.Validate(authenticationRequest))
                .Returns(new ValidationResult(new List<ValidationFailure> { new ValidationFailure("Username", "Username is required") }));

            // [2] Вызов метода
            var result = _authenticateController.Login(authenticationRequest);

            // [3] Проверка результата
            var badRequestResult = result as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);
            Assert.IsType<BadRequestObjectResult>(badRequestResult);
        }

        [Fact]
        public void Login_ShouldReturnOk_WhenLoginSucceeds()
        {
            // [1] Подготовка mock-объектов
            var authenticationRequest = new AuthenticationRequest { Login = "user", Password = "password" };
            _mockAuthenticationRequestValidator.Setup(validator => validator.Validate(authenticationRequest)).Returns(new ValidationResult());

            var authenticationResponse = new AuthenticationResponse
            {
                Status = AuthenticationStatus.Success,
                Session = new SessionDto { SessionToken = "valid_token" }
            };
            _mockAuthenticateService.Setup(service => service.Login(authenticationRequest)).Returns(authenticationResponse);

            // [2] Вызов метода
            var result = _authenticateController.Login(authenticationRequest);

            // [3] Проверка результата
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.IsType<OkObjectResult>(okResult);

            // [4] Проверка наличия заголовка X-Session-Token
            Assert.True(_authenticateController.Response.Headers.ContainsKey("X-Session-Token"));
            Assert.Equal("valid_token", _authenticateController.Response.Headers["X-Session-Token"]);
        }

        [Fact]
        public void GetSession_ShouldReturnUnauthorized_WhenAuthorizationHeaderIsMissing()
        {
            // [1] Имитация отсутствия заголовка
            _mockHttpContextAccessor.SetupGet(context => context.HttpContext.Request.Headers[HeaderNames.Authorization]).Returns(string.Empty);
            _authenticateController.ControllerContext.HttpContext = _mockHttpContextAccessor.Object.HttpContext;

            // [2] Вызов метода
            var result = _authenticateController.GetSession();

            // [3] Проверка результата
            var unauthorizedResult = result as UnauthorizedResult;
            Assert.NotNull(unauthorizedResult);
            Assert.IsType<UnauthorizedResult>(unauthorizedResult);
        }

        [Fact]
        public void GetSession_ShouldReturnOk_WhenSessionTokenIsValid()
        {
            // [1] Подготовка mock-объектов
            _mockHttpContextAccessor.SetupGet(context => context.HttpContext.Request.Headers[HeaderNames.Authorization]).Returns("Bearer valid_token");
            _authenticateController.ControllerContext.HttpContext = _mockHttpContextAccessor.Object.HttpContext;

            var sessionDto = new SessionDto { SessionToken = "valid_token", SessionId = 1 };
            _mockAuthenticateService.Setup(service => service.GetSession("valid_token")).Returns(sessionDto);

            // [2] Вызов метода
            var result = _authenticateController.GetSession();

            // [3] Проверка результата
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.IsType<OkObjectResult>(okResult);
            Assert.Equal(sessionDto, okResult.Value);
        }

        [Fact]
        public void GetSession_ShouldReturnUnauthorized_WhenSessionTokenIsInvalid()
        {
            // [1] Подготовка mock-объектов
            _mockHttpContextAccessor.SetupGet(context => context.HttpContext.Request.Headers[HeaderNames.Authorization]).Returns("Bearer invalid_token");
            _authenticateController.ControllerContext.HttpContext = _mockHttpContextAccessor.Object.HttpContext;

            _mockAuthenticateService.Setup(service => service.GetSession("invalid_token")).Returns((SessionDto)null);

            // [2] Вызов метода
            var result = _authenticateController.GetSession();

            // [3] Проверка результата
            var unauthorizedResult = result as UnauthorizedResult;
            Assert.NotNull(unauthorizedResult);
            Assert.IsType<UnauthorizedResult>(unauthorizedResult);
        }
    }
}
