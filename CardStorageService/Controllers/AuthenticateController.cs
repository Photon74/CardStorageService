﻿using CardStorageService.Models;
using CardStorageService.Models.Requests;
using CardStorageService.Models.Responses;
using CardStorageService.Services;
using Castle.Core.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace CardStorageService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly IAuthenticateService _authenticateService;

        public AuthenticateController(IAuthenticateService authenticateService)
        {
            _authenticateService = authenticateService;
        }

        [HttpPost("Login")]
        public IActionResult Login([FromBody] AuthenticationRequest authenticationRequest)
        {
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
                //if (sessionToken.IsNullOrEmpty()) return Unauthorized();
                if (string.IsNullOrEmpty(sessionToken))
                    return Unauthorized();

                var sessionInfo = _authenticateService.GetSessionInfo(sessionToken);
                if (sessionInfo == null)
                    return Unauthorized();

                return Ok(sessionInfo);
            }

            return Unauthorized();
        }
    }
}
