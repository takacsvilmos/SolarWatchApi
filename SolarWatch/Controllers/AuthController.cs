﻿using Microsoft.AspNetCore.Mvc;
using SolarWatch.Contracts;
using SolarWatch.Services.Authentication;

namespace SolarWatch.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("Register")]
        public async Task<ActionResult<RegistrationResponse>> Register(RegistrationRequest request)
        {
            if(!ModelState.IsValid) 
            { 
                return BadRequest(ModelState);    
            }

            var result = await _authService.RegisterAsync(request.Email, request.Username, request.Password, "User");
            if(!result.Success) 
            {
                AddErrors(result);
                return BadRequest(ModelState);
            }
            return CreatedAtAction(nameof(Register), new RegistrationResponse(result.Email, result.UserName));

        }
        private void AddErrors(AuthResult result)
        {
            foreach(var error in result.ErrorMessages) 
            {
                ModelState.AddModelError(error.Key, error.Value);
            }
        }

        [HttpPost("Login")]
        public async Task<ActionResult<AuthResponse>> Authenticate([FromBody] AuthRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _authService.LoginAsync(request.Email, request.Password);

            if (!result.Success)
            {
                AddErrors(result);
                return BadRequest(ModelState);
            }

            return Ok(new AuthResponse(result.Email, result.UserName, result.Token));
        }
    }
}
