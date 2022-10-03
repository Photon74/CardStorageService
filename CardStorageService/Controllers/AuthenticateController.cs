using CardStorageService.Models;
using CardStorageService.Models.Requests;
using CardStorageService.Models.Responses;
using CardStorageService.Services;
using Castle.Core.Internal;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace CardStorageService.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly IAuthenticateService _authenticateService;
        private readonly IValidator<AuthenticationRequest> _validator;

        public AuthenticateController(IAuthenticateService authenticateService,
                                      IValidator<AuthenticationRequest> validator)
        {
            _authenticateService = authenticateService;
            _validator = validator;
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public IActionResult Login([FromBody] AuthenticationRequest authenticationRequest)
        {
            var validationResult = _validator.Validate(authenticationRequest);
            if (!validationResult.IsValid) return BadRequest(validationResult.ToDictionary());

            AuthenticationResponse authenticationResponse = _authenticateService.Login(authenticationRequest);
            if (authenticationResponse.Status == AuthenticationStatus.Success)
            {
                Response.Headers.Add("X-Session-Token", authenticationResponse.SessionInfo.SessionToken);
            }
            return Ok(authenticationResponse);
        }

        [HttpGet("session")]
        public IActionResult GetSessionInfo()
        {
            var autherization = Request.Headers.Authorization;

            if (AuthenticationHeaderValue.TryParse(autherization, out var headerValue))
            {
                var scheme = headerValue.Scheme; // "Bearer"
                var sessionToken = headerValue.Parameter; // Token

                if (string.IsNullOrEmpty(sessionToken))
                    return Unauthorized();

                var sessionInfo = _authenticateService.GetSessionInfo(sessionToken);

                return sessionInfo == null
                    ? Unauthorized()
                    : Ok(sessionInfo);
            }

            return Unauthorized();
        }
    }
}
