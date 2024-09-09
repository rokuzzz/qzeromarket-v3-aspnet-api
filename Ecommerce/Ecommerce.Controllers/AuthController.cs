using Ecommerce.Domain.Models;
using Ecommerce.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Ecommerce.Services.AuthService.DTO;
using Ecommerce.Services.AuthService.Intefaces;
using FluentValidation.Results;
using static Ecommerce.Services.AuthService.DTO.RegisterDto;
using static Ecommerce.Services.AuthService.DTO.LoginDto;
using Ecommerce.Services.Common.Interfaces;

namespace Ecommerce.Controllers
{
    [ApiController]
    [Route("api/v1/auth")]
    [SwaggerTag("User authentication and registration")]
    public class AuthController(IAuthService authService, IFileService fileService) : ControllerBase
    {
        private readonly IAuthService _authService = authService;
        private readonly IFileService _fileService = fileService;

        [HttpPost("register")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Register a new user")]
        [SwaggerResponse(200, "The user was registered successfully", typeof(IRegisterResult))]
        [SwaggerResponse(400, "Validation failed", typeof(List<ValidationFailure>))]
        [SwaggerResponse(409, "User already exists", typeof(ProblemDetails))]
        public async Task<ActionResult<User>> Register([FromForm] RegisterDto registerDto)
        {
            var validationResult = new RegisterDtoValidator().Validate(registerDto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
            var relativeFilePath = await _fileService.SaveFileAsync(registerDto.Avatar, "avatars");
            registerDto.AvatarPath = relativeFilePath;
            var user = await _authService.RegisterAsync(registerDto);

            return Ok(user);
        }

        [HttpPost("login")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Login a user")]
        [SwaggerResponse(200, "The user was logged in successfully", typeof(ILoginResult))]
        [SwaggerResponse(400, "Validation failed", typeof(List<ValidationFailure>))]
        [SwaggerResponse(404, "Not Found - user does not exist", typeof(ProblemDetails))]
        public async Task<ActionResult<ILoginResult>> Login([FromBody] LoginDto loginDto)
        {
            var validationResult = new LoginDtoValidator().Validate(loginDto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
            var result = await _authService.LoginAsync(loginDto);

            return Ok(result);
        }
    }
}